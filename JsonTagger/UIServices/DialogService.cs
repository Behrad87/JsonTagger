using Microsoft.Win32;
using System;
using System.IO;

namespace JsonTagger.UIServices;

public class DialogService : IDialogService
{
  
    public IReadOnlyList<string> OpenJsonFiles()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Multiselect = true,
            Filter = "JSON Files (*.json)|*.json"
        };

        return dialog.ShowDialog() == true
            ? dialog.FileNames
            : Array.Empty<string>();
    }



    public IReadOnlyList<string> OpenFolder()
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog();

        var result = dialog.ShowDialog();
        if (result != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
            return Array.Empty<string>();

        return Directory.GetFiles(dialog.SelectedPath, "*.json", SearchOption.TopDirectoryOnly);
    }
}


//public class DialogServiceWinUI : IDialogService
//{
//    public async Task<IReadOnlyList<string>> OpenJsonFilesAsync()
//    {
//        var picker = new FileOpenPicker();
//        WinRT.Interop.InitializeWithWindow.Initialize(picker, MainWindowHandle); // Need HWND helper

//        picker.FileTypeFilter.Add(".json");
//        picker.ViewMode = PickerViewMode.List;

//        var files = await picker.PickMultipleFilesAsync();
//        return files?.Select(f => f.Path).ToList() ?? [];
//    }

//    // Similar for folder picker using FolderPicker
//}