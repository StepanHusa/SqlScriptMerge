using CommandLine;
using spOne.Desktop.Commons.Styling.XamlDictionaryMergeTool;
using SqlScriptMerge.Console;
using SqlScriptMerge.Lib;

namespace SqlScriptMerge.Program;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                var logger = new Logger(); // just a normal console.
                try
                {
                    var proc = new MainProcessor(options, logger);
                    proc.StartMergeProcess();
                }
                catch (Exception ex)
                {
                    logger.Log("Exception: " + ex.ToString());
                    logger.Log("Press Enter to exit.");
                    System.Console.ReadLine();
                }
            });

    }


}