using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.Services
{
    public class DefaultLocalAuthService : ILocalAuth
    {
        public void AuthenticatPin()
        {
            // For non-iOS platforms, we'll just send a message to check PIN
            MessagingCenter.Send<string>("CheckPin", "CheckPin");
        }
    }
} 