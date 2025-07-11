using CCIMIGRATION.ApiModels;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.ViewModels;
using Newtonsoft.Json;
// using Xamarin.Plugin.Calendar.Models; // REMOVED: Replace with Plugin.Maui.Calendar
using Plugin.Maui.Calendar.Models;
using System.Windows.Input;

namespace CCIMIGRATION.SiteManagerViewModel
{
    public class CalenderViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private int _month = DateTime.Now.Month;
        private int _year = DateTime.Now.Year;
        private string siteID;
        private string monthText;
        private DateTime selectedDate;
        
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); }
        }
        public string MonthText
        {
            get { return monthText; }
            set { monthText = value; OnPropertyChanged(); }
        }
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; OnPropertyChanged("SiteID"); }
        }
        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }
        public int Month
        {
            get { return _month; }
            set 
            { 
                _month = value; 
                OnPropertyChanged("Month"); 
            }
        }
        public EventCollection Events { get; set; }
        #endregion
        #region Commands
        public ICommand LoadNextMonthEvents
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (Month == 12)
                        {
                            Month = 1;
                            Year++;
                        }
                        else
                        {
                            Month++;
                        }
                        SelectedDate = new DateTime(Year, Month, 1);
                        UpdateMonth();
                        GetEvents();
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        public ICommand LoadLastMonthEvents
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (Month == 1)
                        {
                            Month = 12;
                            Year--;
                        }
                        else
                        {
                            Month--;
                        }
                        SelectedDate = new DateTime(Year, Month, 1);
                        UpdateMonth();
                        GetEvents();
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        #endregion
        #region Constructor
        public CalenderViewModel()
        {
            Events = new EventCollection();
            GetUser();
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
            SelectedDate = DateTime.Today.Date;
            UpdateMonth();
            //CheckGeofence();
        }
        public async void CheckGeofence()
        {
            try
            {
                var status = await CheckAndRequestPermissionAsync(new Permissions.LocationAlways());
                if (status != PermissionStatus.Granted)
                {
                    ShowToast("Permission error"); // TODO: Fix Resource.PermissionError
                    return;
                }
                var gpsStatus = DependencyService.Get<ILocationService>().IsGpsAvailable();
                if (gpsStatus)
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
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
                                    Preferences.Set(Constants.CurrentSite, JsonConvert.SerializeObject(site));
                                    //Preferences.Set(Constants.RecentSite, JsonConvert.SerializeObject(site));
                                    SiteID = site.SiteId.ToString();
                                    GetEvents();

                                }
                            }
                            else if (!result.Status)
                            {
                                GetLastSiteData();
                            }
                            else
                            {
                                ShowToast("Not on active site"); // TODO: Fix Resource.NotOnActiveSite
                            }
                        }
                        else
                        {
                            InternetError();
                        }
                    }
                    else
                    {
                        ShowToast("Error getting location"); // TODO: Fix Resource.ErrorGettingLocation
                    }
                }
                else
                {
                    ShowLocationSettingAlert();
                    return;
                }

            }
            catch (Exception ex)
            {
                ShowToast(ex.Message);
            }
            finally
            {
                HideLoader();
            }
        }
        private async void GetLastSiteData()
        {
            try
            {
                GetUser();
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.UserCheckInOnAnySite + "?empId=" + LoginUser.EmployeeId.ToString(), null, true);
                if (response.Status)
                {
                    var data = ApiHelper.ConvertResult<RecentSiteModel>(response.Result);
                    if (data != null)
                    {
                        SiteID = data.SiteId.ToString();
                        GetEvents();

                    }
                    else
                    {
                        ShowToast(ResourceConstants.CannotSeeHoliday);
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.CannotSeeHoliday);
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
                ShowToast(ex.Message);
            }
            
            

            //var data = Preferences.Get(Constants.RecentSite, "");
            //if (data != null)
            //{
            //    var SiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.RecentSite, ""));
            //    if (SiteData != null)
            //    {
            //        SiteID = SiteData.SiteId.ToString();
            //        GetEvents();
            //    }
            //    else
            //    {
            //        ShowToast(Resource.CannotSeeHoliday);
            //    }
            //}
            //else
            //{
            //    ShowToast(Resource.CannotSeeHoliday);
            //}
        }
        private void UpdateMonth()
        {
            switch (Month)
            {
                case 1:
                    MonthText = ResourceConstants.January;
                    break;
                case 2:
                    MonthText = ResourceConstants.February;
                    break;
                case 3:
                    MonthText = ResourceConstants.March;
                    break;
                case 4:
                    MonthText = ResourceConstants.April;
                    break;
                case 5:
                    MonthText = ResourceConstants.May;
                    break;
                case 6:
                    MonthText = ResourceConstants.June;
                    break;
                case 7:
                    MonthText = ResourceConstants.July;
                    break;
                case 8:
                    MonthText = ResourceConstants.August;
                    break;
                case 9:
                    MonthText = ResourceConstants.September;
                    break;
                case 10:
                    MonthText = ResourceConstants.October;
                    break;
                case 11:
                    MonthText = ResourceConstants.November;
                    break;
                case 12:
                    MonthText = ResourceConstants.December;
                    break;
            }
        }

        private async void GetEvents()
        {
            try
            {
                var url = "?managerId=" + LoginUser.EmployeeId + "&siteId=" + SiteID + "&month=" + Month + "&year=" + Year;
                ShowLoader("");
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.GetEmployeeHolidaysByMonth+url,null, true);
                if (response != null)
                {
                    if (response.Status)
                    {
                        var eventlist = ApiHelper.ConvertResult<List<EventResponse>>(response.Result);
                        foreach(var item in eventlist)
                        {
                            AddEventToCalender(item.Day, item.EmployeesOnLeave);
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.NoHolidayEventFound);
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.SomethingWentWrongTryAgain);
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

        private void AddEventToCalender(DateTime date, EmployeesOnLeave[] list)
        {
            var collection = new DayEventCollection<AdvancedEventModel>(Color.FromArgb(Constants.AppThemeColor), Color.FromArgb(Constants.White));
            foreach(var item in list)
            {
                collection.Add(new AdvancedEventModel
                {
                    Description = item.FirstName + " " + item.LastName + ResourceConstants.IsOnLeaveOn + date.ToString("dd MMM "),
                    ApprovedBy = item.ApprovedBy
                }); ;
            }
            Events.Add(date, collection);
            //Events.Add(date, new DayEventCollection<List<AdvancedEventModel>>(Color.FromHex(Constants.AppThemeColor), Color.FromHex(Constants.White))
            //{
            //   lst
            //});
        }
        
        #endregion
    }
}
