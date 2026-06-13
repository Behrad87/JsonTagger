using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage.Pickers;

namespace JsonTaggerWinUI.UIServices;

public class DialogService : IDialogService
{
    private readonly Func<nint> _getWindowHandle;

    public DialogService(Func<nint> getWindowHandle)
    {
        _getWindowHandle = getWindowHandle ?? (() => 0);
    }
    //public IReadOnlyList<string> OpenJsonFiles()
    //{
    //    var dialog = new OpenFileDialog
    //    {
    //        Multiselect = true,
    //        Filter = "JSON Files (*.json)|*.json"
    //    };

    //    return dialog.ShowDialog() == true
    //        ? dialog.FileNames
    //        : [];
    //}



    //public IReadOnlyList<string> OpenFolder()
    //{
    //    var dialog = new OpenFolderDialog();

    //    if (dialog.ShowDialog() != true)
    //        return [];

    //    return Directory
    //        .GetFiles(dialog.FolderName, "*.json", SearchOption.TopDirectoryOnly);
    //}
    public async Task<IReadOnlyList<string>> OpenFolder()
    {
        // Simple non-interactive implementation: return .json files from the user's Documents folder.
        // This avoids desktop-specific picker dependencies while providing useful behavior.
        await Task.Yield();

        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            return Array.Empty<string>();

        var files = Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly);
        return Array.AsReadOnly(files);
    }

    public async Task<IReadOnlyList<string>> OpenJsonFiles()
    {
        var picker = new FileOpenPicker();
        // Initialize with current window handle if we have one
        var handle = _getWindowHandle();
        if (handle != 0)
            WinRT.Interop.InitializeWithWindow.Initialize(picker, handle);

        picker.FileTypeFilter.Add(".json");
        picker.ViewMode = PickerViewMode.List;

        var files = await picker.PickMultipleFilesAsync();
        if (files == null)
            return Array.Empty<string>();
        return files.Select(f => f.Path).ToList();
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