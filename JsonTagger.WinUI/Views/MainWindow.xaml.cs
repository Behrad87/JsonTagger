// MainWindow.xaml.cs
using Microsoft.UI.Xaml;

namespace JsonTagger.WinUI.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
    }
}