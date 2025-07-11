using System;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using CloudCheckInMaui.Models;
using System.Security.Cryptography;
using System.Text;

namespace CloudCheckInMaui.Services.DeviceService
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private const string DEVICE_TOKEN_KEY = "device_token";
        private const string INSTALL_ID_KEY = "install_id";
        private string _cachedDeviceToken;

        public async Task<string> GetDeviceToken()
        {
            try
            {
                // Try to get cached token first
            if (!string.IsNullOrEmpty(_cachedDeviceToken))
                return _cachedDeviceToken;

                // Try to get from preferences
                var savedToken = await SecureStorage.Default.GetAsync(DEVICE_TOKEN_KEY);
                if (!string.IsNullOrEmpty(savedToken))
                {
                    _cachedDeviceToken = savedToken;
                    return savedToken;
                }

                // Get or create installation ID
                var installId = await GetOrCreateInstallId();

                // Create device info
                var deviceInfo = new DeviceInfoModel
                {
                    DeviceId = GetDeviceId(),
                    DeviceName = GetDeviceName(),
                Model = GetDeviceModel(),
                Manufacturer = GetDeviceManufacturer(),
                Platform = GetDevicePlatform(),
                Version = GetDeviceVersion(),
                    Timestamp = DateTime.UtcNow.Ticks,
                    AppInstallId = installId
            };

                // Generate token using SHA256
                var deviceInfoJson = System.Text.Json.JsonSerializer.Serialize(deviceInfo);
                var tokenBytes = SHA256.HashData(Encoding.UTF8.GetBytes(deviceInfoJson));
                var token = Convert.ToBase64String(tokenBytes);

                // Save token
                await SecureStorage.Default.SetAsync(DEVICE_TOKEN_KEY, token);
                _cachedDeviceToken = token;

                return token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error generating device token: {ex.Message}");
                // Fallback to a simple unique identifier if something goes wrong
                return Guid.NewGuid().ToString("N");
            }
        }

        private async Task<string> GetOrCreateInstallId()
        {
            try
            {
                var installId = await SecureStorage.Default.GetAsync(INSTALL_ID_KEY);
                if (string.IsNullOrEmpty(installId))
                {
                    installId = Guid.NewGuid().ToString("N");
                    await SecureStorage.Default.SetAsync(INSTALL_ID_KEY, installId);
                }
                return installId;
            }
            catch
            {
                return Guid.NewGuid().ToString("N");
            }
        }

        public string GetDeviceId()
        {
            try
            {
                return DeviceInfo.Current.Name + "_" + DeviceInfo.Current.Model;
            }
            catch
            {
                return "unknown_device";
            }
        }

        public string GetDeviceName()
        {
            try
        {
            return DeviceInfo.Current.Name;
            }
            catch
            {
                return "Unknown";
            }
        }

        public string GetDeviceModel()
        {
            try
        {
            return DeviceInfo.Current.Model;
            }
            catch
            {
                return "Unknown";
            }
        }

        public string GetDeviceManufacturer()
        {
            try
        {
            return DeviceInfo.Current.Manufacturer;
            }
            catch
            {
                return "Unknown";
            }
        }

        public string GetDevicePlatform()
        {
            try
        {
            return DeviceInfo.Current.Platform.ToString();
            }
            catch
            {
                return "Unknown";
            }
        }

        public string GetDeviceVersion()
        {
            try
        {
            return DeviceInfo.Current.VersionString;
            }
            catch
            {
                return "Unknown";
            }
        }
    }
} 