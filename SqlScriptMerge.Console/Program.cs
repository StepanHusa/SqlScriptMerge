using CommandLine;
using SqlScriptMerge.Lib;

namespace SqlScriptMerge.ConsoleApp;

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
                    proc.RunCurrentOptions();
                }
                catch (Exception ex)
                {
                    logger.Log("Exception: " + ex.ToString());
                    logger.Log("Press Enter to exit.");
                    Console.ReadLine();
                }

                if (logger.EncounteredException)
                {
                    Console.ReadLine();
                }
            });

    }


}