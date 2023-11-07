using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SqlScriptMerge.Lib.Tests.Options;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace SqlScriptMerge.Lib.Tests;
public class MainProcessorTests
{
    private ILogger _loggerMock;
    
    public MainProcessorTests()
    {
        _loggerMock = new Mock<ILogger>().Object;
    }


    [Fact]
    public void RunCurrentOptions_SortMode_OpenResultUsingShell()
    {
        var opt = new SqlScriptMerge.Lib.Tests.Options.Options
        {
            SortMode = true,
            OutputFileName = OutputSqlFilename,
            InDirectory = ".\\TestFiles\\SortTest01\\"
        };


        var mainProcessor = new MainProcessor(opt, _loggerMock);
        mainProcessor.RunCurrentOptions();

        OpenFile(OutputSqlFilename);
    }


}
