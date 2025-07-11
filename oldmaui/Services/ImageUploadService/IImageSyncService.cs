using System.Threading.Tasks;

namespace CloudCheckInMaui.Services.ImageUploadService
{
    public interface IImageSyncService
    {
        Task<bool> StartSync(string employeeId, string facePersistencyId);
        Task<bool> StopSync();
    }
} 