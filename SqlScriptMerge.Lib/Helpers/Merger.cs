using SqlMergeTool;
using SqlScriptMerge.Lib.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Helpers;
internal static class Merger
{

    public static string MergeQueriesByTable(IEnumerable<TaggedQuery> queries, bool mergeComments=true, bool authorComments = true, bool onlySort = false)
    {
        CategoryHelper.CategorizeQueries(queries);
        var progressSB = new StringBuilder();
        var realSB = new StringBuilder();


        // filter out uncategorized Queries
        var uncategorized = queries.Where(q => q.Type == QueryType.NotRecognized);
        queries = queries.Where(q => q.Type != QueryType.NotRecognized);

        if (uncategorized.Any())
        {
            progressSB.AppendLine("--- BEGIN Uncategorized queries ---");
            realSB.AppendLine("--- BEGIN Uncategorized queries ---");

            foreach (var q in uncategorized)
            {
                progressSB.AppendLine(mergeComments ? string.Empty : $@"/*From file: {q.FromFile}*/");
                progressSB.AppendLine(q.Query);
                realSB.AppendLine(mergeComments ? string.Empty : $@"/*From file: {q.FromFile}*/");
                realSB.AppendLine(q.Query);
            }
            progressSB.AppendLine("--- END Uncategorized queries ---");
            realSB.AppendLine("--- END Uncategorized queries ---");
        }

        // filter out comment Queries
        var floatingComments = queries.Where(q => q.Type == QueryType.Comment);
        queries = queries.Where(q => q.Type != QueryType.Comment);

        if (floatingComments.Any())
        {
            progressSB.AppendLine("--- BEGIN Floating Comments ---");
            realSB.AppendLine("--- BEGIN Floating Comments ---");

            foreach (var q in floatingComments)
            {
                progressSB.AppendLine(q.Query);
                realSB.AppendLine(q.Query);
            }
            progressSB.AppendLine("--- END Floating Comments ---");
            realSB.AppendLine("--- END Floating Comments ---");
        }

        // filter out use queries.
        var useQueries = queries.Where(q => q.Type == QueryType.Use).DistinctBy(q => q.Table);
        queries = queries.Where(q => q.Type != QueryType.Use);

        if (useQueries.Count() > 1)
        {
            progressSB.AppendLine("Warning: more then one USE query, you have to place them correctly.");
            realSB.AppendLine("Warning: more then one USE query, you have to place them correctly.");
        }
        foreach (var q in useQueries)
        {
            progressSB.AppendLine(q.Query);
            realSB.AppendLine(q.Query);
        }

        // filter out view queries.
        var viewQueries = queries.Where(q => q.Type == QueryType.DropView || q.Type == QueryType.CreateView);
        queries = queries.Where(q => !(q.Type == QueryType.DropView || q.Type == QueryType.CreateView));

        if (viewQueries.Any())
        {
            progressSB.AppendLine("--- BEGIN Veiws  ---");
            realSB.AppendLine("--- BEGIN Veiws (merging Views is not supported) ---");

            foreach (var q in viewQueries.OrderBy(q => q.Table))
            {
                progressSB.AppendLine(q.Query);
                realSB.AppendLine(q.Query);
            }
            progressSB.AppendLine("--- END Veiws ---");
            realSB.AppendLine("--- END Veiws ---");
        }

        var queriesByTable = queries.GroupBy(q => q.Table);
        foreach (var queryGroup in queriesByTable)
        {
            progressSB.AppendLine();
            progressSB.AppendLine(mergeComments ? string.Empty : $@"/*Table: {queryGroup.Key}*/");
            foreach (var q in queryGroup)
            {
                progressSB.AppendLine(mergeComments ? string.Empty : $@"/*From file: {q.FromFile}*/");
                progressSB.AppendLine(q.Query);
            }

            if (!onlySort) //TODO only sort
            {
                progressSB.AppendLine();
                progressSB.AppendLine(mergeComments ? string.Empty : $@"/*Table: {queryGroup.Key}*/");

                var merged = MergeQueriesOneTable(queryGroup);
                if (merged == null)
                {

                    progressSB.AppendLine($@"/*Was deleted.*/");

                    continue;
                }

                foreach (var q in merged)
                {
                    realSB.AppendLine(q.Query);
                }
            }
        }

        if (!onlySort) { }
            //File.WriteAllText(Settings.Options.OutputFileName ?? "Merged.sql", realSB.ToString()); //TODO write it somewhere

        //if (!Settings.Options.NoSortOutput && !Settings.Options.CustomSpOne)
          return   progressSB.ToString();
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
