using Foundation;
using LocalAuthentication;
using CloudCheckInMaui.Services;
using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.Platforms.iOS
{
    public class LocalAuthenticateService : ILocalAuth
    {
        public void AuthenticatPin()
        {
            var context = new LAContext();
            NSError error;
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out error))
            {
                if (error == null)
                {
                    context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, "Unlock CloudCheckIn", HandleLAContextReplyHandler);
                }
            }
        }

        private void HandleLAContextReplyHandler(bool success, NSError error)
        {
            if (success)
                MessagingCenter.Send<string>("Success", "iOSPinResult");
            else
                MessagingCenter.Send<string>("Fail", "iOSPinResult");
        }
    }
} 