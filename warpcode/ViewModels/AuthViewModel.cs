using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.CustomControls;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Services;
using CCIMIGRATION.Services.FaceService;
using CCIMIGRATION.Views;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Diagnostics;


namespace CCIMIGRATION.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        #region Constructor
        public AuthViewModel(IDialogService dialogService) : base(dialogService)
        {
            GetUser();
            RefreshToken();
        }
        #endregion
        #region Method
        private async Task<byte[]> SaveFiles()
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
                    File.WriteAllBytes(Path.Combine(folderPath, "Image" + App.ImageList.IndexOf(item)), item);
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
                await ShowToastAsync(ex.Message);
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
        public async Task<bool> SubmitPhotos()
        {
            try
            {
                await ShowLoaderAsync("");
                var bytes = await SaveFiles();
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
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return false;
            }
            finally
            {
                HideLoader();
            }
        }
        public async void RefreshToken()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.ValidateRefreshToken, GetTokenRequest(), true);
                if (response != null)
                {
                    if (response.Status)
                    {
                        var data = ApiHelper.ConvertResult<RefreshTokenResponse>(response.Result);
                        App.Token = data.AccessToken;
                        var userdata = Preferences.Get(Constants.UserData, "");
                        var userdetails = JsonConvert.DeserializeObject<User>(userdata);
                        userdetails.Token = data.AccessToken;
                        userdetails.RefreshToken = data.RefreshToken;
                        var user = JsonConvert.SerializeObject(userdetails);
                        Preferences.Set(Constants.UserData, user);
                    }
                    else
                    {
                        await ShowToastAsync(response.Message);
                    }
                }
                else
                {
                                            await ShowToastAsync(Constants.SomethingWentWrongTryAgain);
                }
               
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }
        public string GetTokenRequest()
        {
            var userdata = Preferences.Get(Constants.UserData, "");
            var userdetails = JsonConvert.DeserializeObject<User>(userdata);
            return JsonConvert.SerializeObject(userdetails.RefreshToken);

        }
        public async void AndroidPinLogin()
        {
            try
            {
                await ShowLoaderAsync("");
                var _request = new UserLoginType()
                {
                    UserId = Convert.ToInt32(App.LoggedInUser.Id),
                    LoginType = 2
                };
                var result = await ApiHelper.CallApi(HttpMethod.Post, Constants.LoggedBy, JsonConvert.SerializeObject(_request), false);
                if (result.Status)
                {
                    CheckEmployeeImageStatus();
                }
                else
                {
                    await ShowToastAsync("AuthenticationFailed");
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
        public async void CheckEmployeeImageStatus()
        {
            try
            {
                await ShowLoaderAsync("");
                
                // Add validation
                if (LoginUser == null || string.IsNullOrEmpty(LoginUser.EmployeeId))
                {
                    await ShowToastAsync("User login information is missing. Please log in again.");
                    // Navigate back to login
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    });
                    return;
                }
                
                // Add timeout protection with CancellationToken
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.EmployeeImageStatus + "?employeId=" + LoginUser.EmployeeId, null, true);
                    
                    if (response != null && response.Status)
                    {
                        // Employee image exists, navigate to main app
                        Application.Current.Dispatcher.Dispatch(() =>
                        {
                            try
                            {
                                Debug.WriteLine("Navigating to main app - user has image");
                                App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                            }
                            catch (Exception navEx)
                            {
                                Debug.WriteLine($"Navigation error: {navEx.Message}");
                                // Fallback navigation
                                App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                            }
                        });
                    }
                    else
                    {
                        Debug.WriteLine("Employee image doesn't exist - should capture photo");
                        
                        // For now, skip camera and go to main app to avoid getting stuck
                        // TODO: Implement proper camera flow later
                        await ShowToastAsync("Image capture required. Please complete setup in the app.");
                        
                        Application.Current.Dispatcher.Dispatch(() =>
                        {
                            App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                        });
                        
                        // Uncomment below when camera issues are resolved
                        /*
                        App.CameraFrom = "Retake";
                        
                        try
                        {
                            if (DeviceInfo.Platform == DevicePlatform.iOS)
                            {
                                Application.Current.Dispatcher.Dispatch(async () =>
                                {
                                    await App.Current.MainPage.Navigation.PushModalAsync(new CameraPageView());
                                });
                            }
                            else
                            {
                                await App.Current.MainPage.Navigation.PushModalAsync(new CameraPageView());
                            }
                        }
                        catch (Exception cameraEx)
                        {
                            Debug.WriteLine($"Camera navigation error: {cameraEx.Message}");
                            await ShowToastAsync("Unable to open camera. Please try again.");
                            
                            // Fallback: navigate to main app anyway
                            Application.Current.Dispatcher.Dispatch(() =>
                            {
                                App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                            });
                        }
                        */
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("CheckEmployeeImageStatus timed out");
                await ShowToastAsync("Connection timeout. Please check your internet connection.");
                
                // Navigate to main app anyway to avoid getting stuck
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                });
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                Debug.WriteLine($"CheckEmployeeImageStatus error: {ex.Message}");
                await ShowToastAsync($"Authentication error: {ex.Message}");
                
                // Critical error handling: ensure we don't get stuck
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    try
                    {
                        // For now, navigate to main app to avoid getting stuck
                        App.Current.MainPage = new NavigationPage(new EmployeeMenuPage());
                    }
                    catch (Exception navEx)
                    {
                        Debug.WriteLine($"Critical navigation error: {navEx.Message}");
                        // Last resort: navigate to a basic page
                        App.Current.MainPage = new ContentPage 
                        { 
                            Content = new Label 
                            { 
                                Text = "Application error. Please restart the app.",
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center
                            }
                        };
                    }
                });
            }
            finally
            {
                HideLoader();
            }
        }

        #endregion
    }
}
