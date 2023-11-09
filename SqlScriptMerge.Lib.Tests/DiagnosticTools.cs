using System.Diagnostics;

namespace SqlScriptMerge.Lib.Tests;
internal static class DiagnosticTools
{
    public static void OpenFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            new Process() { StartInfo = new ProcessStartInfo(fileName) { UseShellExecute = true } }.Start();
        }
        else
        {
            Console.WriteLine($"File {fileName} not found.");
        }
    }

    public static void OpenFileInCode(string fileName)
    {
        if (File.Exists(fileName))
        {
            Process.Start("C:\\Users\\husaS\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe", fileName);
        }
        else
        {
            Console.WriteLine($"File {fileName} not found.");
        }
    }
}
