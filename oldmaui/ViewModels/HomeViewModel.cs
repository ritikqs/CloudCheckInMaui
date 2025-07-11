using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Timers;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using CloudCheckInMaui.ConstantHelper;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;
using CloudCheckInMaui.Services;
using CloudCheckInMaui.Services.FaceService;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace CloudCheckInMaui.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        private string _userInitials;
        public string UserInitials
        {
            get => _userInitials;
            set => SetProperty(ref _userInitials, value);
        }

        private bool _isTimeSheetEnabled;
        public bool IsTimeSheetEnabled
        {
            get => _isTimeSheetEnabled;
            set => SetProperty(ref _isTimeSheetEnabled, value);
        }

        private bool _isHolidayEnabled;
        public bool IsHolidayEnabled
        {
            get => _isHolidayEnabled;
            set => SetProperty(ref _isHolidayEnabled, value);
        }

        private bool _isAlertsEnabled;
        public bool IsAlertsEnabled
        {
            get => _isAlertsEnabled;
            set => SetProperty(ref _isAlertsEnabled, value);
        }

        private bool _isProfileEnabled;
        public bool IsProfileEnabled
        {
            get => _isProfileEnabled;
            set => SetProperty(ref _isProfileEnabled, value);
        }

        private int _unreadAlertCount;
        public int UnreadAlertCount
        {
            get => _unreadAlertCount;
            set => SetProperty(ref _unreadAlertCount, value);
        }

        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }

        private readonly IGeolocation _geolocation;
        private readonly TimerService _timerService;
        private readonly IFaceRecognitionService _faceRecognitionService;
        private DateTime _checkInTime;
        private string _loggedTime;
        private string _locationStatus;
        private bool _isCheckedIn;
        private bool _isCheckedInViewVisible;
        private bool _isNotCheckedInViewVisible = true;
        private bool _isTurnOnButtonVisible;

        #region PPE Modal Properties
        private bool _isSafetyInstructionViewVisible;
        public bool IsSafetyInstructionViewVisible
        {
            get => _isSafetyInstructionViewVisible;
            set => SetProperty(ref _isSafetyInstructionViewVisible, value);
        }

        private bool _isSafetySuccessViewVisible;
        public bool IsSafetySuccessViewVisible
        {
            get => _isSafetySuccessViewVisible;
            set => SetProperty(ref _isSafetySuccessViewVisible, value);
        }

        private bool _isSafetyFailViewVisible;
        public bool IsSafetyFailViewVisible
        {
            get => _isSafetyFailViewVisible;
            set => SetProperty(ref _isSafetyFailViewVisible, value);
        }

        private bool _isHeadProtectionChecked;
        public bool IsHeadProtectionChecked
        {
            get => _isHeadProtectionChecked;
            set => SetProperty(ref _isHeadProtectionChecked, value);
        }

        private bool _isHighVJacketChecked;
        public bool IsHighVJacketChecked
        {
            get => _isHighVJacketChecked;
            set => SetProperty(ref _isHighVJacketChecked, value);
        }

        private bool _isEarProtectionChecked;
        public bool IsEarProtectionChecked
        {
            get => _isEarProtectionChecked;
            set => SetProperty(ref _isEarProtectionChecked, value);
        }

        private bool _isConfirmationYesChecked;
        public bool IsConfirmationYesChecked
        {
            get => _isConfirmationYesChecked;
            set => SetProperty(ref _isConfirmationYesChecked, value);
        }

        private bool _isConfirmationNoChecked;
        public bool IsConfirmationNoChecked
        {
            get => _isConfirmationNoChecked;
            set => SetProperty(ref _isConfirmationNoChecked, value);
        }

        private string _currentSite = "Current Site";
        public string CurrentSite
        {
            get => _currentSite;
            set => SetProperty(ref _currentSite, value);
        }
        #endregion

        #region PPE Modal Commands
        public ICommand SafetySubmitButtonCommand { get; }
        public ICommand SafetySuccessDoneButtonCommand { get; }
        public ICommand SafetyFailBackButtonCommand { get; }
        public ICommand CancelSafetyPopUpCommand { get; }
        #endregion

        public string LoggedTime
        {
            get => _loggedTime;
            set => SetProperty(ref _loggedTime, value);
        }

        public string LocationStatus
        {
            get => _locationStatus;
            set => SetProperty(ref _locationStatus, value);
        }

        public bool IsCheckedIn
        {
            get => _isCheckedIn;
            set => SetProperty(ref _isCheckedIn, value);
        }

        public bool IsCheckedInViewVisible
        {
            get => _isCheckedInViewVisible;
            set => SetProperty(ref _isCheckedInViewVisible, value);
        }

        public bool IsNotCheckedInViewVisible
        {
            get => _isNotCheckedInViewVisible;
            set => SetProperty(ref _isNotCheckedInViewVisible, value);
        }

        public bool IsTurnOnButtonVisible
        {
            get => _isTurnOnButtonVisible;
            set => SetProperty(ref _isTurnOnButtonVisible, value);
        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateToTimesheetCommand { get; }
        public ICommand NavigateToHolidayCommand { get; }
        public ICommand NavigateToAlertsCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public ICommand CheckUnreadAlertsCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        private string _clockInTime;
        public string ClockInTime
        {
            get => _clockInTime;
            set => SetProperty(ref _clockInTime, value);
        }

        private string _clockOutTime;
        public string ClockOutTime
        {
            get => _clockOutTime;
            set => SetProperty(ref _clockOutTime, value);
        }

        private bool _isNonAuthTime;
        public bool IsNonAuthTime
        {
            get => _isNonAuthTime;
            set => SetProperty(ref _isNonAuthTime, value);
        }

        public HomeViewModel(IGeolocation geolocation, IFaceRecognitionService faceRecognitionService)
        {
            _geolocation = geolocation;
            _faceRecognitionService = faceRecognitionService;
            _timerService = TimerService.Instance;

            // Initialize time properties to default
            ClockInTime = "--:--";
            ClockOutTime = "--:--";
            IsNonAuthTime = false;

            // Restore _checkInTime from preferences if available
            var clockInTimeStr = Preferences.Get("ClockInTime", null);
            if (!string.IsNullOrEmpty(clockInTimeStr) && DateTime.TryParse(clockInTimeStr, out var parsedTime))
            {
                _checkInTime = parsedTime;
                ClockInTime = _checkInTime.ToLocalTime().ToString("hh : mm tt");
            }
            else
            {
                _checkInTime = DateTime.MinValue;
            }

            // Always hook events (safe to do multiple times if you unhook in Cleanup)
            _timerService.TimerTick -= OnTimerTick;
            _timerService.TimerTick += OnTimerTick;
            _timerService.LocationWarning -= OnLocationWarning;
            _timerService.LocationWarning += OnLocationWarning;
            _timerService.AutoCheckOut -= OnAutoCheckOut;
            _timerService.AutoCheckOut += OnAutoCheckOut;

            StartTimerCommand = new Command(async () => await StartTimerAsync());
            StopTimerCommand = new Command(async () => await StopTimerAsync());

            // Initialize PPE Modal Commands
            SafetySubmitButtonCommand = new Command(OnSafetySubmit);
            SafetySuccessDoneButtonCommand = new Command(OnSafetySuccessDone);
            SafetyFailBackButtonCommand = new Command(OnSafetyFailBack);
            CancelSafetyPopUpCommand = new Command(OnCancelSafetyPopUp);

            Title = "Home";
            LoadDataCommand = new Command(async () => await LoadData());
            LogoutCommand = new Command(async () => await Logout());
            NavigateToTimesheetCommand = new Command(async () => await NavigateToTimesheet());
            NavigateToHolidayCommand = new Command(async () => await NavigateToHoliday());
            NavigateToAlertsCommand = new Command(async () => await NavigateToAlerts());
            NavigateToProfileCommand = new Command(async () => await NavigateToProfile());
            CheckUnreadAlertsCommand = new Command(async () => await CheckUnreadAlerts());
            MenuCommand = new Command(() => OpenMenu());
            
            // Initialize
            SetupUserInfo();
            
            // Check for unread alerts periodically
            Task.Run(async () => await CheckUnreadAlerts());

            CheckLocationStatus();
        }

        public async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
                await LoadData();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to initialize: " + ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadData()
        {
            try
            {
                SetupUserInfo();
                await CheckUnreadAlerts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void SetupUserInfo()
        {
            if (App.LoggedInUser != null)
            {
                WelcomeMessage = $"Welcome {App.LoggedInUser.FirstName} {App.LoggedInUser.LastName}";
                UserInitials = !string.IsNullOrEmpty(App.LoggedInUser.FirstName) && !string.IsNullOrEmpty(App.LoggedInUser.LastName) 
                    ? $"{App.LoggedInUser.FirstName[0]}{App.LoggedInUser.LastName[0]}"
                    : "??";
                
                // Set user role
                UserRole = App.LoggedInUser.Role;
                
                // Enable features based on role
                IsTimeSheetEnabled = true; // Available to all users
                IsHolidayEnabled = true; // Available to all users
                IsAlertsEnabled = true; // Available to all users
                IsProfileEnabled = true; // Available to all users
            }
        }

        private async Task CheckUnreadAlerts()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, $"alert/unread/{App.LoggedInUser.EmployeeId}");
                
                if (response.Status)
                {
                    UnreadAlertCount = response.GetData<int>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to check unread alerts: {ex.Message}");
            }
        }

        private async Task Logout()
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            
            if (confirm)
            {
                try
                {
                    App.ShowLoader();
                    
                    // Call logout API
                    var response = await ApiHelper.CallApi(HttpMethod.Post, "account/logout");
                    
                    if (response.Status)
                    {
                        // Clear user data
                        App.LoggedInUser = null;
                        App.Token = string.Empty;
                        
                        // Use absolute path for navigation
                        await Shell.Current.GoToAsync("//Login");
                    }
                    else
                    {
                        await DisplayAlert("Error", response.Message, "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
                finally
                {
                    App.HideLoader();
                }
            }
        }

        private void OpenMenu()
        {
            // Implement menu opening logic
            Shell.Current.FlyoutIsPresented = true;
        }

        private void SwitchTab(int tabIndex)
        {
            // Use Shell navigation to switch tabs
            var route = tabIndex switch
            {
                0 => "///MainApp/Home",
                1 => "///MainApp/Timesheet",
                2 => "///MainApp/Holiday",
                3 => "///MainApp/Messages",
                4 => "///MainApp/Alerts",
                _ => "///MainApp/Home"
            };
            Shell.Current.GoToAsync(route);
        }

        private async Task NavigateToTimesheet()
        {
            await Shell.Current.GoToAsync("///MainApp/Timesheet");
        }

        private async Task NavigateToHoliday()
        {
            await Shell.Current.GoToAsync("///MainApp/Holiday");
        }

        private async Task NavigateToAlerts()
        {
            await Shell.Current.GoToAsync("///MainApp/Alerts");
        }

        private async Task NavigateToProfile()
        {
            await Shell.Current.GoToAsync("///Profile");
        }

        private void OnSafetySubmit()
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
            else if (IsConfirmationNoChecked)
            {
                HidePopUp();
                IsSafetyFailViewVisible = true;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please confirm if you have all the required safety measures.", "OK");
            }
        }

        private void OnSafetySuccessDone()
        {
            HidePopUp();
            StartTimerAndCheckIn();
        }

        private void OnSafetyFailBack()
        {
            IsSafetyFailViewVisible = false;
            IsSafetyInstructionViewVisible = true;
        }

        private void OnCancelSafetyPopUp()
        {
            HidePopUp();
        }

        private void HidePopUp()
        {
            IsSafetyInstructionViewVisible = false;
            IsSafetySuccessViewVisible = false;
            IsSafetyFailViewVisible = false;
        }

        private void ClearSafetyChecks()
        {
            IsHeadProtectionChecked = false;
            IsHighVJacketChecked = false;
            IsEarProtectionChecked = false;
            IsConfirmationYesChecked = false;
            IsConfirmationNoChecked = false;
        }

        private async Task StartTimerAsync()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection available", "OK");
                    return;
                }

                // Check all required permissions
                var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                var backgroundLocationStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                var mediaStatus = await Permissions.CheckStatusAsync<Permissions.Media>();

                // If any permission is not granted, request all permissions
                if (locationStatus != PermissionStatus.Granted ||
                    backgroundLocationStatus != PermissionStatus.Granted ||
                    cameraStatus != PermissionStatus.Granted ||
                    mediaStatus != PermissionStatus.Granted)
                {
                    var message = "The following permissions are required to start your shift:\n\n";
                    if (locationStatus != PermissionStatus.Granted) message += "- Location\n";
                    if (backgroundLocationStatus != PermissionStatus.Granted) message += "- Background Location\n";
                    if (cameraStatus != PermissionStatus.Granted) message += "- Camera\n";
                    if (mediaStatus != PermissionStatus.Granted) message += "- Media/Storage\n";
                    
                    message += "\nWould you like to grant these permissions now?";

                    var shouldRequest = await Application.Current.MainPage.DisplayAlert(
                        "Permissions Required",
                        message,
                        "Yes",
                        "No"
                    );

                    if (!shouldRequest)
                    {
                        return;
                    }

                    // Request permissions
                    locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    backgroundLocationStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                    mediaStatus = await Permissions.RequestAsync<Permissions.Media>();

                    if (locationStatus != PermissionStatus.Granted ||
                        backgroundLocationStatus != PermissionStatus.Granted ||
                        cameraStatus != PermissionStatus.Granted ||
                        mediaStatus != PermissionStatus.Granted)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Permission Required",
                            "All permissions are required to start your shift. Please enable them in your device settings.",
                            "OK"
                        );
                        return;
                    }
                }

                if (!await CheckCurrentLocation())
                {
                    await Application.Current.MainPage.DisplayAlert("Location Error", "Unable to get your current location. Please ensure location services are enabled.", "OK");
                    return;
                }

                // Show loading indicator
                IsBusy = true;

                try
                {
                    // Set camera navigation flag to prevent token validation during camera process
                    App.IsCameraNavigated = true;
                    
                    // Capture photo using MediaPicker
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to capture photo", "OK");
                        return;
                    }

                    // Verify face
                    byte[] photoBytes;
                    using (var stream = await photo.OpenReadAsync())
                    {
                        using var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        photoBytes = memoryStream.ToArray();
                    }

                    System.Diagnostics.Debug.WriteLine($"Photo captured, size: {photoBytes.Length} bytes");
                    System.Diagnostics.Debug.WriteLine($"Attempting face verification for user: {App.LoggedInUser.Email}");
                    
                    // Check if Face API is properly configured
                    var isConfigValid = await _faceRecognitionService.IsConfigurationValid();
                    if (!isConfigValid)
                    {
                        System.Diagnostics.Debug.WriteLine("Face API configuration is invalid");
                        await Application.Current.MainPage.DisplayAlert("Configuration Error", "Face recognition service is not properly configured. Please check your API settings.", "OK");
                        return;
                    }
                    
                    // Create a fresh stream for face recognition
                    using var faceStream = new MemoryStream(photoBytes);
                    var isFaceVerified = await _faceRecognitionService.IsFaceIdentified(App.LoggedInUser.Email, faceStream);
                    if (!isFaceVerified)
                    {
                        var errorMessage = !string.IsNullOrEmpty(App.FaceMathError) 
                            ? App.FaceMathError 
                            : "Face verification failed. Please try again.";
                        
                        System.Diagnostics.Debug.WriteLine($"Face verification failed: {errorMessage}");
                        await Application.Current.MainPage.DisplayAlert("Face Verification Failed", errorMessage, "OK");
                        return;
                    }
                    
                    System.Diagnostics.Debug.WriteLine("Face verification successful, proceeding with check-in");

                    // Start the timer and check-in process
                    await StartTimerAndCheckIn();
                }
                finally
                {
                    // Reset camera navigation flag
                    App.IsCameraNavigated = false;
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                // Reset camera navigation flag on error
                App.IsCameraNavigated = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task StartTimerAndCheckIn()
        {
            try
            {
                // Get current location
                var location = await _geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(5)
                });

                if (location == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to get your current location", "OK");
                    return;
                }

                // Get current site based on location
                var site = await GetCurrentSiteAsync(location.Latitude, location.Longitude);
                if (site == null)
                {
                    return;
                }

                // Store the site information
                Preferences.Set("CheckedInSite", JsonConvert.SerializeObject(site));
                Preferences.Set("CurrentSite", JsonConvert.SerializeObject(site));
                CurrentSite = site.SiteName;

                // Start the shift
                _checkInTime = DateTime.UtcNow;
                Preferences.Set("ClockInTime", _checkInTime.ToString("o"));
                IsCheckedIn = true;
                IsCheckedInViewVisible = true;
                IsNotCheckedInViewVisible = false;
                StartTimer();
                StartLocationMonitoring();

                ClockInTime = _checkInTime.ToLocalTime().ToString("hh : mm tt");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void StartTimer()
        {
            try
            {
                _timerService.StartTimer(_checkInTime);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting timer: {ex.Message}");
            }
        }

        private void OnTimerTick(object sender, string timeString)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoggedTime = timeString;
            });
        }

        private async Task<bool> CheckLocationPermissions()
        {
            try
            {
                // First check and request location permission
                var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (locationStatus != PermissionStatus.Granted)
                {
                    locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (locationStatus != PermissionStatus.Granted)
                    {
                        return false;
                    }
                }

                // Then check and request background location permission
                var backgroundLocationStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if (backgroundLocationStatus != PermissionStatus.Granted)
                {
                    backgroundLocationStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();
                    if (backgroundLocationStatus != PermissionStatus.Granted)
                    {
                        return false;
                    }
                }

                // Check and request camera permission
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                    if (cameraStatus != PermissionStatus.Granted)
                    {
                        return false;
                    }
                }

                // Check and request media permission
                var mediaStatus = await Permissions.CheckStatusAsync<Permissions.Media>();
                if (mediaStatus != PermissionStatus.Granted)
                {
                    mediaStatus = await Permissions.RequestAsync<Permissions.Media>();
                    if (mediaStatus != PermissionStatus.Granted)
                    {
                        return false;
                    }
                }

                IsTurnOnButtonVisible = false;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking permissions: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> CheckCurrentLocation()
        {
            try
            {
                var location = await _geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await _geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(5)
                    });
                }

                if (location != null)
                {
                    LocationStatus = "Location available";
                    return true;
                }

                LocationStatus = "Location unavailable";
                return false;
            }
            catch (Exception ex)
            {
                LocationStatus = "Location error: " + ex.Message;
                return false;
            }
        }

        public async Task CheckLocationStatus()
        {
            await CheckLocationPermissions();
            await CheckCurrentLocation();
        }

        private void StartLocationMonitoring()
        {
            // Implement background location monitoring
            // This will depend on your specific requirements and platform-specific implementations
        }

        private void StopLocationMonitoring()
        {
            // Implement stopping background location monitoring
        }

        public void Cleanup()
        {
            try
            {
                _timerService.TimerTick -= OnTimerTick;
                _timerService.LocationWarning -= OnLocationWarning;
                _timerService.AutoCheckOut -= OnAutoCheckOut;
                _timerService.Cleanup();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error cleaning up: {ex.Message}");
            }
        }

        private async Task StopTimerAsync()
        {
            try
            {
                if (!IsCheckedIn)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You are not checked in", "OK");
                    return;
                }

                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "End Shift",
                    "Are you sure you want to end your shift?",
                    "Yes",
                    "No"
                );

                if (!confirm)
                {
                    return;
                }

                IsBusy = true;
                await PerformCheckOut(DateTime.UtcNow, false);
                _timerService.StopTimer();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnLocationWarning(object sender, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Location Warning", message, "OK");
            });
        }

        private async void OnAutoCheckOut(object sender, DateTime checkoutTime)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Auto Checkout", "You have been automatically checked out due to being outside the site boundary.", "OK");
                await PerformCheckOut(checkoutTime, true);
            });
        }

        private async Task PerformCheckOut(DateTime checkoutTime, bool isNonAuthCheckOut)
        {
            try
            {
                var currentSiteData = JsonConvert.DeserializeObject<SiteModel>(
                    Preferences.Get("CheckedInSite", "")
                );

                if (currentSiteData == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Current site information not found", "OK");
                    return;
                }

                // Get check-in time from preferences (as in Xamarin)
                var checkInTimeStr = Preferences.Get("ClockInTime", null);
                DateTime checkInTime = _checkInTime;
                if (!string.IsNullOrEmpty(checkInTimeStr) && DateTime.TryParse(checkInTimeStr, out var parsedTime))
                {
                    checkInTime = parsedTime;
                }

                var request = new TimesheetUpdateRequest
                {
                    EmployeeId = App.LoggedInUser.EmployeeId.ToString(),
                    SiteId = currentSiteData.SiteId.ToString(),
                    TimeOnSite = checkInTime,
                    TimeOffSite = checkoutTime,
                    IsNonAuthTimeOffSite = isNonAuthCheckOut,
                    CheckOutImage = null // Add image if needed
                };

                App.ShowLoader(); // Show loader as in Xamarin

                var response = await ApiHelper.CallApi(
                    HttpMethod.Post,
                    "TimeSheet/CreateUpdateTimeSheet",
                    JsonConvert.SerializeObject(request, new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    }),
                    true
                );

                if (!response.Status)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message ?? "Failed to update timesheet", "OK");
                    return;
                }

                // Reset UI state (as in Xamarin)
                IsCheckedIn = false;
                IsCheckedInViewVisible = false;
                IsNotCheckedInViewVisible = true;
                LoggedTime = new TimeSpan(0, 0, 0).ToString(@"hh\:mm\:ss");

                // Clear preferences (as in Xamarin)
                Preferences.Remove("ClockInTime");
                Preferences.Remove("CheckedInSite");

                // Refresh the timesheet (as in Xamarin)
                MessagingCenter.Send<string>("RefreshTimesheet", "RefreshTimesheet");

                // Show toast/alert as in Xamarin
                await Application.Current.MainPage.DisplayAlert("Success", "Time synced", "OK");

                ClockOutTime = checkoutTime.ToLocalTime().ToString("hh : mm tt");
                IsNonAuthTime = isNonAuthCheckOut;

                // Reset time properties
                ClockInTime = "--:--";
                ClockOutTime = "--:--";
                IsNonAuthTime = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                App.HideLoader(); // Hide loader as in Xamarin
            }
        }

        public async Task<SiteModel> GetCurrentSiteAsync(double latitude, double longitude)
        {
            var request = new
            {
                SiteMapRefLat = latitude.ToString(),
                SiteMapRefLong = longitude.ToString()
            };

            var response = await ApiHelper.CallApi(
                HttpMethod.Post,
                ConstantHelper.Constants.GeofenceSite,
                JsonConvert.SerializeObject(request),
                true
            );

            if (response.Status)
            {
                return JsonConvert.DeserializeObject<SiteModel>(response.Result.ToString());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message ?? "Unable to determine current site.", "OK");
                return null;
            }
        }
    }
} 