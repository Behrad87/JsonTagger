namespace JsonTagger.Lib.Services;

public class NotificationHostProxy : INotificationHost
{
    private INotificationHost? _target;

    public void SetTarget(INotificationHost target) => _target = target;

    public void ShowToast(string message, string type)
    {
        _target?.ShowToast(message, type);
    }
}
