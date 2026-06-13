using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

using JsonTagger.Lib.Services;

namespace JsonTaggerWinUI.Controls
{
    public sealed partial class NotificationHost : UserControl, INotificationHost
    {
        private StackPanel ToastPanel;

        public NotificationHost()
        {
            ToastPanel = new StackPanel { Orientation = Orientation.Vertical };
            var grid = new Grid { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(12) };
            grid.Children.Add(ToastPanel);
            this.Content = grid;
        }

        public void ShowToast(string message, string type)
        {
            var bgColor = type == "Success" ? Microsoft.UI.Colors.Green : Microsoft.UI.Colors.DarkRed;
            var border = new Border
            {
                Background = new SolidColorBrush(bgColor),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(12),
                Margin = new Thickness(0, 0, 0, 8),
                Opacity = 0
            };

            var tb = new TextBlock { Text = message, Foreground = new SolidColorBrush(Microsoft.UI.Colors.White) };
            border.Child = tb;

            ToastPanel.Children.Insert(0, border);

            // Fade-in (WinUI uses Composition implicitly via DoubleAnimation on UIElement)
            var fadeIn = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Microsoft.UI.Xaml.Duration(TimeSpan.FromMilliseconds(200))
            };

            var sb = new Microsoft.UI.Xaml.Media.Animation.Storyboard();
            Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(fadeIn, border);
            Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(fadeIn, "Opacity");
            sb.Children.Add(fadeIn);
            sb.Begin();

            _ = DismissAfterAsync(border, TimeSpan.FromSeconds(20));
        }

        private async Task DismissAfterAsync(UIElement element, TimeSpan delay)
        {
            await Task.Delay(delay);
            var dq = DispatcherQueue.GetForCurrentThread();
            dq.TryEnqueue(() =>
            {
                var fadeOut = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = new Microsoft.UI.Xaml.Duration(TimeSpan.FromMilliseconds(1000))
                };

                var sb = new Microsoft.UI.Xaml.Media.Animation.Storyboard();
                Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(fadeOut, element);
                Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(fadeOut, "Opacity");
                sb.Children.Add(fadeOut);
                sb.Completed += (s, e) => ToastPanel.Children.Remove(element);
                sb.Begin();
            });
        }
    }
}
