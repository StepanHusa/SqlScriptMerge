using SqlScriptMerge.Lib.Models;
using SqlScriptMerge.Lib.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Helpers;
internal static class Merger
{

    public static string SortQueriesByTableIntoOneFile(IEnumerable<TaggedQueryBasic> queries, bool mergeComments=true, bool authorComments = true, bool onlySort = false)
    {
        var categorized = CategoryHelper.CategorizeQueries(queries);
        var sb = new StringBuilder();


        // filter out uncategorized Queries
        var uncategorized = categorized.Where(q => q.Type == QueryType.NotRecognized);
        categorized = categorized.Where(q => q.Type != QueryType.NotRecognized);

        if (uncategorized.Any())
        {
            sb.AppendLine("--- BEGIN Uncategorized queries ---");

            foreach (var q in uncategorized)
            {
                sb.AppendLine(!mergeComments ? string.Empty : $@"/*From file: {q.FromFile}*/");
                sb.AppendLine(q.Query);
            }
            sb.AppendLine("--- END Uncategorized queries ---");
        }

        // filter out comment Queries
        var floatingComments = categorized.Where(q => q.Type == QueryType.Comment);
        categorized = categorized.Where(q => q.Type != QueryType.Comment);

        if (floatingComments.Any())
        {
            sb.AppendLine("--- BEGIN Floating Comments ---");

            foreach (var q in floatingComments)
            {
                sb.AppendLine(q.Query);
            }
            sb.AppendLine("--- END Floating Comments ---");
        }

        // filter out use queries.
        var useQueries = categorized.Where(q => q.Type == QueryType.Use).DistinctBy(q => q.Table);
        categorized = categorized.Where(q => q.Type != QueryType.Use);

        if (useQueries.Count() > 1)
        {
            sb.AppendLine("Warning: more then one USE query, you have to place them correctly.");
        }
        foreach (var q in useQueries)
        {
            sb.AppendLine(q.Query);
        }

        // filter out view queries.
        var viewQueries = categorized.Where(q => q.Type == QueryType.DropView || q.Type == QueryType.CreateView);
        categorized = categorized.Where(q => !(q.Type == QueryType.DropView || q.Type == QueryType.CreateView));

        if (viewQueries.Any())
        {
            sb.AppendLine("--- BEGIN Veiws  ---");

            foreach (var q in viewQueries.OrderBy(q => q.Table))
            {
                sb.AppendLine(q.Query);
            }
            sb.AppendLine("--- END Veiws ---");
        }

        var queriesByTable = categorized.GroupBy(q => q.Table);
        foreach (var queryGroup in queriesByTable)
        {
            sb.AppendLine();
            sb.AppendLine(!mergeComments ? string.Empty : $@"/*Table: {queryGroup.Key}*/");
            foreach (var q in queryGroup)
            {
                sb.AppendLine(!mergeComments ? string.Empty : $@"/*From file: {q.FromFile}*/");
                sb.AppendLine(q.Query);
            }
        }

          return   sb.ToString();
    }

    static IEnumerable<TaggedQuery>? MergeQueriesOneTable(IEnumerable<TaggedQuery> queries)
    {
        TaggedQuery? create = null;
        List<TaggedQuery> alters = new();

        List<TaggedQuery> dataQueries = new();
        List<TaggedQuery> inconsistentDataQueries = new();

        List<TaggedQuery> indexies = new();

        foreach (var query in queries.OrderBy(q => q.Order))
        {
            switch (query.Type)
            {
                case QueryType.CreateTable:
                    if (create != null)
                        throw new Exception($"Two create queries for table {create.Table}");
                    create = query;
                    break;

                case QueryType.AlterTable:
                    if (create == null)
                        throw new Exception($"Table {query.Table} is altered before it is created.");
                    if (dataQueries.Count != 0)
                    {
                        inconsistentDataQueries.AddRange(dataQueries);
                        dataQueries = new();
                    }

                    alters.Add(query);
                    break;

                case QueryType.DropTable:
                    create = null;
                    alters = new();
                    dataQueries = new();
                    inconsistentDataQueries = new();
                    indexies = new();
                    break;

                case QueryType.DeleteData:
                case QueryType.UpdateData:
                case QueryType.InsertData:
                    if (create == null)
                        throw new Exception($"Workong with data of table {query.Table} before it was created.");

                    dataQueries.Add(query);
                    break;

                case QueryType.CreateIndex:
                case QueryType.DropIndex:
                    if (create == null)
                        throw new Exception($"Workong with indexes of table {query.Table} before it was created.");

                    indexies.Add(query);
                    break;

            }
        }

        if (create == null)
            return null;

        var returnList = new List<TaggedQuery>
        {
            create,

        };

        return returnList;
    }
}
