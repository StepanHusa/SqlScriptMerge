using SqlScriptMerge.Lib.Helpers;
using SqlScriptMerge.Lib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib;
public class MainProcessor
{

    private  IOptions _options;
    private readonly ILogger _logger;

    public MainProcessor(IOptions options, ILogger logger)
    {
        _options = options;
        _logger = logger;
    }

    public void StartMergeProcess()
    {
        var files = _options.CustomSp ? CustomSpOneOptions() : LoadFromDir();

        var queries = FileHelper.ExtractQueriesFromFiles(files);
        var output = Merger.MergeQueriesByTable(queries);

        File.WriteAllText("SortedFiles.sql", output);
    }



    private IEnumerable<string> CustomSpOneOptions()
    {
        var ignore = ".00 00 schema,users.sql";

        var dir = Environment.CurrentDirectory;
        var files = Directory.GetFiles(dir).Where(f
            => f.EndsWith(".sql")
            && Path.GetFileName(f).Split('.')[0].All(char.IsDigit)
            && !f.EndsWith(ignore));


        int lastVersion = files.Max(f => Convert.ToInt32(Path.GetFileName(f).Split('.')[0]));

        _options.OutputFileName = "automerge_" + (lastVersion + 1) + ".00 01 sturcture.sql";

        return files.Where(f => Convert.ToInt32(Path.GetFileName(f).Split('.')[0]) == lastVersion).OrderBy(f => Path.GetFileName(f));
    }

    private IEnumerable<string> LoadFromDir()
    {
        var prefix = Path.GetFileName(_options.AutoReadPrefix);
        var dir = Path.Join(Environment.CurrentDirectory, Path.GetDirectoryName(_options.AutoReadPrefix));
        if (!Directory.Exists(dir)) { throw new Exception(); }

        var files = Directory.GetFiles(dir);

        return files.Where(f => f.EndsWith(".sql") && (prefix == null || Path.GetFileName(f).StartsWith(prefix)));
    }
}
