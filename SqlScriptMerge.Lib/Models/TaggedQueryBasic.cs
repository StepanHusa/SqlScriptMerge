using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Lib.Models;
internal class TaggedQueryBasic : IQuery
{
    public required string Query { get; set; }
    public required int Order { get; set; }

    public string? FromFile { get; set; }
}
