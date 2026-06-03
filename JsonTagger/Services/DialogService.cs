using Microsoft.Win32;

using System.Windows;

namespace JsonTagger.Services;

public class DialogService : IDialogService
{
    public string? OpenJsonFile()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select JSON File",
            Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
            CheckFileExists = true
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
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
    public IEnumerable<string> OpenMultipleJsonFiles()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select JSON Files",
            Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
            Multiselect = true,
            CheckFileExists = true
        };
        return dialog.ShowDialog() == true ? dialog.FileNames : [];
    }

    public bool Confirm(string title, string message)
    {
        var result = MessageBox.Show(message, title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    public string? OpenFolder()
    {
        throw new NotImplementedException();
    }
}
