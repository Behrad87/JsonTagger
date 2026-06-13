// App.xaml.cs
using JsonTagger.WinUI.Views;

using Microsoft.UI.Xaml;

using System;

namespace JsonTagger.WinUI;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Services = BuildServices();

        var window = new MainWindow
        {
            DataContext = Services.GetRequiredService<MainViewModel>()
        };
        window.Activate();
    }

    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IJsonFileService, JsonFileService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<MainViewModel>();
        return services.BuildServiceProvider();
    }
}