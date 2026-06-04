using System.Windows;

namespace JsonTagger;

/// <summary>
/// Swaps the active palette ResourceDictionary at runtime.
/// Works because every brush reference in XAML uses DynamicResource,
/// which re-evaluates whenever MergedDictionaries changes.
/// </summary>
public static class ThemeManager
{
    // Pack URIs are required when the XAML is compiled into the assembly (Build Action = Page).
    private const string DarkUri = "pack://application:,,,/JsonTagger;component/Themes/DarkTheme.xaml";
    private const string RetroUri = "pack://application:,,,/JsonTagger;component/Themes/RetroTheme.xaml";

    public static bool IsRetroLight { get; private set; }

    public static void Toggle()
    {
        IsRetroLight = !IsRetroLight;
        ApplyTheme(IsRetroLight ? RetroUri : DarkUri);
    }

    private static void ApplyTheme(string uri)
    {
        var merged = Application.Current.Resources.MergedDictionaries;

        // Remove the currently active theme dict (identified by its source URI)
        var current = merged.FirstOrDefault(
            d => d.Source is not null &&
                 (d.Source.OriginalString.Contains("DarkTheme") ||
                  d.Source.OriginalString.Contains("RetroTheme")));

        if (current is not null)
            merged.Remove(current);

        merged.Add(new ResourceDictionary
        {
            Source = new Uri(uri, UriKind.Absolute)
        });
    }
}
