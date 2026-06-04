using System.Windows;
using System.Windows.Input;

namespace JsonTagger.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        StateChanged += (_, _) => UpdateMaxRestoreIcon();
    }

    // ── Title bar drag + double-click maximize ──────────────────────────────

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
            ToggleMaximize();
        else
            DragMove();
    }

    // ── Chrome buttons ──────────────────────────────────────────────────────

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void MaxRestoreBtn_Click(object sender, RoutedEventArgs e)
        => ToggleMaximize();

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => Close();

    // ── Theme toggle ────────────────────────────────────────────────────────

    private void ThemeToggleBtn_Click(object sender, RoutedEventArgs e)
    {
        ThemeManager.Toggle();
        ThemeIcon.Text = ThemeManager.IsRetroLight ? "\uE708" : "\uE706";
        ThemeToggleBtn.ToolTip = ThemeManager.IsRetroLight
            ? "Switch to dark theme"
            : "Switch to retro light theme";
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private void ToggleMaximize()
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void UpdateMaxRestoreIcon()
        => MaxRestoreBtn.Content = WindowState == WindowState.Maximized
            ? "\uE923"   // Restore
            : "\uE922";  // Maximize
}
