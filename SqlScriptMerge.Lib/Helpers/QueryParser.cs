using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlMergeTool
{
    internal static class QueryParser
    {
        public static void tTEY()
        {
            TSqlParser parser = new TSql120Parser(false);
            TSqlFragment fragment = parser.Parse(new StringReader("ALTER TABLE TableName ADD ColumnName INT"), out var errors);

            foreach(var e in errors)
            {
                Console.WriteLine(e.Message);
            }

            AlterTableVisitor visitor = new AlterTableVisitor();
            fragment.Accept(visitor);
        }
    }

    public class AlterTableVisitor : TSqlFragmentVisitor
    {
        public override void Visit(AlterTableAlterColumnStatement node)
        {
            // Extract column name, data type, and other properties
            string columnName = node.ColumnIdentifier.Value;
            //DataTypeReference dataType = node.DataType;
            // ... extract other properties

            // Your logic to process the extracted information
        }
    }

}
