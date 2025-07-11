using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Data;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
// using Matcha.BackgroundService; // REMOVED: Use Microsoft.Maui.Essentials background services
// using Com.OneSignal; // REMOVED: Causes Xamarin.Forms dependencies
// using Com.OneSignal.Abstractions; // REMOVED: Causes Xamarin.Forms dependencies  
// using CCIMIGRATION.Resources; // TODO: Fix Resource generation
using CCIMIGRATION.MultilanguageHelper;
using CCIMIGRATION.Service.ApiService;
// using XF.Material.Forms; // REMOVED: XF.Material not compatible with MAUI
using CCIMIGRATION.Services;
using CCIMIGRATION.ViewModels;
using CCIMIGRATION.Views;
using Newtonsoft.Json;
using System.Diagnostics;
// using Plugin.Geolocator; // REMOVED: Use Microsoft.Maui.Essentials.Geolocation
// Note: Microsoft.Maui.Essentials types are included in Microsoft.Maui.Essentials package
using ServiceHelper = CCIMIGRATION.Services.ServiceHelper;

namespace CCIMIGRATION
{
    public partial class App : Application
    {
        public static List<ImageList> Data = new List<ImageList>();
        public static List<byte[]> ImageList = new List<byte[]>();
        public static byte[] FaceRecogImage; 
        public static List<OfflineTime> OfflineTimeList = new List<OfflineTime>();
        public static string Token = "";
        public static string Role = "";
        public static string SiteName = "";
        public static string CameraFrom;
        static Repository repository;
        public static bool CheckedIn;
        public static int OutLocationCounter;
        public static string CheckedInSite;
        public static User LoggedInUser;
        public static bool Updatetimer;
        public static bool IsOnHomePage;
        public static double GeofencTimeInterval;
        public static bool CanApplyHoliday;
        public static string RetakeStatus;
        public static string PhotoCaptureStatus;
        public static string RetakeError;
        // public static PlayerIds DeviceToken; // REMOVED: OneSignal not available
        public static string DeviceToken; // Replaced PlayerIds with string for MAUI compatibility
        public static string pushtoken;
        public static int iOSCameraCaptureCounter;
        public static string FaceMathError;
        public static string DBKey = "cd7ede46-d841-4647-9af2-6a79ad64e30c";
        public static string XApikey = "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp";
        public static int ImageCaptureCount;
        public static bool InternetAvailable;
        public static bool IsBusy = false;
        public static AuthViewModel _authVm;
        public static bool IsCameraNavigated;
        public static Repository Repository { get { if (repository == null) { repository = new Repository(); } return repository; } }
        private readonly LocalizationService _localizationService;
        private readonly IPushNotificationService _pushNotificationService;
        
