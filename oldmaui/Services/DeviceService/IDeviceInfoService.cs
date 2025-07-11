using System.Threading.Tasks;

namespace CloudCheckInMaui.Services.DeviceService
{
    public interface IDeviceInfoService
    {
        Task<string> GetDeviceToken();
        string GetDeviceId();
        string GetDeviceName();
        string GetDeviceModel();
        string GetDeviceManufacturer();
        string GetDevicePlatform();
        string GetDeviceVersion();
    }
} 