using SqlScriptMerge.Lib;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.ConsoleApp
{
    internal class Logger : ILogger
    {
        public bool EncounteredException { get; private set; }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, Exception exception)
        {
            EncounteredException = true;
            Log(message);
        }
    }
}