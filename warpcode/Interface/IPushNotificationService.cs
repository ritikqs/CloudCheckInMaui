namespace CCIMIGRATION.Interface
{
    public interface IPushNotificationService
    {
        void Initialize();
        void RegisterForPushNotifications();
        void UnregisterForPushNotifications();
        string GetDeviceToken();
        event EventHandler<string> TokenRefreshed;
        event EventHandler<IDictionary<string, object>> NotificationReceived;
    }
}
