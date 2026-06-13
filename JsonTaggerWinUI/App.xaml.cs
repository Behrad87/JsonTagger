using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Dispatching;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace JsonTaggerWinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;
        public static nint MainWindowHandle { get; private set; }
        public static IServiceProvider Services { get; private set; } = null!;
        public static Exception? InitializationException { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"App.InitializeComponent failed: {ex}");
                InitializationException = ex;
            }
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Services = BuildServices();

            _window = new MainWindow();
            _window.Activate();

            // Store HWND for DialogService provider
            try
            {
                MainWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(_window);
            }
            catch { MainWindowHandle = 0; }

            // Wire notifications: find NotificationHost in MainWindow and subscribe to NotificationService
            try
            {
                var notif = Services.GetService(typeof(JsonTagger.Lib.Services.INotificationService)) as JsonTagger.Lib.Services.NotificationService;
                if (notif != null)
                {
                    var host = (_window as MainWindow)?.NotificationHost;
                    if (host != null)
                    {
                        notif.NotificationRaised += (msg, type) =>
                        {
                            System.Diagnostics.Debug.WriteLine($"WinUI: Received notification {type} - {msg}");
                            var dq = DispatcherQueue.GetForCurrentThread();
                            if (dq != null)
                                dq.TryEnqueue(() => host.ShowToast(msg, type.ToString()));
                            else
                                host.ShowToast(msg, type.ToString());
                        };
                    }
                }
            }
            catch { }

            // If a NotificationHost proxy was registered, set its target so DI consumers
            // get the real UI host instance.
            try
            {
                var proxy = Services.GetService(typeof(JsonTagger.Lib.Services.INotificationHost)) as JsonTagger.Lib.Services.NotificationHostProxy;
                var host = (_window as MainWindow)?.NotificationHost;
                if (proxy != null && host != null)
                    proxy.SetTarget(host);
            }
            catch { }

            // If initialization failed, show detailed dialog for debugging
            try
            {
                if (InitializationException != null)
                {
                    var dlg = new Microsoft.UI.Xaml.Controls.ContentDialog
                    {
                        Title = "XAML Initialization Error",
                        Content = InitializationException.ToString(),
                        CloseButtonText = "Close"
                    };

                    _ = dlg.ShowAsync();
                }
            }
            catch { }
        }

        private static IServiceProvider BuildServices()
        {
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            // Register library services
            services.AddSingleton<JsonTagger.Lib.Services.IJsonFileService, JsonTagger.Lib.Services.JsonFileService>();

            // Register WinUI DialogService with provider that reads App.MainWindowHandle
            services.AddSingleton<JsonTaggerWinUI.UIServices.IDialogService>(sp =>
            {
                return new JsonTaggerWinUI.UIServices.DialogService(() => MainWindowHandle);
            });

            services.AddSingleton<JsonTagger.Lib.Services.INotificationService, JsonTagger.Lib.Services.NotificationService>();

            // Proxy host so UI can provide the concrete instance after window creation
            services.AddSingleton<JsonTagger.Lib.Services.INotificationHost, JsonTagger.Lib.Services.NotificationHostProxy>();

            // ViewModels
            services.AddSingleton<JsonTaggerWinUI.ViewModels.MainViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
