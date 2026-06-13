using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;

namespace JsonTagger.Controls;

using JsonTagger.Lib.Services;

public partial class NotificationHost : UserControl, INotificationHost
{
    public NotificationHost()
    {
        InitializeComponent();
    }

    public void ShowToast(string message, string type)
    {
        var border = new Border
        {
            Background = type == "Success" ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.DarkRed),
            CornerRadius = new CornerRadius(6),
            Padding = new Thickness(12),
            Margin = new Thickness(0, 0, 0, 8)
        };

        var tb = new TextBlock { Text = message, Foreground = new SolidColorBrush(Colors.White) };
        border.Child = tb;

        // Start hidden then fade in
        border.Opacity = 0;
        ToastPanel.Children.Insert(0, border);

        var fadeIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
        border.BeginAnimation(UIElement.OpacityProperty, fadeIn);

        // Stay visible for 20 seconds then fade out
        _ = DismissAfterAsync(border, TimeSpan.FromSeconds(20));
    }

    private async Task DismissAfterAsync(UIElement element, TimeSpan delay)
    {
        await Task.Delay(delay);

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            // Fade out over 1s then remove
            var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(1000)));
            fadeOut.Completed += (s, e) => ToastPanel.Children.Remove(element);
            (element as UIElement)?.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        });
    }
}
