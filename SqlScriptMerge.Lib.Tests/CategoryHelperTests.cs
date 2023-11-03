using SqlMergeTool;
using SqlScriptMerge.Lib.Helpers;
using SqlScriptMerge.Lib.Models.Enums;

namespace SqlScriptMerge.Lib.Tests;

public class CategoryHelperTests
{
    [Theory]
    [InlineData("01.test")]
    public void Test1(string file)
    {
        string querystr = File.ReadAllText("testfiles\\" + file);
        var query = new TaggedQuery { Query = querystr, Order = 1 };
        CategoryHelper.CategorizeQueries(new TaggedQuery[] { query });

        Assert.Equal(QueryType.CreateTable,query.Type);
    }
}