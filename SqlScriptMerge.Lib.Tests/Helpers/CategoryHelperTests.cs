using SqlScriptMerge.Lib.Helpers;
using SqlScriptMerge.Lib.Models.Enums;

namespace SqlScriptMerge.Lib.Tests.Helpers;

public class CategoryHelperTests
{
    [Theory]
    [InlineData("CreateTable\\01.test", QueryType.CreateTable, "Opravneni")]
    [InlineData("CreateTable\\02.test", QueryType.CreateTable, "globRole")]
    [InlineData("AlterTable\\01.test", QueryType.AlterTable, "Opravneni")]
    [InlineData("RenameTable\\01.test", QueryType.RenameTable, "zadavatel")]
    public void CategorizeQueries_ValidQuery_CorrectlyIdentifies(string file, QueryType expectedQueryType, string expectedTableName)
    {
        string querystr = File.ReadAllText("testfiles\\" + file);
        (string tableName, QueryType qt) = CategoryHelper.DetermineQueryTypeAndTable(querystr);

        Assert.Equal(expectedQueryType, qt);
        Assert.Equal(expectedTableName, tableName);
    }
}