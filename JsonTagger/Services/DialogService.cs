using Microsoft.Win32;

using System.Windows;

namespace JsonTagger.Services;

public class DialogService : IDialogService
{
  
    public IReadOnlyList<string> OpenJsonFiles()
    {
        var dialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "JSON Files (*.json)|*.json"
        };

        return dialog.ShowDialog() == true
            ? dialog.FileNames
            : [];
    }



    public IReadOnlyList<string> OpenFolder()
    {
        var dialog = new OpenFolderDialog();

        if (dialog.ShowDialog() != true)
            return [];

        return Directory
            .GetFiles(dialog.FolderName, "*.json", SearchOption.TopDirectoryOnly);
    }
}
