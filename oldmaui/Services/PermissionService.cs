using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using System.Diagnostics; 

namespace CloudCheckInMaui.Services
{
    public interface IPermissionService
    {
        Task<bool> RequestLocationPermission();
        Task<bool> RequestBackgroundLocationPermission();
        Task<bool> RequestCameraPermission();
        Task<bool> RequestMediaPermission();
    }

    public class PermissionService : IPermissionService
    {
        public async Task<bool> RequestLocationPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error requesting location permission: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RequestBackgroundLocationPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationAlways>();
                }
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error requesting background location permission: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RequestCameraPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error requesting camera permission: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RequestMediaPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Media>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Media>();
                }
                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error requesting media permission: {ex.Message}");
                return false;
            }
        }
    }
} 