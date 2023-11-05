
using SqlScriptMerge.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Helpers;
internal static class FileHelper
{
    public static List<TaggedQueryBasic> ExtractQueriesFromFiles(IEnumerable<string> scriptPaths)
    {
        var queries = new List<TaggedQueryBasic>();
        int counter = 1;

        foreach (string scriptPath in scriptPaths)
        {
            string scriptContent = File.ReadAllText(scriptPath);
            var queryArray = scriptContent.Split(';').SkipLast(1); // removes comments in the end of file TODO repair

            foreach (string query in queryArray)
            {
                string trimmedQuery = query.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedQuery))
                {
                    queries.Add(new TaggedQueryBasic { Query = trimmedQuery + ";", Order = counter, FromFile = Path.GetFileName(scriptPath) });
                    counter++;
                }
            }
        }
        return queries;

    }

}
