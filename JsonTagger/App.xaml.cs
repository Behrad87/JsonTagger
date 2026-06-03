
using System.Windows;
using JsonTagger.Services;
using JsonTagger.ViewModels;
using JsonTagger.Views;

using Microsoft.Extensions.DependencyInjection;

namespace JsonTagger;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var sc = new ServiceCollection();
        sc.AddSingleton<IJsonFileService, JsonFileService>();
        sc.AddSingleton<IDialogService, DialogService>();
        sc.AddSingleton<INotificationService, NotificationService>();
        sc.AddSingleton<MainViewModel>();
        Services = sc.BuildServiceProvider();

        var win = new MainWindow
        {
            DataContext = Services.GetRequiredService<MainViewModel>()
        };
        win.Show();
    }
}
