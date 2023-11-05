using SqlScriptMerge.Lib.Models;
using SqlScriptMerge.Lib.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static SqlScriptMerge.Lib.MapperInstance;

namespace SqlScriptMerge.Lib.Helpers;
internal static class CategoryHelper
{
    public static IEnumerable<TaggedQuery> CategorizeQueries(IEnumerable<TaggedQueryBasic> queries)
    {
        var result = queries.Select(q => {
            var tq = Mapper.Map<TaggedQuery>(q);
            (tq.Table, tq.Type) = DetermineQueryTypeAndTable(q.Query);
            return tq;
        });

        return result;
    }

    public static (string, QueryType) DetermineQueryTypeAndTable(string query)
    {
        QueryType qt;
        string? tableName = null;

        if (false)
        {
        }
        else if (ContainsSubstring(query, "CREATE TABLE"))
        {
            qt = QueryType.CreateTable;
            tableName = ExtractTableNameAfterWord(query, "TABLE");
        }
        else if (ContainsSubstring(query, "ALTER TABLE"))
        {
            qt = QueryType.AlterTable;
            tableName = ExtractTableNameAfterWord(query, "TABLE");
        }
        else if (ContainsSubstring(query, "DROP TABLE"))
        {
            qt = QueryType.DropTable;
            tableName = ExtractTableNameAfterWord(query, "TABLE");
        }
        else if (ContainsSubstring(query, "INSERT INTO") || ContainsSubstring(query, "INSERT IGNORE INTO"))
        {
            qt = QueryType.InsertData;
            tableName = ExtractTableNameAfterWord(query, "INTO");
        }
        else if (ContainsSubstring(query, "UPDATE"))
        {
            qt = QueryType.UpdateData; // Should this be QueryType.UpdateData?
            tableName = ExtractTableNameAfterWord(query, "UPDATE");
        }
        else if (ContainsSubstring(query, "DELETE FROM"))
        {
            qt = QueryType.DeleteData;
            tableName = ExtractTableNameAfterWord(query, "DELETE FROM");
        }
        else if (ContainsSubstring(query, "CREATE INDEX") || ContainsSubstring(query, "CREATE UNIQUE INDEX"))
        {
            qt = QueryType.CreateIndex;
            tableName = ExtractTableNameAfterWord(query, "ON");
        }
        else if (ContainsSubstring(query, "DROP INDEX"))
        {
            qt = QueryType.DropIndex;
            tableName = ExtractTableNameAfterWord(query, "ON");
        }
        else if (ContainsSubstring(query, "CREATE VIEW"))
        {
            qt = QueryType.CreateView;
            tableName = ExtractTableNameAfterWord(query, "VIEW");
        }
        else if (ContainsSubstring(query, "DROP VIEW"))
        {
            qt = QueryType.DropView;
            tableName = ExtractTableNameAfterWord(query, "VIEW");
        }
        else if (ContainsSubstring(query, "USE"))
        {
            qt = QueryType.Use;
            tableName = ExtractTableNameAfterWord(query, "USE");
        }
        else if (ContainsSubstring(query, "SELECT"))
        {
            qt = QueryType.SelectData;
            tableName = ExtractTableNameAfterWord(query, "FROM");
            throw new Exception($"not able to process SELECT query {query}");
        }
        else if (Regex.IsMatch(query, @"(--[^\n] *|\/\*[\s\S] *?\*\/)", RegexOptions.IgnoreCase))
        {
            qt = QueryType.Comment;
            tableName = string.Empty;
        }
        else
        {
            qt = QueryType.NotRecognized;
            tableName = string.Empty;
        }

        return (tableName, qt);
    }

    public static bool ContainsSubstring(string query, string substr)
    {
        return Regex.IsMatch(query, $@"\b{substr}\b", RegexOptions.IgnoreCase);
    }

    static string ExtractTableNameAfterWord(string input, string word)
    {
        //string pattern = @"TABLE\s+('?)(\w+)\1";
        //string pattern = word + @"\s+('?)(\w+)\1";
        string pattern = $@"{word}\s+(IF NOT EXISTS\s+)?('?)(\w+)\2";

        Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        if (match.Success)
        {
            string extractedWord = match.Groups[3].Value;
            return extractedWord;
        }
        else
        {
            throw new Exception($"Not able to extract table name from a query \" {input} \" Make sure to only merge structural queries.");
        }
    }

}