        public App(LocalizationService localizationService, IPushNotificationService pushNotificationService)
        {
            try
            {
                Debug.WriteLine("App constructor: Starting initialization");
                
                _localizationService = localizationService;
                _pushNotificationService = pushNotificationService;
                
                Debug.WriteLine("App constructor: Initializing component");
                InitializeComponent();
                
                Debug.WriteLine("App constructor: Setting up basic properties");
                App.Current.UserAppTheme = AppTheme.Light;
                App.CameraFrom = String.Empty;
                pushtoken = "";
                
                Debug.WriteLine("App constructor: Setting MainPage to LoginPage directly");
                // DIRECT: Set MainPage to LoginPage immediately
                MainPage = new NavigationPage(new LoginPage());
                
                Debug.WriteLine("App constructor: MainPage set to LoginPage");
                
                // Initialize services asynchronously after MainPage is set
                Task.Run(async () =>
                {
                    await InitializeServicesAsync();
                });
                
                Debug.WriteLine("App constructor: Initialization complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Critical error in App constructor: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Emergency fallback - ensure we have a MainPage
                try
                {
                    MainPage = new NavigationPage(new LoginPage());
                    Debug.WriteLine("App constructor: Emergency fallback to LoginPage successful");
                }
                catch (Exception fallbackEx)
                {
                    Debug.WriteLine($"Emergency fallback failed: {fallbackEx.Message}");
                }
            }
        }
        
        private async Task InitializeServicesAsync()
        {
            try
            {
                Debug.WriteLine("InitializeServicesAsync: Starting async initialization");
                
                // Initialize connectivity monitoring
                Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
                
                // Load localization asynchronously
                var savedLanguage = _localizationService.GetSavedLanguage();
                await _localizationService.LoadAsync(savedLanguage);
                
                // Initialize the LocalizationResourceManager wrapper
                LocalizationResourceManager.Initialize(_localizationService);
                
                // Initialize push notification service
                try
                {
                    _pushNotificationService.Initialize();
                    _pushNotificationService.RegisterForPushNotifications();
                    _pushNotificationService.TokenRefreshed += OnPushTokenRefreshed;
                    _pushNotificationService.NotificationReceived += OnNotificationReceived;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Push notification initialization error: {ex.Message}");
                }
                
                Debug.WriteLine("InitializeServicesAsync: Async initialization complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InitializeServicesAsync: {ex.Message}");
            }
        }
        
        // COMMENTED OUT: SetupFallbackTimer - not needed since we go directly to LoginPage
        private void SetupFallbackTimer()
        {
            // Timer removed - direct navigation to LoginPage eliminates need for fallback
            Debug.WriteLine("SetupFallbackTimer: Skipped - direct navigation implemented");
        }
        
        private void OnPushTokenRefreshed(object sender, string token)
        {
            pushtoken = token;
            DeviceToken = token;
            System.Diagnostics.Debug.WriteLine($"Push token refreshed: {token}");
            
            // You can send this token to your server here if needed
        }
        
        private void OnNotificationReceived(object sender, IDictionary<string, object> data)
        {
            System.Diagnostics.Debug.WriteLine("Notification received");
            
            // Handle notification data
            if (data.ContainsKey("title") && data.ContainsKey("message"))
            {
                var title = data["title"].ToString();
                var message = data["message"].ToString();
                
                // Show alert or navigate based on notification content
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    if (App.Current.MainPage != null)
                    {
                        await App.Current.MainPage.DisplayAlert(title, message, "OK");
                    }
                });
            }
        }


        private async Task SetAppLanguageAsync()
        {
            var language = Preferences.Get(Constants.AppLanguageCode, "");
            if (!String.IsNullOrEmpty(language))
            {
                await _localizationService.ChangeLanguageAsync(language);
                CheckUser();
            }
            else
            {
                MainPage = new NavigationPage(new LanguageSelectionPage());
            }
        }

        private void GetPushToken()
        {
            // OneSignal.Current.IdsAvailable(id); // REMOVED: OneSignal not available in MAUI project
            // TODO: Implement push notification service for MAUI
        }

