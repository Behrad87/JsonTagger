using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JsonTagger.UIServices;

public class DialogService : IDialogService
{
  
    public Task<IReadOnlyList<string>> OpenJsonFiles()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Multiselect = true,
            Filter = "JSON Files (*.json)|*.json"
        };

        var res = dialog.ShowDialog() == true ? dialog.FileNames : Array.Empty<string>();
        return Task.FromResult((IReadOnlyList<string>)res);
    }



    public Task<IReadOnlyList<string>> OpenFolder()
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog();

        var result = dialog.ShowDialog();
        if (result != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
            return Task.FromResult((IReadOnlyList<string>)Array.Empty<string>());

        var files = Directory.GetFiles(dialog.SelectedPath, "*.json", SearchOption.TopDirectoryOnly);
        return Task.FromResult((IReadOnlyList<string>)files);
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