using JsonTagger.Services;
using JsonTagger.ViewModels;
using JsonTagger.Views;

using Microsoft.Extensions.DependencyInjection;

using System.Windows;

namespace JsonTagger;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemeManager.LoadSaved();

        Services = BuildServices();

        var window = new MainWindow
        {
            DataContext = Services.GetRequiredService<MainViewModel>()
        };

        window.Show();
    }

    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();

        // Services
        services.AddSingleton<IJsonFileService, JsonFileService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INotificationService, NotificationService>();

        // ViewModels
        services.AddSingleton<MainViewModel>();

        return services.BuildServiceProvider();
    }
}
