using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Options;
public interface IOptions
{
    string? OutputFileName { get; set; }
    bool NoComments { get; set; }
    bool CustomSp { get; set; }
    string? AutoReadPrefix { get; set; }
}
