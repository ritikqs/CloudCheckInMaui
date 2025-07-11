// using Matcha.BackgroundService; // REMOVED: Replace with MAUI background services
using Newtonsoft.Json;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.MultilanguageHelper;
// using CCIMIGRATION.Resources; // TODO: Fix Resource generation
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Services.FaceService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Networking;

namespace CCIMIGRATION.Services.ImageUploadService
{
    public class ImagesSyncService // TODO: Implement MAUI background service pattern
    {
        // public TimeSpan Interval { get; set; } // TODO: Implement with MAUI background services
        public string EmployeeId { get; set; }
        public string FacePersistancyId { get; set; }
        private readonly IDialogService _dialogService;
        
        public ImagesSyncService(string empId,string facePersistancyId)
        {
            EmployeeId = empId;
            FacePersistancyId = facePersistancyId;
            _dialogService = ServiceHelper.GetService<IDialogService>();
        }
        public async Task<bool> StartJob()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await SyncPhotos();
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }



        private async Task SyncPhotos()
        {
            try
            {
                var bytes = SaveFiles();
                if (bytes != null)
                {
                    var _req = new RetakePhotoRequest()
                    {
                        EmployeeId = EmployeeId,
                        ZipByte = Convert.ToBase64String(bytes),
                        FacePersistencyId = FacePersistancyId
                    };
                    //var data = JsonConvert.SerializeObject(_req);
                    //Debug.WriteLine(data);
                    //Console.WriteLine(data);
                    //WebApiRestClient _webclient = new WebApiRestClient();
                    //var result = await _webclient.PostAsync<RetakePhotoRequest,RetakeResponse>(Constants.RetakeEmployeeImage, _req);
                    //if (result != null)
                    //{
                    //    if (result.status)
                    //    {

                    //    }
                    //}
                    //System.IO.File.WriteAllText(@"/Users/Mac/Documents/Data/Coding/C#/TestFolder/WriteText.txt", data);
                    var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.RetakeEmployeeImage, JsonConvert.SerializeObject(_req));
                    if (response.Status)
                    {
                        // BackgroundAggregatorService.StopBackgroundService(); // TODO: Implement with MAUI background services
                    }
                    else
                    {
                        await _dialogService.ShowToastAsync(response.Message);
                    }
                }
                else
                {
                    await _dialogService.ShowToastAsync("Error compressing"); // TODO: Fix Resource.ErrorCompressing
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
          
        }
        private byte[] SaveFiles()
        {
            try
            {
                IFileService fileService = DependencyService.Get<IFileService>();
                //Check or Create Folder
                var folderPath = Path.Combine(fileService.GetFolderPath(), "Pictures");
                if (Directory.Exists(folderPath))
                    Directory.Delete(folderPath, true);
                Directory.CreateDirectory(folderPath);

                //write files into device
                foreach (var item in App.ImageList)
                {
                    File.WriteAllBytes(Path.Combine(folderPath, App.ImageList.IndexOf(item).ToString()), item);
                }
                var filenamearray = Directory.GetFiles(folderPath);
                //filenamearray.OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f))).ToArray();
                //check or create zip file 
                if (File.Exists(Path.Combine(fileService.GetFolderPath(), "Files.zip")))
                    File.Delete(Path.Combine(fileService.GetFolderPath(), "Files.zip"));

                //create zip file of folder
                ZipFile.CreateFromDirectory(folderPath, Path.Combine(fileService.GetFolderPath(), "Files.zip"));

                //converting zip file to byte array
                var zipstream = File.OpenRead(Path.Combine(fileService.GetFolderPath(), "Files.zip"));
                var ziparray = GetZipBytes(zipstream);
                File.WriteAllText(Path.Combine(fileService.GetFolderPath(), "ZipByteFile.txt"),Convert.ToBase64String(ziparray));
                return ziparray;

            }
            catch (Exception ex)
            {
                //ShowToast(Resource.SomethingwentWrong);
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        private byte[] GetZipBytes(FileStream file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
