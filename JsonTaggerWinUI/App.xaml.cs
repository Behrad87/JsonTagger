using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

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
        public static IServiceProvider Services { get; private set; } = null!;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
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
        }

        private static IServiceProvider BuildServices()
        {
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            // Register library services
            services.AddSingleton<JsonTagger.Lib.Services.IJsonFileService, JsonTagger.Lib.Services.JsonFileService>();

            // DialogService requires main window handle; resolve later via factory
            services.AddSingleton<JsonTaggerWinUI.UIServices.IDialogService>(sp =>
            {
                // Provide a default handle; MainWindow will re-register if needed.
                var handle = 0;
                return new JsonTaggerWinUI.UIServices.DialogService((nint)handle);
            });

            services.AddSingleton<JsonTagger.Lib.Services.INotificationService, JsonTagger.Lib.Services.NotificationService>();

            // ViewModels
            services.AddSingleton<JsonTaggerWinUI.ViewModels.MainViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
