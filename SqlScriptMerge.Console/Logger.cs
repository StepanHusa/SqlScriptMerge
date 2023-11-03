using SqlScriptMerge.Lib;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.Console;
internal class Logger : ILogger
{
    public void Log(string message)
    {
        System.Console.WriteLine(message);
    }
}
