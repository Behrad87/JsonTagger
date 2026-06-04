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
 

    
    public string? OpenFolder()
    {
        throw new NotImplementedException();
    }
}
