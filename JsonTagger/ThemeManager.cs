using System.Windows;

using JsonTagger.Properties;

namespace JsonTagger;

public static class ThemeManager
{
    public static IReadOnlyList<AppTheme> AvailableThemes { get; } =
    [
        new AppTheme("Dark",  "🌙  Dark",  "pack://application:,,,/JsonTagger;component/Themes/DarkTheme.xaml"),
        new AppTheme("Light", "☀️  Light", "pack://application:,,,/JsonTagger;component/Themes/LightTheme.xaml"),
        new AppTheme("Retro", "📷  Retro", "pack://application:,,,/JsonTagger;component/Themes/RetroTheme.xaml"),
    ];

    public static AppTheme CurrentTheme { get; private set; } = AvailableThemes[0];

    public static event EventHandler<AppTheme>? ThemeChanged;

    /// <summary>
    /// Call once at startup — reads the persisted setting and applies it.
    /// </summary>
    public static void LoadSaved()
    {
        var savedId = Settings.Default.ActiveThemeId;

        var theme = AvailableThemes.FirstOrDefault(t => t.Id == savedId)
                    ?? AvailableThemes[0]; // fall back to Dark if setting is missing/corrupt

        // Apply without saving again (nothing changed yet)
        SwapDictionary(theme.PackUri);
        CurrentTheme = theme;
    }

    public static void Apply(AppTheme theme)
    {
        if (theme == CurrentTheme) return;

        SwapDictionary(theme.PackUri);
        CurrentTheme = theme;

        // Persist immediately so even a crash won't lose the preference
        Settings.Default.ActiveThemeId = theme.Id;
        Settings.Default.Save();

        ThemeChanged?.Invoke(null, theme);
    }

    public static void Apply(string themeId)
    {
        var theme = AvailableThemes.FirstOrDefault(t => t.Id == themeId);
        if (theme is not null) Apply(theme);
    }

    private static void SwapDictionary(string packUri)
    {
        var merged = Application.Current.Resources.MergedDictionaries;

        var existing = merged.FirstOrDefault(d =>
            d.Source?.OriginalString.Contains("Theme.xaml") == true);

        if (existing is not null)
            merged.Remove(existing);

        merged.Add(new ResourceDictionary
        {
            Source = new Uri(packUri, UriKind.Absolute)
        });
    }
}

public sealed class AppTheme(string id, string displayName, string packUri)
{
    public string Id { get; } = id;
    public string DisplayName { get; } = displayName;
    public string PackUri { get; } = packUri;
}