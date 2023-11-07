using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Options;
public interface IOptions
{
    string? SortOutputFileName { get; set; }
    string? MergeOutputFileName { get; set; }
    bool NoMergeComments { get; set; }
    bool NoAuthorComments { get; set; }
    bool CustomSpLoad { get; set; }
    bool SortMode { get; set; }
    bool MergeMode { get; set; }
    string? InDirectory { get; set; }
    string? FileExtension { get; set; }
}
