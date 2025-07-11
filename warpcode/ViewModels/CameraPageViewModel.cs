using CCIMIGRATION.ConstantHelper;
// using Matcha.BackgroundService; // REMOVED: Replace with MAUI background services
using CCIMIGRATION.Helpers;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Services;
using CCIMIGRATION.Services.FaceService;
using CCIMIGRATION.Services.ImageUploadService;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;
using System.IO.Compression;

namespace CCIMIGRATION.ViewModels
{
    public class CameraPageViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private bool _isSuccessVisible;
        private bool _isSuccessForLoginVisible;
        private string _successForLoginText;
        public string SuccessForLoginText
        {
            get { return _successForLoginText; }
            set { _successForLoginText = value;OnPropertyChanged(); }
        }
        public bool IsSuccessForLoginVisible
        {
            get { return _isSuccessForLoginVisible; }
            set { _isSuccessForLoginVisible = value;OnPropertyChanged(); }
        }
        public bool IsSuccessVisible
        {
            get { return _isSuccessVisible; }
            set
            {
                _isSuccessVisible = value; OnPropertyChanged("IsSuccessVisible");
            }
        }
        #endregion
        #region Contructor
        public CameraPageViewModel()
        {
            GetUser();
        }
        #endregion
        #region Methods
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

                //check or create zip file 
                if (File.Exists(Path.Combine(fileService.GetFolderPath(), "Files.zip")))
                    File.Delete(Path.Combine(fileService.GetFolderPath(), "Files.zip"));

                
                //create zip file of folder
                ZipFile.CreateFromDirectory(folderPath, Path.Combine(fileService.GetFolderPath(), "Files.zip"));

                //converting zip file to byte array
                var zipstream = File.OpenRead(Path.Combine(fileService.GetFolderPath(), "Files.zip"));
                
                var ziparray = GetZipBytes(zipstream);


                return ziparray;

            }
            catch (Exception ex)
            {
                //ShowToast(Resource.SomethingwentWrong);
                ShowToast(ex.Message);
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

        public async void SubmitPhotosForLogin()
        {
            try
            {

                ShowLoader("");
                var bytes = SaveFiles();
                if (bytes != null)
                {
                    var _facialRecognitionUserGUID = await FaceRecognitionService.AddNewFace(App.LoggedInUser.Email, new MemoryStream(App.ImageList[0])).ConfigureAwait(false);
                    if (_facialRecognitionUserGUID != null)
                    {
                        var _req = new RetakePhotoRequest()
                        {
                            EmployeeId = App.LoggedInUser.EmployeeId,
                            ZipByte = Convert.ToBase64String(bytes),
                            FacePersistencyId = _facialRecognitionUserGUID.ToString()
                        };
                        var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.RetakeEmployeeImage, JsonConvert.SerializeObject(_req));
                        if (response.Status)
                        {
                            HideLoader();
                            Application.Current.Dispatcher.Dispatch(() =>
                            {
                                SuccessForLoginText = "Photos submitted"; // TODO: Fix Resource.PhotosSubmitted
                                IsSuccessForLoginVisible = true;
                                //App.Current.MainPage = new TransitionNavigationPage(new HomeTabbedPage());
                            });

                        }
                        else
                        {
                            ShowToast(response.Message);
                            NavigateBack();
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.ErrorUploadingAzure);
                        NavigateBack();
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.ErrorCompressing);
                    NavigateBack();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                ShowToast(ex.Message);
                NavigateBack();
            }
            finally
            {
                HideLoader();
            }
        }
        public async void SubmitPhotos()
        {
            try
            {
                ShowLoader("");
                var bytes = SaveFiles();
                if (bytes != null)
                {
                    var _facialRecognitionUserGUID = await FaceRecognitionService.AddNewFace(App.LoggedInUser.Email, new MemoryStream(App.ImageList[0])).ConfigureAwait(false);
                    if (_facialRecognitionUserGUID != null)
                    {
                        var _req = new RetakePhotoRequest()
                        {
                            EmployeeId = LoginUser.EmployeeId,
                            ZipByte = Convert.ToBase64String(bytes),
                            FacePersistencyId = _facialRecognitionUserGUID.ToString()
                        };
                        var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.RetakeEmployeeImage, JsonConvert.SerializeObject(_req));
                        if (response.Status)
                        {
                            HideLoader();
                            UIThreadHelper.InvokeOnMainThread(() =>
                            {
                                HideLoader();
                                SuccessForLoginText = ResourceConstants.PhotoRecapturedContactAdmin;
                                IsSuccessForLoginVisible = true;
                            });
                            
                        }
                        else
                        {
                            ShowToast(response.Message);
                            NavigateBack();
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.ErrorUploadingAzure);
                        NavigateBack();
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.ErrorCompressing);
                    NavigateBack();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                ShowToast(ex.Message);
                NavigateBack();
            }
            finally
            {
                HideLoader();
            }
        }
        private void NavigateBack()
        {
            try
            {
                UIThreadHelper.InvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.Navigation.PopModalAsync();
                });
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }
            
        }
        public async void RegisterUser(RegistrationRequestModel data)
        {
            try
            { 
                ShowLoader(ResourceConstants.Registering);

                    //await FaceRecognitionService.DeleteGroup();
                    var _facialRecognitionUserGUID = await FaceRecognitionService.AddNewFace(data.Email, new MemoryStream(App.ImageList[0])).ConfigureAwait(false);
                    if (!String.IsNullOrEmpty(_facialRecognitionUserGUID))
                    {
                        data.FacePersistencyId = _facialRecognitionUserGUID.ToString();

                        var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.Register, JsonConvert.SerializeObject(data));
                        if (response.Status)
                        {

                            HideLoader();
                            var responsedata = ApiHelper.ConvertResult<RegistrationResponseModel>(response.Result);
                            BackgroundAggregatorService.Instance.Clear();
                            BackgroundAggregatorService.Add(() => new ImagesSyncService(responsedata.EmpId.ToString(), _facialRecognitionUserGUID.ToString()));
                            Preferences.Set(Constants.OnceRegistered, "true");
                            IsSuccessVisible = true;
                            BackgroundAggregatorService.StartBackgroundService();
                            
                        }
                        else
                        {
                            ShowToast(response.Message);
                            NavigateBack();
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.SomethingWentWrong);
                        NavigateBack();
                    }
                

            }
            catch (APIErrorException e)
            {
                ShowToast(e.Body.Error.Message);
                NavigateBack();
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                ShowToast("Exception:- " + ex.Message);
                NavigateBack();
            }
            finally
            {
                HideLoader();
            }
        }

        private void CreateChunk()
        {
            var pagearray = new List<int> { };
            try
            {
                if (App.ImageList.Count % 20 == 0)
                {
                    var pagecount = App.ImageList.Count / 20;
                    for(var i = 0; i < pagecount; i++)
                    {
                        pagearray.Add(20);
                    }
                }
                else
                {
                    var pagecount = App.ImageList.Count / 20;
                    for (var i = 0; i < pagecount; i++)
                    {
                        pagearray.Add(20);
                    }
                    pagearray.Add(App.ImageList.Count % 20);
                }
                foreach(var item in pagearray)
                {
                   var data = CreateChunkArray(pagearray.IndexOf(item),Convert.ToInt32(item));
                }
                
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {

            }
        }
        private string CreateChunkArray(int start,int size)
        {
            try
            {
                var a = App.ImageList[start];
                for(var i = start+1; i < size; i++)
                {
                    a.Concat(App.ImageList[i]);
                }
                return Convert.ToBase64String(a);
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
                return null;
            }
            finally
            {

            }
        }
        #endregion
    }
}
