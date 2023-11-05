using SqlScriptMerge.Lib.Models.Enums;

namespace SqlScriptMerge.Lib.Models
{
    internal class TaggedQuery : TaggedQueryBasic
    {
        public QueryType Type { get; set; }

        public required string Table { get; set; }
    }
}
