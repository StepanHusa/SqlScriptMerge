using CommunityToolkit.Mvvm.ComponentModel;
using SqlScriptMerge.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.GUI;
internal partial class Logger : ObservableObject, ILogger
{
    [ObservableProperty]
    private string _outputLogString = "";


    public void Log(string message)
    {
        OutputLogString += message + '\n';
    }

    public void Log(string message, Exception exception)
    {
        Log(message);
    }
}
