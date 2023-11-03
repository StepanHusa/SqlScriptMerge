using SqlScriptMerge.Lib.Models.Enums;

namespace SqlMergeTool
{
    public class TaggedQuery
    {
        public required string Query { get; set; }
        public string FromFile { get; set; }
        public required int Order { get; set; }
        public QueryType Type { get; set; }

        
        public string Table { get; set; }
    }
}