        private void id(string playerID, string pushToken)
        {
     
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            InternetAvailable = false;
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                InternetAvailable = true;

                // Updated to use MAUI DeviceInfo.Platform
                if(DeviceInfo.Platform == DevicePlatform.Android)
                {
                    //if (DependencyService.Get<IAppStateService>().IsInBackground())
                    //{
                    //    CheckAndSyncLocations();
                    //}
                }
                else
                {
                    CheckAndSyncLocations();
                }
                
            }
        }

        public static void CheckAndSyncLocations()
        {
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                try
                {
                    if(DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        MessagingService.Send(Constants.StopService, Constants.StopService);
                    }
                    else if(DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        DependencyService.Get<IMonitoringService>().StopMonitoring();
                    }
                    var data = await App.Repository.GetAsync<LocationModel>();
                    if (data != null && data.Count > 0)
                    {
                        OfflineTimeList.Clear();
                        foreach (var item in data)
                        {
                            var entry = new OfflineTime()
                            {
                                SiteMapRefLat = item.Latitude,
                                SiteMapRefLong = item.Longitude,
                                TimeInSite = item.TimeStamp
                            };
                            OfflineTimeList.Add(entry);
                        }
                        var user = Preferences.Get(Constants.UserData, "");
                        LoggedInUser = JsonConvert.DeserializeObject<User>(user);
                        var dialogService = ServiceHelper.GetService<IDialogService>() ?? new DialogService();
                        await dialogService.ShowLoadingAsync("Syncing data");
                        var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.CurrentSite, ""));
                        if (currentSiteData != null)
                        {
                            var _request = new OfflineTimeSheetSyncModel()
                            {
                                EmpId = LoggedInUser.EmployeeId,
                                OfflineTime = OfflineTimeList.ToArray(),
                                SiteId = currentSiteData.SiteId.ToString()
                            };
                            var response = await ApiHelper.CallApi(HttpMethod.Post, ConstantHelper.Constants.SyncOfflineTimeSheet, JsonConvert.SerializeObject(_request), true);
                            if (response.Status)
                            {
                                await Repository.DeleteAllAsync<LocationModel>();
                                var result = ApiHelper.ConvertResult<OfflineSyncResponse>(response.Result);
                                if (result.IsCheckedIn)
                                {
                                    if (App.IsOnHomePage)
                                    {
                                        MessagingService.Send("UpdateHomePage", "UpdateHomePage");
                                    }
                                    else
                                    {
                                        if (DeviceInfo.Platform == DevicePlatform.Android)
                                        {
                                            MessagingService.Send(Constants.StartService, Constants.StartService);
                                        }
                                        else
                                        {
                                            DependencyService.Get<IMonitoringService>().StopMonitoring();
                                            //MessagingCenter.Send(Constants.StartService, Constants.StartService);
                                        }
                                    }
                                }
                                else
                                {
                                    if (DeviceInfo.Platform == DevicePlatform.Android)
                                    {
                                        // TODO: Replace CrossGeolocator with MAUI Essentials Geolocation
                                        // if (CrossGeolocator.Current.IsListening)
                                        // {
                                        //     await CrossGeolocator.Current.StopListeningAsync();
                                            MessagingService.Send(Constants.StopService, Constants.StopService);
                                        // }
                                    }
                                    else
                                    {
                                        DependencyService.Get<IMonitoringService>().StopMonitoring();
                                    }
                                    if (App.IsOnHomePage)
                                    {
                                        MessagingService.Send("UpdateHomePage", "UpdateHomePage");
                                    }
                                }
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", "Offline sync error occurred", "Ok"); // TODO: Fix Resource.OfflineSyncError
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "", "Ok");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    var dialogService = ServiceHelper.GetService<IDialogService>() ?? new DialogService();
                    dialogService.HideLoading();
                }
            });
            
        }
        // COMMENTED OUT: CheckUser - not needed since we set MainPage directly in constructor
        private void CheckUser()
        {
            // Method not needed - MainPage is set directly in constructor
            Debug.WriteLine("CheckUser: Skipped - MainPage already set in constructor");
        }
        protected override void OnStart()
        {
            Application.Current.Dispatcher.Dispatch(async() =>
            {
                try
                {
                    var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.GetMonitoringTime, null, false);
                    if (response != null)
                    {
                        if (response.Status)
                        {
                            var result = ApiHelper.ConvertResult<LocationTrackTimeResponse>(response.Result);
                            ImageCaptureCount = result.ImageCaptureCount;
                            Preferences.Set(Constants.MonitoringTime, result.LocationTrackTime);
                            if (result.LocationTrackTime != null)
                            {
                                GeofencTimeInterval = (60000 * Convert.ToInt32(result.LocationTrackTime));
                            }
                            else
                            {
                               GeofencTimeInterval = Constants.DefaultGeofenceInterval;
                            }
                            
                        }
                        else
                        {
                            ImageCaptureCount = Constants.DefaultImageCount;
                            GeofencTimeInterval = Constants.DefaultGeofenceInterval;
                        }
                    }
                    else
                    {
                        ImageCaptureCount = Constants.DefaultImageCount;
                        GeofencTimeInterval = Constants.DefaultGeofenceInterval;
                    }
                }
                catch (Exception ex)
                {
                    ImageCaptureCount = Constants.DefaultImageCount;
                    GeofencTimeInterval = Constants.DefaultGeofenceInterval;
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    
                }
            });
        }


        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
