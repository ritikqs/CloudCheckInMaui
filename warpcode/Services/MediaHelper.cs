using Microsoft.Maui.Media;
using Microsoft.Maui.Graphics;

namespace CCIMIGRATION.Services
{
    public static class MediaHelper
    {
        public static async Task<FileResult> TakePhotoAsync(int maxWidthHeight = 2000, int compressionQuality = 20)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                    {
                        Title = "Take Photo"
                    });

                    if (photo != null)
                    {
                        // Process the photo to resize and compress
                        var stream = await photo.OpenReadAsync();
                        var processedPath = await ProcessImage(stream, photo.FileName, maxWidthHeight, compressionQuality);
                        
                        // Return a new FileResult with the processed image
                        return new FileResult(processedPath);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                // Handle exception
                return null;
            }
        }

        private static async Task<string> ProcessImage(Stream inputStream, string fileName, int maxWidthHeight, int compressionQuality)
        {
            // Get the cache directory
            var cacheDir = FileSystem.Current.CacheDirectory;
            var outputPath = Path.Combine(cacheDir, $"processed_{fileName}");

            // For now, just copy the original image
            // TODO: Implement proper image resizing using SkiaSharp or similar library
            try
            {
                using (var outputStream = File.Create(outputPath))
                {
                    inputStream.Position = 0; // Reset stream position
                    await inputStream.CopyToAsync(outputStream);
                }
            }
            catch (Exception)
            {
                // If there's an error, return the original filename
                return fileName;
            }

            return outputPath;
        }

        public static bool IsCameraAvailable => MediaPicker.Default.IsCaptureSupported;
    }
}
