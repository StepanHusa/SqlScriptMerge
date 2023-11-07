using SqlScriptMerge.Lib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.GUI.Options;
internal class Options :IOptions
{
    public string? SortOutputFileName { get; set; }

    public string? MergeOutputFileName { get; set; }

    public string? InDirectory { get; set; }

    public string? FileExtension { get; set; }

    public bool SortMode { get; set; }

    public bool MergeMode { get; set; }

    public bool NoMergeComments { get; set; }

    public bool NoAuthorComments { get; set; }

    public bool CustomSpLoad { get; set; }
}
