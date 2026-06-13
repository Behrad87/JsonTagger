using JsonTagger.Lib.Services;
using JsonTagger.UIServices;
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
        // Set the concrete NotificationHost instance on the proxy registered
        // earlier in BuildServices so tests can replace/mock it.
        try
        {
            var proxy = Services.GetService(typeof(INotificationHost)) as NotificationHostProxy;
            proxy?.SetTarget(window.NotificationHostControl);
        }
        catch { }
        // Wire notification service to the window's NotificationHost so toasts
        // are shown inside the WPF window.
        try
        {
            var notif = Services.GetService(typeof(INotificationService)) as NotificationService;
            if (notif != null)
            {
                var host = window.NotificationHostControl;
                    if (host != null)
                    {
                        notif.NotificationRaised += (msg, type) =>
                        {
                            System.Diagnostics.Debug.WriteLine($"WPF: Received notification {type} - {msg}");
                            // Ensure this runs on UI thread
                            Application.Current.Dispatcher.Invoke(() => host.ShowToast(msg, type.ToString()));
                        };
                    }
            }
        }
        catch
        {
            // If wiring fails, continue to show the window normally.
        }

        window.Show();
    }

    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();

        // Services
        services.AddSingleton<IJsonFileService, JsonFileService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INotificationService, NotificationService>();
        // Proxy host used so the UI layer can provide the real host instance
        services.AddSingleton<INotificationHost, NotificationHostProxy>();

        // ViewModels
        services.AddSingleton<MainViewModel>();

        return services.BuildServiceProvider();
    }
}
