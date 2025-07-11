using CCIMIGRATION.ApiModels;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Services;
using CCIMIGRATION.Services.FaceService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CCIMIGRATION.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        #region Private/Public Variables
        public static List<OfflineTime> OfflineTimeList = new List<OfflineTime>();
        private readonly JsonSerializerSettings _jsonSettings;
        private bool _isCheckedIn;
        private DateTime _checkInTime;
        private DateTime _checkOutTime;
        private string _loggedTime;
        public System.Timers.Timer _timer;
        public System.Timers.Timer _geofencetimer;
        private List<SiteModel> _sites;
        private SiteModel _selectedSite;
        private int _selectedSiteIndex;
        private ObservableCollection<TimeSheetRequest> _timesheetList;
        private bool _isSafetyInstructionViewVisible;
        private bool _isSafetySuccessViewVisible;
        private bool _isSafetyFailViewVisible;
        private bool _isHeadProtectionChecked;
        private bool _isHighVJacketChecked;
        private bool _isEarProtectionChecked;
        private bool _isConfirmationYesChecked;
        private bool _isConfirmationNoChecked;
        private string _currentSite;
        private bool _isListRefreshing;

        private string _locationStatus;
        private bool _isCheckedInViewVisible;
        private bool _isNotCheckedInViewVisible;
        private string _clockInTime;
        private string _clockOutTime;
        private bool _isUpdatingTimeSheet;
        private string _workingHours;
        private bool _isNonAuthTime;
        private bool _isTurnOnButtonVisible;
        private bool _isFaceCaptureViewVisible;
        private bool _isCaptureButtonVisible;
        private bool _isSubmitButtonVisible;

        public bool IsFaceCaptureViewVisible
        {
            get { return _isFaceCaptureViewVisible; }
            set { _isFaceCaptureViewVisible = value;OnPropertyChanged(); }
        }
        public bool IsCaptureButtonVisible
        {
            get { return _isCaptureButtonVisible; }
            set { _isCaptureButtonVisible = value; OnPropertyChanged(); }
        }
        public bool IsSubmitButtonVisible
        {
            get { return _isSubmitButtonVisible; }
            set { _isSubmitButtonVisible = value; OnPropertyChanged(); }
        }
        private bool _isbusy;

        public bool IsBusy
        {
            get { return _isbusy; }
            set { _isbusy = value; OnPropertyChanged(); }
        }

        public bool IsTurnOnButtonVisible
        {
            get { return _isTurnOnButtonVisible; }
            set { _isTurnOnButtonVisible = value;OnPropertyChanged(); }
        }
        public bool IsNonAuthTime
        {
            get { return _isNonAuthTime; }
            set { _isNonAuthTime = value;OnPropertyChanged(); }
        }
        public string WorkingHours
        {
            get { return _workingHours; }
            set { _workingHours = value;OnPropertyChanged(); }
        }
        public bool IsUpdatingTimeSheet
        {
            get { return _isUpdatingTimeSheet; }
            set { _isUpdatingTimeSheet = value;OnPropertyChanged(); }
        }

        public string ClockOutTime
        {
            get { return _clockOutTime; }
            set { _clockOutTime = value;OnPropertyChanged(); }
        }

        public string ClockInTime
        {
            get { return _clockInTime; }
            set { _clockInTime = value;OnPropertyChanged(); }
        }

        public bool IsNotCheckedInViewVisible
        {
            get { return _isNotCheckedInViewVisible; }
            set { _isNotCheckedInViewVisible = value;OnPropertyChanged(); }
        }

        public bool IsCheckedInViewVisible
        {
            get { return _isCheckedInViewVisible; }
            set { _isCheckedInViewVisible = value;OnPropertyChanged(); }
        }

        public string LocationStatus
        {
            get { return _locationStatus; }
            set { _locationStatus = value;OnPropertyChanged(); }
        }



        public string CheckInImageString;
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value;OnPropertyChanged("IsListRefreshing"); }
        }

        public string CurrentSite
        {
            get { return _currentSite; }
            set { _currentSite = value; OnPropertyChanged("CurrentSite"); }
        }
        public bool IsHeadProtectionChecked
        {
            get { return _isHeadProtectionChecked; }
            set { _isHeadProtectionChecked = value; OnPropertyChanged("IsHeadProtectionChecked"); }
        }
        public bool IsHighVJacketChecked
        {
            get { return _isHighVJacketChecked; }
            set { _isHighVJacketChecked = value; OnPropertyChanged("IsHighVJacketChecked"); }
        }
        public bool IsEarProtectionChecked
        {
            get { return _isEarProtectionChecked; }
            set { _isEarProtectionChecked = value; OnPropertyChanged("IsEarProtectionChecked"); }
        }
        public bool IsConfirmationYesChecked
        {
            get { return _isConfirmationYesChecked; }
            set
            {
                _isConfirmationYesChecked = value;
                OnPropertyChanged("IsConfirmationYesChecked");

            }
        }
        public bool IsConfirmationNoChecked
        {
            get { return _isConfirmationNoChecked; }
            set
            {
                _isConfirmationNoChecked = value;
                OnPropertyChanged("IsConfirmationNoChecked");
            }
        }
        public bool IsSafetyInstructionViewVisible
        {
            get { return _isSafetyInstructionViewVisible; }
            set { _isSafetyInstructionViewVisible = value; OnPropertyChanged("IsSafetyInstructionViewVisible"); }
        }
        public bool IsSafetySuccessViewVisible
        {
            get { return _isSafetySuccessViewVisible; }
            set { _isSafetySuccessViewVisible = value; OnPropertyChanged("IsSafetySuccessViewVisible"); }
        }
        public bool IsSafetyFailViewVisible
        {
            get { return _isSafetyFailViewVisible; }
            set { _isSafetyFailViewVisible = value; OnPropertyChanged("IsSafetyFailViewVisible"); }
        }
        private bool _isInGeofence;

        public bool IsInGeofence
        {
            get { return _isInGeofence; }
            set { _isInGeofence = value;OnPropertyChanged("IsInGeofence"); }
        }

        public ObservableCollection<TimeSheetRequest> TimesheetList
        {
            get { return _timesheetList; }
            set { _timesheetList = value; OnPropertyChanged("TimesheetList"); }
        }
        public int SelectedSiteIndex
        {
            get { return _selectedSiteIndex; }
            set { _selectedSiteIndex = value; }
        }

        public SiteModel SelectedSite
        {
            get { return _selectedSite; }
            set { _selectedSite = value; OnPropertyChanged("SelectedSite"); }
        }

        public List<SiteModel> Sites
        {
            get { return _sites; }
            set { _sites = value;OnPropertyChanged("Sites"); }
        }

        public string LoggedTime
        {
            get { return _loggedTime; }
            set { _loggedTime = value; OnPropertyChanged("LoggedTime"); }
        }
        public DateTime CheckOutTime
        {
            get { return _checkOutTime; }
            set { _checkOutTime = value; OnPropertyChanged("CheckOutTime"); }
        }
        public DateTime CheckInTime
        {
            get { return _checkInTime; }
            set { _checkInTime = value;OnPropertyChanged("CheckInTime"); }
        }

        public bool IsCheckedIn
        {
            get { return _isCheckedIn; }
            set { _isCheckedIn = value;OnPropertyChanged("IsCheckedIn"); }
        }

        #endregion
        #region Commands
        public ICommand CancelCaptureCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        IsFaceCaptureViewVisible = false;
                        ShowToast(ResourceConstants.ErrorCapturingPhoto);
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand CapturePhotoCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        MessagingService.Send("CaptureOne", "CaptureOne");
                        IsCaptureButtonVisible = false;
                        IsSubmitButtonVisible = true;
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand CancelSafetyPopUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        HidePopUp();
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand RetakeCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        MessagingService.Send("Retake", "Retake");
                        IsCaptureButtonVisible = true;
                        IsSubmitButtonVisible = false;
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand SubmitCommand
        {
            get
            {
                return new Command(async() =>
                {
                    try
                    {
                        if (App.FaceRecogImage != null)
                        {
                            MessagingService.Send("Retake", "Retake");
                            IsFaceCaptureViewVisible = false;
                            if (IsCheckedIn)
                            {
                                var checkoutimage = Convert.ToBase64String(App.FaceRecogImage);
                                var stream = new MemoryStream(App.FaceRecogImage);
                                ShowLoader("");
                                var isFaceRecognized = await FaceRecognitionService.IsFaceIdentified(LoginUser.Email, stream).ConfigureAwait(false);
                                if (true)
                                {
                                    HideLoader();
                                    StopMonitoringAndCheckOut(checkoutimage);
                                }
                                //else
                                //{
                                //    HideLoader();
                                //    ShowToast(Resource.FaceNotMatch);
                                //}
                            }
                            else
                            {
                                CheckInImageString = Convert.ToBase64String(App.FaceRecogImage);
                                var stream = new MemoryStream(App.FaceRecogImage);
                                ShowLoader("");
                                var isFaceRecognized = await FaceRecognitionService.IsFaceIdentified(LoginUser.Email, stream).ConfigureAwait(false);
                                if (true)
                                {
                                    HideLoader();
                                    //GetMonitoringTime();
                                    ClearSafetyChecks();
                                    IsSafetyInstructionViewVisible = true;
                                }
                                //else
                                //{
                                //    HideLoader();
                                //    UserDialogs.Instance.Toast(Resource.FaceNotMatch);
                                //    UpdateFailedLoginDetail();
                                //}
                            }

                        }
                        else
                        {
                            ShowToast("Photo Not captured. Please try again");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand SafetySuccessDoneButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        HidePopUp();
                        CheckInAndStartTimer();
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand SafetySubmitButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsConfirmationYesChecked)
                    {
                        if (IsHeadProtectionChecked && IsHighVJacketChecked && IsEarProtectionChecked)
                        {
                            HidePopUp();
                            IsSafetySuccessViewVisible = true;
                        }
                        else
                        {
                            HidePopUp();
                            IsSafetyFailViewVisible = true;
                        }
                    }
                    else if(IsConfirmationNoChecked)
                    {
                        HidePopUp();
                        IsSafetyFailViewVisible = true;
                    }
                    else
                    {
                        await ShowToastAsync(ResourceConstants.SafetyMeasureErrorMessage);
                    }
                    
                });
            }
        }


        public ICommand StartTimerCommand
        {
            get
            {
                return new Command(async() =>
                {
                    try
                    {
                        //ClearSafetyChecks();
                      //  IsSafetyInstructionViewVisible = true;
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            if (await CheckBackgroundLocationEnabled())
                            {
                                if (await CheckCurrentLcoation())
                                {
                                    //IsCaptureButtonVisible = true;
                                    //IsSubmitButtonVisible = false;
                                    //IsFaceCaptureViewVisible = true;
                                    if (MediaHelper.IsCameraAvailable)
                                    {
                                        App.IsCameraNavigated = true;
                                        var photo = await MediaHelper.TakePhotoAsync(maxWidthHeight: 2000, compressionQuality: 20);
                                        if (photo != null)
                                        {
                                            var byteArray = File.ReadAllBytes(photo.FullPath);
                                            CheckInImageString = Convert.ToBase64String(byteArray);
                                            var stream = new MemoryStream(byteArray);
                            await ShowLoaderAsync("");
                                            var isFaceRecognized = await FaceRecognitionService.IsFaceIdentified(LoginUser.Email, stream).ConfigureAwait(false);
                                            if (isFaceRecognized)
                                            {
                                                //GetMonitoringTime();
                                                ClearSafetyChecks();
                                                IsSafetyInstructionViewVisible = true;
                                            }
                                            else
                                            {
                                                await ShowToastAsync(ResourceConstants.FaceNotMatch);
                                                //ShowToast(Resource.FaceNotMatch);
                                                UpdateFailedLoginDetail();
                                                //await App.Current.MainPage.DisplayAlert("Alert", Resource.FaceNotMatch, "ok");
                                            }
                                        }
                                        else
                                        {
                                            await ShowToastAsync(ResourceConstants.ErrorCapturingPhoto);
                                        }
                                        App.IsCameraNavigated = false;
                                    }
                                    else
                                    {
                                        await ShowToastAsync(ResourceConstants.CameraUnavailable);
                                    }

                                }
                                else
                                {
                                    LocationStatus = ResourceConstants.NotInSiteReach;
                                    MessagingService.Send(Constants.AnimateText, Constants.AnimateText);
                                }

                            }
                            else
                            {
                                DependencyService.Get<ILocationService>().OpenSettings();
                            }
                        }
                        else
                        {
                            InternetError();
                        }
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                    finally
                    {
                        HideLoader();
                    }
                });
            }
        }
        public ICommand SafetyFailBackButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsSafetyFailViewVisible = false;
                    IsSafetyInstructionViewVisible = true;
                });
            }
        }
        public ICommand StopTimerCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //IsCaptureButtonVisible = true;
                    //IsSubmitButtonVisible = false;
                    //IsFaceCaptureViewVisible = true;

                    try
                    {
                        SyncTime(false, DateTime.UtcNow);
                        //if (CrossMedia.Current.IsCameraAvailable)
                        //{
                        //    App.IsCameraNavigated = true;
                        //    var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                        //    {
                        //        PhotoSize = PhotoSize.Medium,
                        //        MaxWidthHeight = 2000,
                        //        DefaultCamera = CameraDevice.Front,
                        //        CompressionQuality = 20,
                        //        SaveMetaData = false
                        //    });
                        //    if (photo != null)
                        //    {
                        //        var byteArray = File.ReadAllBytes(photo.Path);
                        //        var checkoutimage = Convert.ToBase64String(byteArray);
                        //        var stream = new MemoryStream(byteArray);
                        //        ShowLoader("");
                        //        var isFaceRecognized = await FaceRecognitionService.IsFaceIdentified(LoginUser.Email, stream).ConfigureAwait(false);
                        //        if (isFaceRecognized)
                        //        {
                        //            StopMonitoringAndCheckOut(checkoutimage);
                        //        }
                        //        else
                        //        {
                        //            ShowToast(Resource.FaceNotMatch);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        ShowToast(Resource.RequiredPhotoForCheckout);
                        //    }
                        //    App.IsCameraNavigated = false;
                        //}
                        //else
                        //{
                        //    ShowToast(Resource.RequiredPhotoForCheckout);
                        //}

                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                        App.IsCameraNavigated = false;
                    }

                    
                });
            }
        }

        #endregion
        #region Constructor
        public HomePageViewModel(IDialogService dialogService = null) : base(dialogService)
        {
            _timer = new System.Timers.Timer();
            _geofencetimer = new System.Timers.Timer();
            GetUser();
            CheckInTime = DateTime.Now;
            Sites = new List<SiteModel>();
            TimesheetList = new ObservableCollection<TimeSheetRequest>();
            LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");
            HidePopUp();
            ClockInTime = "--:--";
            ClockOutTime = "--:--";
            WorkingHours = "--:--";
           
            _jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include
            };
        }



        #endregion
        #region Methods
        private async void StartListeningLocationUpdates()
        {
            try
            {
                // TODO: Replace CrossGeolocator with MAUI Essentials Geolocation background tracking
                // if (!CrossGeolocator.Current.IsListening)
                // {
                    MessagingService.Send(Constants.StartService, Constants.StartService);
                //     CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
                //     await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(30), 1);
                // }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        private void Current_PositionChanged(object sender, object e) // TODO: Replace with MAUI event args
        {
            try
            {
                // CheckLocationUpdate(e.Position); // TODO: Fix when implementing MAUI Geolocation
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }
        public async void CheckLocationUpdate(Location position) // TODO: Replace Position with Location from MAUI
        {
            try
            {
                if (!App.IsBusy)
                {
                    App.IsBusy = true;
                    if (IsGpsOn())
                    {
                        var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                        {
                            DesiredAccuracy = GeolocationAccuracy.Best,
                            Timeout = TimeSpan.FromSeconds(5)
                        });
                        if (location != null)
                        {
                            var _request = new GeofenceCheckRequest()
                            {
                                SiteMapRefLat = location.Latitude.ToString(),
                                SiteMapRefLong = location.Longitude.ToString()
                            };
                            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                            {
                                var result = await ApiHelper.CallApi(HttpMethod.Post, Constants.GeofenceSite, JsonConvert.SerializeObject(_request), true);
                                if (result.Status)
                                {
                                    var site = ApiHelper.ConvertResult<SiteModel>(result.Result);
                                    if (site != null)
                                    {
                                        var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.CheckedInSite, ""));
                                        if (currentSiteData.SiteId == site.SiteId)
                                        {
                                            Preferences.Set(Constants.CurrentSite, JsonConvert.SerializeObject(site));
                                            Preferences.Set(Constants.RecentSite, JsonConvert.SerializeObject(site));
                                            HideLoader();
                                            CurrentSite = site.SiteName;
                                            IsInGeofence = true;
                                            App.OutLocationCounter = 0;
                                            App.IsBusy = false;
                                        }
                                        else
                                        {
                                            // TODO: Replace with MAUI location tracking check
                                            // if (CrossGeolocator.Current.IsListening)
                                            // {
                                                App.OutLocationCounter++;
                                            // }
                                            if (App.OutLocationCounter == 1)
                                            {
                                                DependencyService.Get<Interface.INotificationService>().SendNotification("Out of Site", "Go back to location warning"); // TODO: Fix Resource references
                                            }
                                            if (App.OutLocationCounter == 2)
                                            {
                                                DependencyService.Get<Interface.INotificationService>().SendNotification("Out of Site", "You are signed out"); // TODO: Fix Resource references
                                                var checkouttime = DateTime.UtcNow;
                                                Preferences.Set(Constants.NativeCheckOutTime, checkouttime.ToString());
                                                // await CrossGeolocator.Current.StopListeningAsync(); // TODO: Replace with MAUI location tracking stop
                                                if (App.IsOnHomePage)
                                                {
                                                    StopMonitoring();
                                                }
                                                else
                                                {
                                                    MessagingService.Send(Constants.StopService, Constants.StopService);
                                                }
                                            }
                                            App.IsBusy = false;
                                        }
                                    }
                                }
                                else
                                {
                                    // TODO: Replace with MAUI location tracking check
                                    // if (CrossGeolocator.Current.IsListening)
                                    // {
                                        App.OutLocationCounter++;
                                    // }
                                    if (App.OutLocationCounter == 1)
                                    {
                                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.OutOfSite, Constants.GoBackLocationWarning);
                                    }
                                    if (App.OutLocationCounter == 2)
                                    {
                                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.OutOfSite, Constants.YouAreSignedOut);
                                        var checkouttime = DateTime.UtcNow;
                                        Preferences.Set(Constants.NativeCheckOutTime, checkouttime.ToString());
                                        // await CrossGeolocator.Current.StopListeningAsync(); // TODO: Replace with MAUI location tracking stop
                                        if (App.IsOnHomePage)
                                        {
                                            StopMonitoring();
                                        }
                                        else
                                        {
                                            MessagingService.Send(Constants.StopService, Constants.StopService);
                                        }
                                    }
                                    App.IsBusy = false;
                                }
                            }
                            else
                            {
                                SaveLocationInMemory(_request);
                                App.IsBusy = false;
                            }
                        }
                        else
                        {

                            // TODO: Replace with MAUI location tracking check
                            // if (CrossGeolocator.Current.IsListening)
                            // {
                                App.OutLocationCounter++;
                            // }
                            if (App.OutLocationCounter == 1)
                            {
                                DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.LocationUnavailable, Constants.LocationOnWarning);
                                return;

                            }
                            if (App.OutLocationCounter == 2)
                            {
                                DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.LocationUnavailable, Constants.YouAreSignedOut);
                                var checkouttime = DateTime.UtcNow;
                                Preferences.Set(Constants.NativeCheckOutTime, checkouttime.ToString());
                                // await CrossGeolocator.Current.StopListeningAsync(); // TODO: Replace with MAUI location tracking stop
                                if (App.IsOnHomePage)
                                {
                                    StopMonitoring();
                                }
                                else
                                {
                                    MessagingService.Send(Constants.StopService, Constants.StopService);
                                }

                                App.IsBusy = false;
                            }
                        }
                    }
                    else
                    {
                        // TODO: Replace with MAUI location tracking check
                        // if (CrossGeolocator.Current.IsListening)
                        // {
                            App.OutLocationCounter++;
                        // }
                        if (App.OutLocationCounter == 1)
                        {
                            DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.LocationUnavailable, Constants.LocationOnWarning);
                            
                            return;

                        }
                        if (App.OutLocationCounter == 2)
                        {
                            DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.LocationUnavailable, Constants.YouAreSignedOut);
                            var checkouttime = DateTime.UtcNow;
                            Preferences.Set(Constants.NativeCheckOutTime, checkouttime.ToString());
                            // await CrossGeolocator.Current.StopListeningAsync(); // TODO: Replace with MAUI location tracking stop
                            if (App.IsOnHomePage)
                            {
                                StopMonitoring();
                            }
                            else
                            {
                                MessagingService.Send(Constants.StopService, Constants.StopService);
                            }

                            App.IsBusy = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowToast(ex.Message);
                App.IsBusy = false;
            }
            finally
            {
                HideLoader();
            }
        }
        private UserTimeSheetRequest CreateTimeSheetRequest()
        {
            var _req = new UserTimeSheetRequest()
            {
                Start = 0,
                EmpId = LoginUser.EmployeeId.ToString(),
                Length = 50,
                Draw = 0,
                Columns = new Column[]
                    {
                       new Column()
                       {
                           Data = "Id",
                           Name = "Id",
                           Searchable = true,
                           Orderable = false,
                           Search = new Search()
                           {
                               IsRegex = false,
                               Value = ""
                           }
                       }
                    },
                Order = new Order[]
                    {
                        new Order()
                        {
                            Column = 0,
                            Dir = "desc"
                        }

                    },
                Search = new Search()
                {
                    Value = "",
                    IsRegex = false
                }
            };
            return _req;
        }
        public async void LoadRecetTimeSheet()
        {
            try
            {
                IsUpdatingTimeSheet = true;
                //await Task.Delay(1000);
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.EmployeeTimeSheet, JsonConvert.SerializeObject(CreateTimeSheetRequest()), true);
                if (response.Status)
                {
                    IsNonAuthTime = false;
                    var timesheet = ApiHelper.ConvertResult<List<SiteListModel>>(response.Result);
                    var data = timesheet.FirstOrDefault();
                    if (data.TimeOnSite.Date.Day == DateTime.Now.Date.Day)
                    {
                        ClockInTime = data.TimeOnSite.ToLocalTime().ToString("hh : mm tt");
                        if(data.TimeOffSite == null && data.NonAutTimeOffSite!=null)
                        {
                            ClockOutTime = data.NonAutTimeOffSite?.ToLocalTime().ToString("hh : mm tt");
                            var diffWorkingHours = (data.NonAutTimeOffSite?.ToLocalTime() - data.TimeOnSite.ToLocalTime());
                            WorkingHours = diffWorkingHours?.ToString(@"hh\:mm");
                            IsNonAuthTime = true;
                        }
                        else if(data.TimeOffSite!=null)
                        {
                            ClockOutTime = data.TimeOffSite?.ToLocalTime().ToString("hh : mm tt");
                            var diffWorkingHours = (data.TimeOffSite?.ToLocalTime() - data.TimeOnSite.ToLocalTime());
                            WorkingHours = diffWorkingHours?.ToString(@"hh\:mm");
                        }
                        
                    }
                    if(timesheet.Count > 0)
                    {
                        App.CanApplyHoliday = true;
                    }
                }
                else
                {
                    ClockInTime = "--:--";
                    ClockOutTime = "--:--";
                    WorkingHours = "--:--";
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {
                IsUpdatingTimeSheet = false;
            }
           

        }
        public async void CheckLocationStatus()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if(status == PermissionStatus.Denied)
                {
                    var z = Permissions.ShouldShowRationale<Permissions.LocationAlways>();
                    if (z)
                    {
                        var status1 = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        if(status1 == PermissionStatus.Granted)
                        {
                            var isGpsOn = DependencyService.Get<ILocationService>().IsGpsAvailable();
                            if (isGpsOn)
                            {
                                IsTurnOnButtonVisible = false;
                                if (await CheckCurrentLcoation())
                                {
                                    LocationStatus = ResourceConstants.YouAreIn + " " + CurrentSite;
                                }
                                else
                                {
                                    LocationStatus = ResourceConstants.NotInSiteReach;
                                }
                            }
                            else
                            {
                                ShowLocationSettingAlert();
                            }
                        }
                        else
                        {
                            var status2 = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                            LocationStatus = ResourceConstants.EnableLocationAndAccess;
                            IsTurnOnButtonVisible = true;
                            return;
                        }
                    }
                    else
                    {
                        var status1 = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        if (status1 == PermissionStatus.Granted)
                        {
                            var isGpsOn = DependencyService.Get<ILocationService>().IsGpsAvailable();
                            if (isGpsOn)
                            {
                                IsTurnOnButtonVisible = false;
                                if (await CheckCurrentLcoation())
                                {
                                    LocationStatus = ResourceConstants.YouAreIn + " " + CurrentSite;
                                }
                                else
                                {
                                    LocationStatus = ResourceConstants.NotInSiteReach;
                                }
                            }
                            else
                            {
                                ShowLocationSettingAlert();
                            }
                        }
                        else
                        {
                            LocationStatus = ResourceConstants.EnableLocationAndAccess;
                            IsTurnOnButtonVisible = true;
                            return;
                        }
                        
                    }
                    
                }
                else if (status != PermissionStatus.Granted)
                {
                    var request = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if(request == PermissionStatus.Granted)
                    {
                        IsTurnOnButtonVisible = false;
                        var isGpsOn = DependencyService.Get<ILocationService>().IsGpsAvailable();
                        if (isGpsOn)
                        {
                            if (await CheckCurrentLcoation())
                            {
                                LocationStatus = ResourceConstants.YouAreIn + " " + CurrentSite;
                            }
                            else
                            {
                                LocationStatus = ResourceConstants.NotInSiteReach;
                            }
                        }
                        else
                        {
                            ShowLocationSettingAlert();
                        }
                    }
                    else
                    {
                        LocationStatus = ResourceConstants.EnableLocationAndAccess;
                        IsTurnOnButtonVisible = true;
                        return;
                    }
                }
                else
                {
                    IsTurnOnButtonVisible = false;
                    var isGpsOn = DependencyService.Get<ILocationService>().IsGpsAvailable();
                    if (isGpsOn)
                    {
                        if (await CheckCurrentLcoation())
                        {
                            LocationStatus = ResourceConstants.YouAreIn + " " + CurrentSite;
                        }
                        else
                        {
                            LocationStatus = ResourceConstants.NotInSiteReach;
                        }
                    }
                    else
                    {
                        ShowLocationSettingAlert();
                    }
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
        private void UpdateFailedLoginDetail()
        {
            Application.Current.Dispatcher.Dispatch(async () =>
           {
               try
               {
                   var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.UpdateFailedLoginDetails, JsonConvert.SerializeObject(CreateFailedLoginRequest()), true);
                   if (response != null && response.Status)
                   {

                   }
               }
               catch (Exception ex)
               {
                   WriteLog(ex.Message);
               }

           });
        }

        private FailedLoginUpdateRequest CreateFailedLoginRequest()
        {
            return new FailedLoginUpdateRequest()
            {
                Description = App.FaceMathError,
                Email = LoginUser.Email,
                ZipByte = CheckInImageString
            };
        }
        public async void GetUserCheckInStatus()
        {
            try
            {
                ShowLoader("");
                if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var result = await ApiHelper.CallApi(HttpMethod.Get, Constants.UserCheckInStatus + "?empId=" + App.LoggedInUser.EmployeeId, null, true);
                    if (result.Status)
                    {
                        var data = ApiHelper.ConvertResult<CheckInStatusResponse>(result.Result);
                        if (data != null)
                        {

                            if (data.IsCheckIn)
                            {
                                if (Preferences.ContainsKey(Constants.NativeCheckOutTime))
                                {
                                    var CheckOutTime = Preferences.Get(Constants.NativeCheckOutTime, "");
                                    if (!String.IsNullOrEmpty(CheckOutTime))
                                    {
                                        StopMonitoringFromLastGeoFenceCheck(Convert.ToDateTime(CheckOutTime));
                                    }
                                }
                                else
                                {
                                    // TODO Xamarin.Forms.DeviceInfo.Platform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                                    if(DeviceInfo.Platform == DevicePlatform.iOS)
                                    {
                                        if (Preferences.ContainsKey(Constants.LastGeofenceChecked))
                                        {
                                            var lastgeofenceTime = Preferences.Get(Constants.LastGeofenceChecked, "");
                                            var diff = DateTime.UtcNow - Convert.ToDateTime(lastgeofenceTime);
                                            if(diff.Minutes > 10)
                                            {
                                                StopMonitoringFromLastGeoFenceCheck(Convert.ToDateTime(lastgeofenceTime));
                                            }
                                            else
                                            {
                                                CheckInTime = data.CheckInTime;
                                                Preferences.Set(Constants.ClockInTime, data.CheckInTime.ToString());
                                                IsCheckedIn = true;
                                                App.CheckedIn = true;
                                                IsCheckedInViewVisible = true;
                                                IsNotCheckedInViewVisible = false;
                                                StartTimer();
                                                StartMonitoring();
                                            }
                                        }
                                        else
                                        {
                                            CheckInTime = data.CheckInTime;
                                            Preferences.Set(Constants.ClockInTime, data.CheckInTime.ToString());
                                            IsCheckedIn = true;
                                            App.CheckedIn = true;
                                            IsCheckedInViewVisible = true;
                                            IsNotCheckedInViewVisible = false;
                                            StartTimer();
                                            StartMonitoring();
                                        }
                                    }
                                    else
                                    {
                                        CheckInTime = data.CheckInTime;
                                        Preferences.Set(Constants.ClockInTime, data.CheckInTime.ToString());
                                        IsCheckedIn = true;
                                        App.CheckedIn = true;
                                        IsCheckedInViewVisible = true;
                                        IsNotCheckedInViewVisible = false;
                                        MessagingService.Send(Constants.StopService, Constants.StopService);
                                        StartTimer();
                                        StartMonitoring();
                                    }
                                   
                                }
                            }
                            else
                            {
                                IsNotCheckedInViewVisible = true;
                                IsCheckedInViewVisible = false;
                                IsCheckedIn = false;
                                App.CheckedIn = false;
                                LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");
                                MessagingService.Send(Constants.StopService, Constants.StopService);
                            }
                        }
                        else
                        {
                            IsNotCheckedInViewVisible = true;
                            IsCheckedInViewVisible = false;
                            IsCheckedIn = false;
                            App.CheckedIn = false;
                            LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");
                            MessagingService.Send(Constants.StopService, Constants.StopService);
                        }
                    }
                    else
                    {
                        if (result.Message.ToLower() == "no records found")
                        {
                            IsNotCheckedInViewVisible = true;
                            IsCheckedInViewVisible = false;
                            IsCheckedIn = false;
                            App.CheckedIn = false;
                            LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");
                            MessagingService.Send(Constants.StopService, Constants.StopService);
                        }
                        else
                        {
                            await ShowToastAsync(result.Message);
                            MessagingService.Send(Constants.StopService, Constants.StopService);
                        }
                    }
                }
                else
                {
                    InternetError();
                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessagingService.Send(Constants.StopService, Constants.StopService);
            }
            finally
            {
                HideLoader();
            }
        }
        private void HidePopUp()
        {
            IsSafetyInstructionViewVisible = false;
            IsSafetySuccessViewVisible = false;
            IsSafetyFailViewVisible = false;
        }
        public async Task CheckGeofence()
        {
            try
            {
                var status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
                if (status != PermissionStatus.Granted)
                {
                    // Notify user permission was denied
                    //ShowToast(Resource.PermissionError);
                    if (IsCheckedIn)
                    {
                        App.OutLocationCounter++;
                    }
                    if (App.OutLocationCounter == 1)
                    {
                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.AppName, "Please grant location permission to always in app settings");
                    }
                    if (App.OutLocationCounter == 2)
                    {
                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.AppName, Constants.YouAreSignedOut);
                        StopMonitoring();
                    }
                    IsInGeofence = false;
                    return;
                }
                var gpsStatus = DependencyService.Get<ILocationService>().IsGpsAvailable();
                if (gpsStatus)
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                    {
                        DesiredAccuracy = GeolocationAccuracy.Best,
                        Timeout = TimeSpan.FromSeconds(5)
                    });
                    if (location != null)
                    {
                        //Preferences.Set("LastGeofenceTime", DateTime.UtcNow);
                        var _request = new GeofenceCheckRequest()
                        {
                            SiteMapRefLat = location.Latitude.ToString(),
                            SiteMapRefLong = location.Longitude.ToString()
                        };
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            var result = await ApiHelper.CallApi(HttpMethod.Post, Constants.GeofenceSite, JsonConvert.SerializeObject(_request), true);
                            if (result.Status)
                            {
                                var site = ApiHelper.ConvertResult<SiteModel>(result.Result);
                                if (site != null)
                                {
                                    var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.CurrentSite, ""));
                                    if(currentSiteData.SiteId == site.SiteId)
                                    {
                                        Preferences.Set(Constants.CurrentSite, JsonConvert.SerializeObject(site));
                                        Preferences.Set(Constants.RecentSite, JsonConvert.SerializeObject(site));
                                        HideLoader();
                                        await ShowToastAsync(ResourceConstants.OnActiveSite + " " + site.SiteName);
                                        //ShowToast(Resource.OnActiveSite + " " + site.SiteName);
                                        CurrentSite = site.SiteName;
                                        IsInGeofence = true;
                                        App.OutLocationCounter = 0;
                                    }
                                    else
                                    {
                                        if (IsCheckedIn)
                                        {
                                            App.OutLocationCounter++;
                                        }
                                        if (App.OutLocationCounter == 1)
                                        {
                                            DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.OutOfSite, Constants.GoBackLocationWarning);
                                        }
                                        if (App.OutLocationCounter == 2)
                                        {
                                            DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.OutOfSite, Constants.YouAreSignedOut);
                                            StopMonitoring();
                                        }
                                        IsInGeofence = false;
                                        //ShowToast(Resource.NotOnActiveSite);
                                    }
                                    //if (IsInGeofence && !IsCheckedIn)
                                    //{
                                    //    DependencyService.Get<Interface.INotificationService>().SendNotification("CCIMIGRATION", "You are on valid site Please check in");
                                    //}
                                    //StartTimer();
                                }
                            }
                            else
                            {
                                if (IsCheckedIn)
                                {
                                    App.OutLocationCounter++;
                                }
                                if (App.OutLocationCounter == 1)
                                {
                                    DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.OutOfSite, Constants.GoBackLocationWarning);
                                }
                                if (App.OutLocationCounter == 2)
                                {
                                    DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.OutOfSite, Constants.YouAreSignedOut);
                                    StopMonitoring();
                                }
                                IsInGeofence = false;
                                //ShowToast(Resource.NotOnActiveSite);
                            }
                        }
                        else
                        {
                            SaveLocationInMemory(_request);
                        }
                    }
                    else
                    {
                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.YouAreSignedOut, ResourceConstants.NeedLocationInBackground);
                        StopMonitoring();
                    }
                }
                else
                {
                    if (App.CheckedIn)
                    {
                        App.OutLocationCounter++;
                    }
                    if (App.OutLocationCounter == 1)
                    {
                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.LocationUnavailable, Constants.LocationOnWarning);
                        ShowLocationSettingAlert();
                        return;
                        
                    }
                    if (App.OutLocationCounter == 2)
                    {
                        DependencyService.Get<Interface.INotificationService>().SendNotification(Constants.LocationUnavailable, Constants.YouAreSignedOut);
                        StopMonitoring();
                    }
                    IsInGeofence = false;

                }
               
            }
            catch(Exception ex)
            {
                await ShowToastAsync(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }
        private async Task<bool> CheckBackgroundLocationEnabled()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if(status == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    if (await App.Current.MainPage.DisplayAlert(ResourceConstants.LocationRequired, ResourceConstants.LocationConsent, ResourceConstants.AllowInSettings, ResourceConstants.Cancel))
                    {
                        // TODO Xamarin.Forms.DeviceInfo.Platform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if(DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            if (Permissions.ShouldShowRationale<LocationAlways>())
                            {
                                var status1 = await Permissions.RequestAsync<Permissions.LocationAlways>();
                                if (status1 == PermissionStatus.Granted)
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
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
                return false;
            }
        }
        public async Task<bool> CheckCurrentLcoation()
        {
            try
            {
                ShowLoader();
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(5)
                });
                if (location != null)
                {
                    var _request = new GeofenceCheckRequest()
                    {
                        SiteMapRefLat = location.Latitude.ToString(),
                        SiteMapRefLong = location.Longitude.ToString()
                    };

                    var result = await ApiHelper.CallApi(HttpMethod.Post, Constants.GeofenceSite, JsonConvert.SerializeObject(_request), true);
                    if (result.Status)
                    {
                        var site = ApiHelper.ConvertResult<SiteModel>(result.Result);
                        if (site != null)
                        {
                            IsInGeofence = true;
                            Preferences.Set(Constants.CurrentSite, JsonConvert.SerializeObject(site));
                            Preferences.Set(Constants.CheckedInSite, JsonConvert.SerializeObject(site));
                            //Preferences.Set(Constants.RecentSite, JsonConvert.SerializeObject(site));
                            CurrentSite = site.SiteName;
                            LocationStatus = ResourceConstants.YouAreIn + " " + CurrentSite;
                            return true;
                        }
                        else
                        {
                            IsInGeofence = false;
                            return false;
                        }
                    }
                    else
                    {
                        IsInGeofence = false;
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
                ShowToast(ex.Message);
                return false;
            }
            finally
            {
                HideLoader();
            }
        }

        private async void SaveLocationInMemory(GeofenceCheckRequest request)
        {
            try
            {
                await App.Repository.CreateAsync<LocationModel>(new LocationModel()
                {
                    TimeStamp = DateTime.UtcNow,
                    Latitude = request.SiteMapRefLat,
                    Longitude = request.SiteMapRefLong
                }, true);
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }
        }
        public async void StopMonitoringFromLastGeoFenceCheck(DateTime checkoutTime)
        {
            try
            {
                CheckOutTime = checkoutTime;              
                await SyncTime(true,checkoutTime, null);

            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }
        public async void StopMonitoring()
        {
            try
            {
                CheckOutTime = DateTime.UtcNow;
                var checkOutTime = Preferences.Get(Constants.NativeCheckOutTime, "");
                _timer.Stop();
                await SyncTime(true,Convert.ToDateTime(checkOutTime),null);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }
        public async void StopMonitoringAndCheckOut(string image)
        {
            try
            {
                CheckOutTime = DateTime.UtcNow;
                _timer.Stop();
                await SyncTime(false,CheckOutTime,image);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        public void StartMonitoring()
        {
            try
            {

                // TODO Xamarin.Forms.DeviceInfo.Platform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    
                    MessagingService.Send(Constants.StartService, Constants.StartService);
                    //StartListeningLocationUpdates();
                }
                else
                {
                    DependencyService.Get<IMonitoringService>().StartMonitoring();
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }
        }
        private async Task SyncTime(bool IsNonAuthCheckOut,DateTime checkoutTime, string image = null)
        {
            try
            {
                GetUser();
                var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.CheckedInSite, ""));
                if (currentSiteData != null)
                {

                    var checkintime = Preferences.Get(Constants.ClockInTime, "");
                    var request = new TimesheetUpdateRequest()
                    {
                        EmployeeId = LoginUser.EmployeeId.ToString(),
                        SiteId = currentSiteData.SiteId.ToString(),
                        TimeOffSite = checkoutTime,
                        CheckOutImage = image,
                        IsNonAuthTimeOffSite = IsNonAuthCheckOut
                    };
                    if (!String.IsNullOrEmpty(checkintime))
                    {
                        request.TimeOnSite = Convert.ToDateTime(checkintime);
                    }
                    ShowLoader(ResourceConstants.SyncingTime);
                    var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.TimeSheet + Constants.CreateUpdateTimeSheet, JsonConvert.SerializeObject(request), true);
                    if (response != null)
                    {
                        if (response.Status)
                        {
                            Preferences.Remove(Constants.CheckInTime);
                            Preferences.Remove(Constants.ClockInTime);
                            Preferences.Remove(Constants.NativeCheckOutTime);
                            Preferences.Remove(Constants.LastGeofenceChecked);
                            ShowToast(ResourceConstants.TimeSynced);
                            LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");
                            //ClockOutTime = CheckOutTime.ToLocalTime().ToString(@"hh\:mm tt");
                            MessagingService.Send(Constants.StopService, Constants.StopService);
                            var diffWorkingHours = (CheckOutTime.ToLocalTime() - CheckInTime.ToLocalTime());
                            WorkingHours = diffWorkingHours.ToString(@"hh\:mm");
                            IsCheckedIn = false;
                            App.CheckedIn = false;
                            IsCheckedInViewVisible = false;
                            IsNotCheckedInViewVisible = true;
                            LoadRecetTimeSheet();
                            CheckLocationStatus();
                        }
                        else
                        {
                            ShowToast(response.Message);
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.SomethingWentWrong);
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.CurrentSiteError);
                }

            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                ShowToast(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }
        
        
        public async void CheckInAndStartTimer()
        {
            try
            {
                ShowLoader("Checking In...");
                var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.CurrentSite, ""));
                if (currentSiteData != null)
                {
                    CheckInTime = DateTime.UtcNow;
                    var _req = new CheckInTimesheetUpdateRequest()
                    {
                        EmployeeId = LoginUser.EmployeeId.ToString(),
                        SiteId = currentSiteData.SiteId.ToString(),
                        TimeOnSite = CheckInTime,
                        CheckInImage = CheckInImageString
                    };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.CheckIn, JsonConvert.SerializeObject(_req), true);
                    if (response.Status)
                    {
                        //Preferences.Set("LastGeofenceTime", DateTime.UtcNow);
                        ClockInTime = CheckInTime.ToLocalTime().ToString(@"hh\:mm tt");
                        
                        ClockOutTime = "--:--";
                        WorkingHours = "--:--";
                        IsNonAuthTime = false;
                        IsNotCheckedInViewVisible = false;
                        IsCheckedInViewVisible = true;
                      
                        App.OutLocationCounter = 0;
                        Preferences.Set(Constants.CheckInTime, CheckInTime);
                        var clockintime = CheckInTime.ToString();
                        Preferences.Set(Constants.ClockInTime, CheckInTime.ToString());
                        ClearSafetyChecks();
                        StartMonitoring();
                        StartTimer();
                        IsCheckedIn = true;
                        App.CheckedIn = true;
                        //Temporary to be removed later
                        Preferences.Set("OutLocation", "0");
                        Preferences.Set("CheckedIn", "true");
                        var data = await App.Repository.GetAsync<LocationModel>();
                        if (data != null && data.Count > 0)
                        {
                            await App.Repository.DeleteAllAsync<LocationModel>();
                        }
                    }
                    else
                    {
                        ShowToast(response.Message);
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.CurrentSiteError);
                }
            }
            catch(Exception ex)
            {
                ShowToast(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }

        private void ClearSafetyChecks()
        {
            IsHeadProtectionChecked = false;
            IsEarProtectionChecked = false;
            IsHighVJacketChecked = false;
            IsConfirmationYesChecked = false;
            IsConfirmationNoChecked = false;
        }

        private void StartTimer()
        {
           
            _timer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    StartLogging();
                });
            };
            _timer.Interval = 1000;
            _timer.AutoReset = true;
            _timer.Start();
        }
        private void StartLogging()
        {
            LoggedTime = (DateTime.UtcNow - CheckInTime).ToString(@"hh\:mm\:ss");
        }
        public async void SyncLocations(List<LocationModel> _data)
        {
            try
            {
                OfflineTimeList.Clear();
                foreach (var item in _data)
                {
                    var entry = new OfflineTime()
                    {
                        SiteMapRefLat = item.Latitude,
                        SiteMapRefLong = item.Longitude,
                        TimeInSite = item.TimeStamp
                    };
                    OfflineTimeList.Add(entry);
                }
                ShowLoader("Syncing data");
                var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.CheckedInSite, ""));
                if (currentSiteData != null)
                {
                    var _request = new OfflineTimeSheetSyncModel()
                    {
                        EmpId = App.LoggedInUser.EmployeeId,
                        OfflineTime = OfflineTimeList.ToArray(),
                        SiteId = currentSiteData.SiteId.ToString()
                    };
                    var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.SyncOfflineTimeSheet, JsonConvert.SerializeObject(_request), true);
                    if (response.Status)
                    {
                        await App.Repository.DeleteAllAsync<LocationModel>();
                        var result = ApiHelper.ConvertResult<OfflineSyncResponse>(response.Result);
                        if (result.IsCheckedIn && !Preferences.ContainsKey(Constants.NativeCheckOutTime))
                        {
                            CheckInTime = Convert.ToDateTime(result.CheckInTime);
                            Preferences.Set(Constants.ClockInTime, Convert.ToDateTime(result.CheckInTime).ToString());
                            IsCheckedIn = true;
                            App.CheckedIn = true;
                            IsCheckedInViewVisible = true;
                            IsNotCheckedInViewVisible = false;
                            StartTimer();
                            StartMonitoring();
                        }
                        else
                        {
                            IsNotCheckedInViewVisible = true;
                            IsCheckedInViewVisible = false;
                            IsCheckedIn = false;
                            App.CheckedIn = false;
                            LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");
                            MessagingService.Send(Constants.StopService, Constants.StopService);
                            LoadRecetTimeSheet();
                            CheckLocationStatus();
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", ResourceConstants.OfflineSyncError, "Ok");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Error Getting Checked in site", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }
        #endregion
    }
}
