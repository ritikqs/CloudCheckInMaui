using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;
using Plugin.LocalNotification;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;

namespace CCIMIGRATION;

[Activity(
    Name = "com.CCIMIGRATION.app.MainActivity",
    Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    LaunchMode = LaunchMode.SingleTop,
    Exported = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // Initialize Fingerprint plugin
        Plugin.Fingerprint.CrossFingerprint.SetCurrentActivityResolver(() => this);
        
        // Initialize local notifications
        LocalNotificationCenter.CreateNotificationChannel(new Plugin.LocalNotification.AndroidOption.NotificationChannelRequest
        {
            Id = "general",
            Name = "General",
            Description = "General notifications"
        });
        
        // Request necessary permissions
        RequestPermissions();
        
        // Set window flags for better performance
        Window?.SetFlags(WindowManagerFlags.HardwareAccelerated, WindowManagerFlags.HardwareAccelerated);
    }
    
    private void RequestPermissions()
    {
        var permissions = new string[]
        {
            Manifest.Permission.Camera,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.UseFingerprint,
            Manifest.Permission.UseBiometric,
            Manifest.Permission.PostNotifications
        };
        
        var permissionsToRequest = new List<string>();
        
        foreach (var permission in permissions)
        {
            if (ContextCompat.CheckSelfPermission(this, permission) != Permission.Granted)
            {
                permissionsToRequest.Add(permission);
            }
        }
        
        if (permissionsToRequest.Count > 0)
        {
            ActivityCompat.RequestPermissions(this, permissionsToRequest.ToArray(), 100);
        }
    }
    
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        
        // Handle permission results here if needed
        for (int i = 0; i < permissions.Length; i++)
        {
            if (grantResults[i] == Permission.Granted)
            {
                // Permission granted
                System.Diagnostics.Debug.WriteLine($"Permission granted: {permissions[i]}");
            }
            else
            {
                // Permission denied
                System.Diagnostics.Debug.WriteLine($"Permission denied: {permissions[i]}");
            }
        }
    }
}
