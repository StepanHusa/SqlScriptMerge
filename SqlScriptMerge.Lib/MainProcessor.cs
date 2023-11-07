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

    public void RunCurrentOptions()
    {
        if(_options.OutputFileName == null)
        {
            _options.OutputFileName = "SqlScriptMergeResult.sql";
        }

        var files = _options.CustomSpLoad ? CustomSpOneOptions() : LoadFromDir();

        var queries = FileHelper.ExtractQueriesFromFiles(files);

        if (_options.SortMode)
        {
            var output = Merger.SortQueriesByTableIntoOneFile(queries);
            File.WriteAllText(_options.OutputFileName, output);
        }

        if (_options.MergeMode) {
            var output = "This option is not finished yet.";
            File.WriteAllText(_options.OutputFileName, output);

        }

        if(!_options.MergeMode && !_options.SortMode)
        {
            _logger.Log("If you want to create any output select one of (or both) the options -s (--SortMode), -m (--MergeMode).", new ArgumentException());
        }
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
        if(_options.InDirectory == null)
        {
            _options.InDirectory = ".\\";
        }
        var dir = Path.GetFullPath(_options.InDirectory);

        if (!Directory.Exists(dir)) { throw new Exception(); }

        var files = Directory.GetFiles(dir);

        return files.Where(f => ( _options.FileExtension == null || string.Equals(Path.GetExtension(f), _options.FileExtension, StringComparison.OrdinalIgnoreCase)));
    }
}
