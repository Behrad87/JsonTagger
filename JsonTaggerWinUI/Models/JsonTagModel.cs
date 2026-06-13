using CommunityToolkit.Mvvm.ComponentModel;

namespace JsonTaggerWinUI.Models;

public partial class JsonTagModel : ObservableObject
{
    [ObservableProperty]
    private string section = string.Empty;

    [ObservableProperty]
    private string key = string.Empty;

    [ObservableProperty]
    private string value = string.Empty;
}