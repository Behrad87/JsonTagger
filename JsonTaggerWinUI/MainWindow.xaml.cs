using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace JsonTaggerWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // If DI is configured, set window DataContext and initialize dialog service handle
            try
            {
                var services = App.Services;
                if (services != null)
                {
                    // Set DataContext if not already set
                    if (RootGrid.DataContext == null && services.GetService(typeof(ViewModels.MainViewModel)) is JsonTaggerWinUI.ViewModels.MainViewModel vm)
                        RootGrid.DataContext = vm;

                    // If DialogService is registered, update its window handle so FileOpenPicker works
                    if (services.GetService(typeof(JsonTaggerWinUI.UIServices.IDialogService)) is JsonTaggerWinUI.UIServices.DialogService dlg)
                    {
                        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                        dlg.SetWindowHandle(hwnd);
                    }
                }
            }
            catch
            {
                // ignore DI setup errors at startup
            }
        }
    }
}
