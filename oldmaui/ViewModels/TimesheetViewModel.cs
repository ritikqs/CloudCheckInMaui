using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using CloudCheckInMaui.Services.FaceService;
using CloudCheckInMaui.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CloudCheckInMaui.ViewModels
{
    public class TimesheetViewModel : BaseViewModel, IIconChange
    {
        private readonly IFaceRecognitionService _faceRecognitionService;
        private bool _isInitialized;

        private ObservableCollection<SiteModel> _sitesList;
        public ObservableCollection<SiteModel> SitesList
        {
            get => _sitesList;
            set => SetProperty(ref _sitesList, value);
        }

        private ObservableCollection<TimeSheetRequest> _timesheetList;
        private bool _isListRefreshing;
        private bool _isNextPageLoading;
        private bool _isSelected;

        public bool IsNextPageLoading
        {
            get => _isNextPageLoading;
            set => SetProperty(ref _isNextPageLoading, value);
        }

        public bool IsListRefreshing
        {
            get => _isListRefreshing;
            set => SetProperty(ref _isListRefreshing, value);
        }

        public ObservableCollection<TimeSheetRequest> TimesheetList
        {
            get => _timesheetList;
            set => SetProperty(ref _timesheetList, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                OnPropertyChanged(nameof(CurrentIcon));
            }
        }

        public string CurrentIcon => IsSelected ? "timesheet_active.png" : "timesheet.png";

        private SiteModel _selectedSite;
        public SiteModel SelectedSite
        {
            get => _selectedSite;
            set => SetProperty(ref _selectedSite, value);
        }

        private bool _isCheckedIn;
        public bool IsCheckedIn
        {
            get => _isCheckedIn;
            set => SetProperty(ref _isCheckedIn, value);
        }

        private DateTime _checkInTime;
        public DateTime CheckInTime
        {
            get => _checkInTime;
            set => SetProperty(ref _checkInTime, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _weeklyHours;
        public string WeeklyHours
        {
            get => _weeklyHours;
            set => SetProperty(ref _weeklyHours, value);
        }

        private byte[] _capturedPhotoBytes;
        private string _temporaryPhotoPath;

        private const string OfflineTimesheetsKey = "OfflineTimesheets";

        public ICommand CheckInCommand { get; }
        public ICommand CheckOutCommand { get; }
        public ICommand LoadSitesCommand { get; }
        public ICommand LoadTimesheetCommand { get; }
        public ICommand CapturePhotoCommand { get; }
        public ICommand GetWeeklyHoursCommand { get; }
        public ICommand RefreshTimeSheetCommand { get; }
        public ICommand SyncOfflineTimeSheetCommand { get; }

        public TimesheetViewModel(IFaceRecognitionService faceRecognitionService)
        {
            _faceRecognitionService = faceRecognitionService;
            
            TimesheetList = new ObservableCollection<TimeSheetRequest>();
            SitesList = new ObservableCollection<SiteModel>();
            
            LoadSitesCommand = new Command(async () => await LoadSites());
            LoadTimesheetCommand = new Command(async () => await LoadTimesheet());
            CheckInCommand = new Command(async () => await CheckIn());
            CheckOutCommand = new Command(async () => await CheckOut());
            CapturePhotoCommand = new Command(async () => await CapturePhoto());
            GetWeeklyHoursCommand = new Command(async () => await GetWeeklyHours());
            RefreshTimeSheetCommand = new Command(async () => await RefreshTimesheet());
            SyncOfflineTimeSheetCommand = new Command(async () => await SyncOfflineTimeSheetAsync());

            // Subscribe to refresh message
            MessagingCenter.Subscribe<string>(this, "RefreshTimesheet", async (sender) =>
            {
                await Task.WhenAll(LoadTimesheet(), GetWeeklyHours());
            });
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;
            
            // Fetch all independent data in parallel for speed
            var sitesTask = LoadSites();
            var statusTask = CheckCurrentStatus();
            var timesheetTask = LoadTimesheet();
            var weeklyHoursTask = GetWeeklyHours();
            await Task.WhenAll(sitesTask, statusTask, timesheetTask, weeklyHoursTask);
            
            _isInitialized = true;
        }

        private async Task CheckCurrentStatus()
        {
            if (App.LoggedInUser?.EmployeeId == null) return;

            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, $"timesheet/checkinstatus/{App.LoggedInUser.EmployeeId}");
                
                if (response?.Status == true)
                {
                    var checkInStatus = ApiHelper.ConvertResult<CheckInStatusResponse>(response.Result);
                    if (checkInStatus != null)
                    {
                        IsCheckedIn = checkInStatus.IsCheckIn;
                        
                        if (IsCheckedIn)
                        {
                            CheckInTime = checkInStatus.CheckInTime;
                            
                            // Find the site
                            var siteId = checkInStatus.SiteId;
                            var site = SitesList?.FirstOrDefault(s => s.SiteId == siteId);
                            if (site != null)
                            {
                                SelectedSite = site;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CheckCurrentStatus: {ex.Message}");
            }
        }

        private async Task LoadSites()
        {
            try
            {
                SitesList.Clear();
                
                var response = await ApiHelper.CallApi(HttpMethod.Get, "timesheet/sites");
                
                if (response?.Status == true)
                {
                    var sites = ApiHelper.ConvertResult<SiteModel[]>(response.Result);
                    if (sites != null)
                    {
                        foreach (var site in sites)
                        {
                            SitesList.Add(site);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadSites: {ex.Message}");
                await DisplayAlert("Error", "Failed to load sites. Please try again.", "OK");
            }
        }

        private async Task LoadTimesheet()
        {
            try
            {
                IsLoading = true;
                TimesheetList.Clear();

                var request = CreateTimeSheetRequest();
                if (request == null) return;

                var response = await ApiHelper.CallApi(
                    HttpMethod.Post, 
                    "timesheet/list", 
                    JsonConvert.SerializeObject(request, new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver() 
                    })
                );

                if (response?.Status == true)
                {
                    var timesheetData = ApiHelper.ConvertResult<TimesheetResponse>(response.Result);
                    if (timesheetData?.Site != null)
                    {
                        var timesheet = timesheetData.Site;
                        var entry = new TimeSheetRequest
                        {
                            SiteName = timesheet.SiteName ?? "Unknown Site",
                            Date = DateTime.Now.ToShortDateString(),
                            OnSite_Time = "-",
                            OffSite_Time = "-",
                            NonAuthOffSiteTime = "-",
                            IsNonAuthTime = false
                        };

                        TimesheetList.Add(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadTimesheet: {ex.Message}");
                await DisplayAlert("Error", "Failed to load timesheet. Please try again.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CapturePhoto()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take Photo"
                });

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    _capturedPhotoBytes = memoryStream.ToArray();
                    _temporaryPhotoPath = photo.FullPath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task CheckIn()
        {
            if (SelectedSite == null)
            {
                await DisplayAlert("Error", "Please select a site", "OK");
                return;
            }

            if (_capturedPhotoBytes == null)
            {
                await DisplayAlert("Error", "Please take a photo first", "OK");
                await CapturePhoto();
                return;
            }

            try
            {
                IsLoading = true;

                // Verify face
                using var photoStream = new MemoryStream(_capturedPhotoBytes);
                var isFaceVerified = await _faceRecognitionService.IsFaceIdentified(App.LoggedInUser.Email, photoStream);
                if (!isFaceVerified)
                {
                    await DisplayAlert("Error", "Face verification failed", "OK");
                    return;
                }

                var request = new CheckInRequest
                {
                    EmpId = App.LoggedInUser.EmployeeId,
                    SiteId = SelectedSite.SiteId,
                    Image = Convert.ToBase64String(_capturedPhotoBytes)
                };

                var response = await ApiHelper.CallApi(
                    HttpMethod.Post, 
                    "timesheet/checkin", 
                    JsonConvert.SerializeObject(request, new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver() 
                    })
                );
                
                if (response.Status)
                {
                    await DisplayAlert("Success", "Checked in successfully", "OK");
                    IsCheckedIn = true;
                    CheckInTime = DateTime.Now;
                    
                    // Clear captured photo
                    _capturedPhotoBytes = null;
                    if (File.Exists(_temporaryPhotoPath))
                    {
                        File.Delete(_temporaryPhotoPath);
                    }
                    
                    // Refresh timesheet
                    await LoadTimesheet();
                    await GetWeeklyHours();
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
                IsLoading = false;
            }
        }

        private async Task CheckOut()
        {
            if (!IsCheckedIn)
            {
                await DisplayAlert("Error", "You are not checked in", "OK");
                return;
            }

            if (_capturedPhotoBytes == null)
            {
                await DisplayAlert("Error", "Please take a photo first", "OK");
                await CapturePhoto();
                return;
            }

            try
            {
                IsLoading = true;

                // Verify face
                using var photoStream = new MemoryStream(_capturedPhotoBytes);
                var isFaceVerified = await _faceRecognitionService.IsFaceIdentified(App.LoggedInUser.Email, photoStream);
                if (!isFaceVerified)
                {
                    await DisplayAlert("Error", "Face verification failed", "OK");
                    return;
                }

                var request = new CheckOutRequest
                {
                    EmpId = App.LoggedInUser.EmployeeId,
                    Image = Convert.ToBase64String(_capturedPhotoBytes)
                };

                var response = await ApiHelper.CallApi(
                    HttpMethod.Post, 
                    "timesheet/checkout", 
                    JsonConvert.SerializeObject(request, new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver() 
                    })
                );
                
                if (response.Status)
                {
                    await DisplayAlert("Success", "Checked out successfully", "OK");
                    IsCheckedIn = false;
                    CheckInTime = DateTime.MinValue;
                    SelectedSite = null;
                    
                    // Clear captured photo
                    _capturedPhotoBytes = null;
                    if (File.Exists(_temporaryPhotoPath))
                    {
                        File.Delete(_temporaryPhotoPath);
                    }
                    
                    // Refresh timesheet
                    await LoadTimesheet();
                    await GetWeeklyHours();
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
                IsLoading = false;
            }
        }

        private async Task GetWeeklyHours()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, $"timesheet/weeklyhours/{App.LoggedInUser.EmployeeId}");
                
                if (response.Status)
                {
                    var weeklyHoursData = ApiHelper.ConvertResult<WeeklyHoursResponse>(response.Result);
                    WeeklyHours = weeklyHoursData?.TotalHours ?? "0";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task RefreshTimesheet()
        {
            IsListRefreshing = true;
            await GetTimeSheet();
        }

        public async Task GetTimeSheet()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                IsNextPageLoading = true;

                if (!await ValidateAndRefreshToken())
                {
                    return;
                }

                TimesheetList.Clear();

                var request = CreateTimeSheetRequest();
                if (request == null) return;

                var response = await ApiHelper.CallApi(
                    HttpMethod.Post, 
                    "TimeSheet/GetAllTimeSheetByEmployee",
                    JsonConvert.SerializeObject(request, new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented
                    }), 
                    true
                );

                if (response?.Status == true && response.Result != null)
                {
                    var list = ApiHelper.ConvertResult<List<SiteListModel>>(response.Result);
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            if (item?.Site == null) continue;

                            var entry = new TimeSheetRequest
                            {
                                SiteName = item.Site.SiteName ?? "Unknown Site",
                                OnSite_Time = item.TimeOnSite.ToLocalTime().ToString(ConstantHelper.Constants.TimeSheetTimeFormat),
                                OffSite_Time = item.TimeOffSite?.ToLocalTime().ToString(ConstantHelper.Constants.TimeSheetTimeFormat),
                                NonAuthOffSiteTime = item.NonAutTimeOffSite?.ToLocalTime().ToString(ConstantHelper.Constants.TimeSheetTimeFormat),
                                IsNonAuthTime = item.NonAutTimeOffSite != null,
                                Date = item.TimeOnSite.ToLocalTime().ToString(ConstantHelper.Constants.TimeSheetDateFormat)
                            };
                            TimesheetList.Add(entry);
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", response?.Message ?? "Failed to load timesheet data", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTimeSheet: {ex.Message}");
                await DisplayAlert("Error", "Failed to load timesheet. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
                IsNextPageLoading = false;
                IsListRefreshing = false;
            }
        }

        private async Task<bool> ValidateAndRefreshToken()
        {
            try
            {
                // First try to get token from App.LoggedInUser
                if (string.IsNullOrWhiteSpace(App.Token) && App.LoggedInUser?.Token != null)
                {
                    App.Token = App.LoggedInUser.Token;
                }

                // If still no token, try to get from preferences
                if (string.IsNullOrWhiteSpace(App.Token))
                {
                    if (Preferences.Default.ContainsKey(ConstantHelper.Constants.UserData))
                    {
                        var userData = Preferences.Default.Get(ConstantHelper.Constants.UserData, "");
                        if (!string.IsNullOrEmpty(userData))
                        {
                            var user = JsonConvert.DeserializeObject<User>(userData);
                            if (user?.Token != null)
                            {
                                App.Token = user.Token;
                                App.LoggedInUser = user;
                            }
                        }
                    }
                }

                // If still no token, redirect to login
                if (string.IsNullOrWhiteSpace(App.Token))
                {
                    await DisplayAlert("Session Expired", "Please log in again to continue.", "OK");
                    await Shell.Current.GoToAsync("//Login");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ValidateAndRefreshToken: {ex.Message}");
                return false;
            }
        }

        private async Task HandleApiError(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (message.Contains("401") || message.Contains("Unauthorized") || message.Contains("token"))
            {
                App.Token = null;
                if (Preferences.Default.ContainsKey(ConstantHelper.Constants.UserData))
                {
                    Preferences.Default.Remove(ConstantHelper.Constants.UserData);
                }
                await DisplayAlert("Session Expired", "Your session has expired. Please log in again.", "OK");
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                await DisplayAlert("Error", message, "OK");
            }
        }

        private UserTimeSheetRequest CreateTimeSheetRequest()
        {
            if (App.LoggedInUser?.EmployeeId == null) return null;

            return new UserTimeSheetRequest
            {
                Start = 0,
                EmpId = App.LoggedInUser.EmployeeId.ToString(),
                Length = 50,
                Draw = 0,
                Columns = new Column[]
                {
                    new Column
                    {
                        Data = "Id",
                        Name = "Id",
                        Searchable = true,
                        Orderable = false,
                        Search = new Search
                        {
                            IsRegex = false,
                            Value = ""
                        }
                    }
                },
                Order = new Order[]
                {
                    new Order
                    {
                        Column = 0,
                        Dir = "desc"
                    }
                },
                Search = new Search
                {
                    Value = "",
                    IsRegex = false
                }
            };
        }

        public async Task SyncOfflineTimeSheetAsync()
        {
            // 1. Get offline timesheet data (replace with your actual storage logic)
            var offlineTimesheets = await GetOfflineTimesheetsAsync(); // Implement this
            if (offlineTimesheets == null || !offlineTimesheets.Any())
                return;
            var request = new { Timesheets = offlineTimesheets }; // Adjust to your API contract
            var response = await ApiHelper.CallApi(
                HttpMethod.Post,
                ConstantHelper.Constants.SyncOfflineTimeSheet,
                JsonConvert.SerializeObject(request),
                true
            );
            if (response.Status)
            {
                // Clear offline data if sync succeeded
                await ClearOfflineTimesheetsAsync(); // Implement this
                await Application.Current.MainPage.DisplayAlert("Success", "Offline timesheets synced.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "OK");
            }
        }

        private async Task<List<TimeSheetRequest>> GetOfflineTimesheetsAsync()
        {
            if (!Preferences.Default.ContainsKey(OfflineTimesheetsKey))
                return new List<TimeSheetRequest>();
            var json = Preferences.Default.Get(OfflineTimesheetsKey, "");
            if (string.IsNullOrEmpty(json))
                return new List<TimeSheetRequest>();
            try
            {
                return JsonConvert.DeserializeObject<List<TimeSheetRequest>>(json) ?? new List<TimeSheetRequest>();
            }
            catch
            {
                return new List<TimeSheetRequest>();
            }
        }

        private async Task ClearOfflineTimesheetsAsync()
        {
            Preferences.Default.Remove(OfflineTimesheetsKey);
        }

        public async Task AddOfflineTimesheetAsync(TimeSheetRequest timesheet)
        {
            var list = await GetOfflineTimesheetsAsync();
            list.Add(timesheet);
            var json = JsonConvert.SerializeObject(list);
            Preferences.Default.Set(OfflineTimesheetsKey, json);
        }
    }
} 