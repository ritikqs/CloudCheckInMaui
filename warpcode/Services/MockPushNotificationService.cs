using CCIMIGRATION.Interface;
using Plugin.LocalNotification;
using System.Diagnostics;

namespace CCIMIGRATION.Services
{
    public class MockPushNotificationService : IPushNotificationService
    {
        public event EventHandler<string> TokenRefreshed;
        public event EventHandler<IDictionary<string, object>> NotificationReceived;

        private string _mockToken = "mock_device_token_" + Guid.NewGuid().ToString();

        public void Initialize()
        {
            Debug.WriteLine("MockPushNotificationService: Initialize called");
            
            // Initialize local notifications as a fallback
            try
            {
                // Request notification permissions
                Task.Run(async () =>
                {
                    var granted = await LocalNotificationCenter.Current.AreNotificationsEnabled();
                    if (!granted)
                    {
                        await LocalNotificationCenter.Current.RequestNotificationPermission();
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Local notification initialization error: {ex.Message}");
            }
        }

        public void RegisterForPushNotifications()
        {
            Debug.WriteLine("MockPushNotificationService: RegisterForPushNotifications called");
            
            // Simulate token generation
            Task.Run(async () =>
            {
                await Task.Delay(1000); // Simulate network delay
                TokenRefreshed?.Invoke(this, _mockToken);
            });
        }

        public void UnregisterForPushNotifications()
        {
            Debug.WriteLine("MockPushNotificationService: UnregisterForPushNotifications called");
        }

        public string GetDeviceToken()
        {
            return _mockToken;
        }

        // Method to simulate receiving a notification (for testing)
        public void SimulateNotification(string title, string message, IDictionary<string, object> data = null)
        {
            var notificationData = new Dictionary<string, object>
            {
                ["title"] = title,
                ["message"] = message
            };

            if (data != null)
            {
                foreach (var item in data)
                {
                    notificationData[item.Key] = item.Value;
                }
            }

            // Trigger notification received event
            NotificationReceived?.Invoke(this, notificationData);

            // Also show a local notification
            ShowLocalNotification(title, message);
        }

        private void ShowLocalNotification(string title, string message)
        {
            try
            {
                var notification = new NotificationRequest
                {
                    NotificationId = new Random().Next(1, int.MaxValue),
                    Title = title,
                    Description = message,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(1)
                    }
                };

                LocalNotificationCenter.Current.Show(notification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing local notification: {ex.Message}");
            }
        }
    }
}
