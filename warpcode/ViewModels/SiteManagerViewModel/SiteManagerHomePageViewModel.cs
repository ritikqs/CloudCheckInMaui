using CCIMIGRATION.ApiModels;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.Models.SiteManagerModel;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.ViewModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCIMIGRATION.SiteManagerViewModel
{
    public class SiteManagerHomePageViewModel:BaseViewModel
    {
        #region Private/Public Variables
        private string siteID;
        private ObservableCollection<SiteStaffModel> siteStaffList;
        private bool _isListRefreshing;
        private string _date;
        private string _currentSite;
        private string _homePageTitleText;

        public string CurrentSite
        {
            get { return _currentSite; }
            set { _currentSite = value; OnPropertyChanged(Constants.CurrentSite); }
        } 
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; OnPropertyChanged("SiteID"); }
        }
        public string HomePageTitleText
        {
            get { return _homePageTitleText; }
            set { _homePageTitleText = value; OnPropertyChanged(); }
        }
        public string Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SiteStaffModel> SiteStaffList
        {
            get { return siteStaffList; }
            set { siteStaffList = value; OnPropertyChanged(); }
        }
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value; OnPropertyChanged(); }
        }
        #endregion
        #region ICommands
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    SiteStaffList.Clear();
                    IsListRefreshing = true;
                    CheckGeofence();
                    IsListRefreshing = false;
                });
            }
        }
        #endregion
        #region Ctor
        public SiteManagerHomePageViewModel()
        {
            GetUser();
            Date = DateTime.Now.ToString("dd MMM yyyy");
            HomePageTitleText = ResourceConstants.StaffOn + " " + CurrentSite;
            CheckGeofence();
        }

        #endregion
        #region Methods

        public async void CheckGeofence()
        {
            try
            {
                var status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
                if (status != PermissionStatus.Granted)
                {
                    ShowToast(ResourceConstants.PermissionError);
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
                                    CurrentSite = site.SiteName;
                                    HomePageTitleText = ResourceConstants.StaffOn + " "+ CurrentSite;
                                    await LoadOnSiteStaff();
                                }
                            }
                            else if (!result.Status)
                            {
                                GetLastSiteData();
                            }
                            else
                            {
                                ShowToast(ResourceConstants.NotOnActiveSite);
                            }
                        }
                        else
                        {
                            InternetError();
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.ErrorGettingLocation);
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
                IsListRefreshing = false;
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
                        SiteID = data.SiteId.ToString();
                        CurrentSite = data.SiteDetails.SiteName;
                        HomePageTitleText = ResourceConstants.StaffOn + " " + CurrentSite;
                        await LoadOnSiteStaff();

                    }
                    else
                    {
                        ShowToast(ResourceConstants.CannotSeeEmployees);
                    }
                }
                else
                {
                    ShowToast(ResourceConstants.CannotSeeEmployees);
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
                ShowToast(ex.Message);
            }
            finally
            {
                HideLoader();
                IsListRefreshing = false;
            }
           
            //var data = Preferences.Get(Constants.RecentSite, "");
            //if (data != null)
            //{
            //    var SiteData = JsonConvert.DeserializeObject<SiteModel>(Preferences.Get(Constants.RecentSite, ""));
            //    if (SiteData != null)
            //    {
            //        SiteID = SiteData.SiteId.ToString();
            //        CurrentSite = SiteData.SiteName;
            //        HomePageTitleText = ResourceConstants.StaffOn + " " + CurrentSite;
            //        await LoadOnSiteStaff();
            //    }
            //    else
            //    {
            //        ShowToast(ResourceConstants.CannotSeeEmployees);
            //    }
            //}
            //else
            //{
            //    ShowToast(ResourceConstants.CannotSeeEmployees);
            //}
        }

        private async Task LoadOnSiteStaff()
        {
            try
            {
                ShowLoader("");
                SiteStaffList = new ObservableCollection<SiteStaffModel>();
                SiteStaffList.Clear();
                var _req = new StaffModel.GetStaffOnSiteModel()
                {
                    draw = 0,
                    empId = LoginUser.EmployeeId.ToString(),
                    length = 20,
                    siteId=SiteID,
                    columns = new StaffModel.Column[]
                   {
                       new StaffModel.Column()
                       {
                           data = "Id",
                           name = "Id",
                           searchable = true,
                           orderable = false,
                           search = new StaffModel.Search()
                           {
                               isRegex = false,
                               value = ""
                           }
                       }
                   }.ToList(),
                    order = new StaffModel.Order[]
                   {
                        new StaffModel.Order()
                        {
                            column = 0,
                            dir = "desc"
                        }

                   }.ToList(),
                    search = new StaffModel.Search()
                    {
                        value = "",
                        isRegex = false
                    }
                };
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.GetAllStaffOnSite, JsonConvert.SerializeObject(_req), true);
                if (response.Status)
                {
                    var list = ApiHelper.ConvertResult<ObservableCollection<SiteStaffResponse>>(response.Result);
                    var staffList = new ObservableCollection<SiteStaffModel>();
                    foreach (var item in list)
                    {
                        staffList.Add(new SiteStaffModel
                        {
                            Name = item.FirstName + " " + item.LastName,
                            Trade = item.Trade,
                            StartDateTime = item.TimeOnSite.ToLocalTime().ToString(Constants.DateFormat),
                        });
                        
                    }
                    SiteStaffList = staffList;
                }
                else
                {
                    ShowToast(response.Message);
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {
                HideLoader();
                IsListRefreshing = false;
                //var data = new SiteStaffModel
                //{
                //    Name = "Pardeep",
                //    Trade = "Trade",
                //    StartDateTime = DateTime.Now.ToLocalTime().ToString("hh mm tt"),
                //    EndDateTime = DateTime.Now.ToLocalTime().ToString("hh mm tt")
                //};
                //SiteStaffList.Add(data);
            }
        }
        #endregion
       
    }
}
