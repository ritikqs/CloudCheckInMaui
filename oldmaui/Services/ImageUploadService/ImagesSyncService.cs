using Newtonsoft.Json;
using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Threading;

namespace CloudCheckInMaui.Services.ImageUploadService
{
    public class ImagesSyncService : IImageSyncService
    {
        private CancellationTokenSource _cancellationTokenSource;
        private string _employeeId;
        private string _facePersistencyId;
        private bool _isRunning;

        public ImagesSyncService()
        {
        }

        public async Task<bool> StartSync(string employeeId, string facePersistencyId)
        {
            try
            {
                _employeeId = employeeId;
                _facePersistencyId = facePersistencyId;
                _cancellationTokenSource = new CancellationTokenSource();
                _isRunning = true;

                // Start background sync
                _ = Task.Run(async () =>
                {
                    while (_isRunning && !_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        await SyncPhotos();
                        await Task.Delay(TimeSpan.FromMinutes(5), _cancellationTokenSource.Token);
                    }
                }, _cancellationTokenSource.Token);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to start sync: {ex.Message}");
                return false;
            }
        }

        public Task<bool> StopSync()
        {
            try
            {
                _isRunning = false;
                _cancellationTokenSource?.Cancel();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to stop sync: {ex.Message}");
                return Task.FromResult(false);
            }
        }

        private async Task SyncPhotos()
        {
            try
            {
                if (Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess != Microsoft.Maui.Networking.NetworkAccess.Internet)
                {
                    return;
                }

                var bytes = SaveFiles();
                if (bytes != null)
                {
                    var request = new RetakePhotoRequest
                    {
                        EmployeeId = _employeeId,
                        ZipByte = Convert.ToBase64String(bytes),
                        FacePersistencyId = _facePersistencyId
                    };

                    var response = await ApiHelper.CallApi(HttpMethod.Post, ConstantHelper.Constants.RetakeEmployeeImage, JsonConvert.SerializeObject(request));
                    if (response.Status)
                    {
                        await StopSync();
                    }
                    else
                    {
                        Debug.WriteLine($"Failed to sync photos: {response.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SyncPhotos: {ex.Message}");
            }
        }

        private byte[] SaveFiles()
        {
            try
            {
                // Create temporary directory for storing images
                var folderPath = Path.Combine(FileSystem.CacheDirectory, "Pictures");
                if (Directory.Exists(folderPath))
                    Directory.Delete(folderPath, true);
                Directory.CreateDirectory(folderPath);

                // Create temporary files from ImageList
                if (App.ImageList != null && App.ImageList.Count > 0)
                {
                    for (int i = 0; i < App.ImageList.Count; i++)
                    {
                        File.WriteAllBytes(Path.Combine(folderPath, i.ToString()), App.ImageList[i]);
                    }
                }

                // Create zip file path
                var zipFilePath = Path.Combine(FileSystem.CacheDirectory, "Files.zip");
                if (File.Exists(zipFilePath))
                    File.Delete(zipFilePath);

                // Create zip file from directory
                ZipFile.CreateFromDirectory(folderPath, zipFilePath);

                // Read zip file as bytes
                using (var zipStream = File.OpenRead(zipFilePath))
                using (var memoryStream = new MemoryStream())
                {
                    zipStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SaveFiles: {ex.Message}");
                return null;
            }
        }
    }
} 