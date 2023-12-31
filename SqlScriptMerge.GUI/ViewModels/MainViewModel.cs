﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SqlScriptMerge.GUI.Extensions;
using SqlScriptMerge.GUI.Models;
using SqlScriptMerge.Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScriptMerge.GUI.ViewModels;
internal partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _sortMode;
    [ObservableProperty]
    private bool _mergeMode;

    [ObservableProperty]
    private bool _autogeneratedComments = true;

    [ObservableProperty]
    private bool _authorComments = true;

    [ObservableProperty]
    private ObservableCollection<FileModel> _files = new();

    [ObservableProperty]
    private Logger _logger = new Logger();

    [RelayCommand]
    private void OnOpenFiles()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Multiselect = true;

        var res = ofd.ShowDialog();
        if (res == null || res == false) return;

        var list = ofd.FileNames.Select(u => new FileModel { FullName = u, DisplayName = Path.GetFileName(u) });

        Files.AddRange(list);
    }

    [RelayCommand]
    private void OnClearFiles()
    {
        Files.Clear();
    }

    [RelayCommand]
    private void OnRunSort()
    {
        var opt = GetOptions();

        var proc = new MainProcessor(new Options.Options(),Logger);


            var output = proc.RunWithPreparedFilesSort(Files.Select(u => u.FullName));

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select file for sort output.";
            var res = sfd.ShowDialog();
            if (res == null || res == false) return;

            var fn = sfd.FileName;

            File.WriteAllText(fn,output);
    }

    [RelayCommand]
    private void OnRunMerge()
    {
        Logger.Log("Mergeing not implementeg yet.");

    }

    private Options.Options GetOptions()
    {
        return new Options.Options
        {
            NoAuthorComments = !AuthorComments,
            NoMergeComments = !AutogeneratedComments,

        };
    }

}
