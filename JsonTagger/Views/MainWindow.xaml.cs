using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JsonTagger.Views;

public partial class MainWindow : Window
{
    // ── Lifecycle ────────────────────────────────────────────────────────────

    public MainWindow()
    {
        InitializeComponent();

        StateChanged += (_, _) => UpdateMaxRestoreIcon();

        // Keep the theme icon in sync whenever the theme changes
        // (handles changes triggered from anywhere, not just this window)
        ThemeManager.ThemeChanged += (_, theme) => UpdateThemeIcon(theme);
        UpdateThemeIcon(ThemeManager.CurrentTheme);
    }

    public JsonTagger.Controls.NotificationHost NotificationHostControl => NotificationHost;

    // ── Title bar ────────────────────────────────────────────────────────────

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
            ToggleMaximize();
        else
            DragMove();
    }

    // ── Chrome buttons ───────────────────────────────────────────────────────

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void MaxRestoreBtn_Click(object sender, RoutedEventArgs e)
        => ToggleMaximize();

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => Close();

    // ── Theme picker ─────────────────────────────────────────────────────────

    private void ThemeToggleBtn_Click(object sender, RoutedEventArgs e)
    {
        var menu = BuildThemeMenu();

        // Attach and open below the button
        menu.PlacementTarget = ThemeToggleBtn;
        menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        menu.IsOpen = true;
    }

    /// <summary>
    /// Builds the theme picker ContextMenu dynamically so the active-check
    /// mark always reflects the current selection, even after hot-swapping.
    /// </summary>
    private ContextMenu BuildThemeMenu()
    {
        var menu = new ContextMenu
        {
            Style = (Style)Resources["ThemeMenuStyle"]
        };

        foreach (var theme in ThemeManager.AvailableThemes)
        {
            var item = new MenuItem
            {
                Header = theme.DisplayName,
                Style = (Style)Resources["ThemeMenuItemStyle"],
                // DataContext carries the IsActive flag for the checkmark trigger
                DataContext = new ThemeMenuItemVm(
                    theme,
                      theme == ThemeManager.CurrentTheme)
            };

            item.Click += (_, _) =>
            {
                ThemeManager.Apply(theme);
                menu.IsOpen = false;
            };

            menu.Items.Add(item);
        }

        return menu;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void ToggleMaximize()
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void UpdateMaxRestoreIcon()
        => MaxRestoreBtn.Content = WindowState == WindowState.Maximized
            ? "\uE923"   // Restore
            : "\uE922";  // Maximize

    private void UpdateThemeIcon(AppTheme theme) =>
        ThemeIcon.Text = theme.Id switch
        {
            "Dark" => "🌙",
            "Light" => "☀️",
            "Retro" => "📷",
            _ => "🎨"
        };

    // ── Nested helper VM (lightweight, no full MVVM needed for a menu item) ──

    private sealed record ThemeMenuItemVm(AppTheme Theme, bool IsActive);
}
