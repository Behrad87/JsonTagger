using JsonTagger.Lib.Models;

namespace JsonTagger.Lib.Services;

public class NotificationService : INotificationService
{
    // We expose these as events so the View can show toast notifications
    public event Action<string, NotificationType>? NotificationRaised;

    public void ShowSuccess(string message) => Raise(message, NotificationType.Success);
    public void ShowError(string message) => Raise(message, NotificationType.Error);
    public void ShowWarning(string message) => Raise(message, NotificationType.Warning);
    public void ShowInfo(string message) => Raise(message, NotificationType.Info);

    private void Raise(string message, NotificationType type)
        => NotificationRaised?.Invoke(message, type);
}
