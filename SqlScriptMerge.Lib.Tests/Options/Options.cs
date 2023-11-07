using SqlScriptMerge.Lib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Tests.Options;
public class Options:IOptions
{
    public string? OutputFileName { get; set; }
    public string? AutoReadPrefix { get; set; }
    public string? FileExtension { get ; set ; }
    public string? InDirectory { get; set; }
    public bool NoMergeComments { get; set; }
    public bool NoAuthorComments { get; set; }
    public bool CustomSpLoad { get; set; }
    public bool SortMode { get; set; }
    public bool MergeMode { get; set; }
}
