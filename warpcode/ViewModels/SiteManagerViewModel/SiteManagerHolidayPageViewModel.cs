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
    public class SiteManagerHolidayPageViewModel:BaseViewModel
    {
        #region Private Properties
        private ObservableCollection<HolidayListResponse> holidayRequestList;
        private bool _isListRefreshing;
        private bool _isAppliedListRefreshing;
        private string siteID;
        private bool _isRequestSelected;
        private bool _isSentSelected;
        private ObservableCollection<HolidayListResponse> _holidayAppliedList;
        private bool _isNoData;

        public bool IsNoData
        {
            get { return _isNoData; }
            set { _isNoData = value;OnPropertyChanged(); }
        }


        #endregion
        private string myVar;

        public string MyProperty
        {
            get { return myVar; }
            set { myVar = value;OnPropertyChanged(); }
        }

        #region Public Properties
        public ObservableCollection<HolidayListResponse> HolidayAppliedList
        {
            get { return _holidayAppliedList; }
            set { _holidayAppliedList = value; OnPropertyChanged(); }
        }

        public bool IsSentSelected
        {
            get { return _isSentSelected; }
            set { _isSentSelected = value; OnPropertyChanged(); }
        }

        public bool IsRequestSelected
        {
            get { return _isRequestSelected; }
            set { _isRequestSelected = value; OnPropertyChanged(); }
        }
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; OnPropertyChanged("SiteID"); }
        }
        public ObservableCollection<HolidayListResponse> HolidayRequestList
        {
            get { return holidayRequestList; }
            set { holidayRequestList = value; OnPropertyChanged(); }
        }
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value; OnPropertyChanged(); }
        }
        
        public bool IsAppliedListRefreshing
        {
            get { return _isAppliedListRefreshing; }
            set { _isAppliedListRefreshing = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommands
        public ICommand LoadHolidayRequest
        {
            get
            {
                return new Command(() =>
                {
                    IsNoData = false;
                    IsRequestSelected = true;
                    IsSentSelected = false;
                    IsListRefreshing = true;
                    CheckGeofence();
                });
            }
        }
        public ICommand LoadAppliedHolidays
        {
            get
            {
                return new Command(() =>
                {
                    IsNoData = false;
                    IsRequestSelected = false;
                    IsSentSelected = true;
                    LoadAppliedHolidayList();
                });
            }
        }
        public ICommand ApproveHolidayCommand
        {
            get
            {
                return new Command(async(obj) =>
                {
                    try
                    {
                        var Item = HolidayRequestList[HolidayRequestList.IndexOf(obj as HolidayListResponse)];
                        var request = new ApproveRejectHolidayModel()
                        {
                            Approved = true,
                            ApprovedBy = LoginUser.FirstName + LoginUser.LastName,
                            Id = Item.Id
                        };
                        ShowLoader("");
                        var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.UpdateEmployeeHoliday, JsonConvert.SerializeObject(request), true);
                        if (response != null)
                        {
                            if (response.Status)
                            {
                                ShowToast(ResourceConstants.HolidayApprovedSuccess);
                                Item.IsApproveRejectButtonVisible = false;
                                Item.IsLeaveStatusVisible = true;
                                
                                Item.LeaveStatus = ResourceConstants.LeaveStatusApproved;
                                Item.StatusButtonTextColor = Constants.AuthorizedColor;
                                Item.StatusButtonBorderColor = Constants.AuthorizedColor;
                                Item.StatusButtonBackgroundColor = Constants.White;
                            }
                            else
                            {
                                ShowToast(response.Message);
                            }
                        }
                        else
                        {
                            ShowToast(ResourceConstants.SomethingWentWrongTryAgain);
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        ShowToast(ex.Message);
                    }
                    finally { HideLoader(); }
                    
                });
            }
        }
        public ICommand RejectHolidayCommand
        {
            get
            {
                return new Command(async(obj) =>
                {
                    try
                    {
                        var Item = HolidayRequestList[HolidayRequestList.IndexOf(obj as HolidayListResponse)];
                        var request = new ApproveRejectHolidayModel()
                        {
                            Approved = false,
                            ApprovedBy = LoginUser.FirstName + LoginUser.LastName,
                            Id = Item.Id
                        };
                        ShowLoader("");
                        var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.UpdateEmployeeHoliday, JsonConvert.SerializeObject(request), true);
                        if (response != null)
                        {
                            if (response.Status)
                            {
                                ShowToast(ResourceConstants.HolidayRejectSuccess);
                                Item.IsApproveRejectButtonVisible = false;
                                Item.IsLeaveStatusVisible = true;
                                Item.LeaveStatus = ResourceConstants.LeaveStatusRejected;

                                Item.StatusButtonTextColor = Constants.UnAuthorizedColor;
                                Item.StatusButtonBorderColor = Constants.UnAuthorizedColor;
                                Item.StatusButtonBackgroundColor = Constants.White;
                            }
                            else
                            {
                                ShowToast(response.Message);
                            }
                        }
                        else
                        {
                            ShowLoader(ResourceConstants.SomethingWentWrongTryAgain);
                        }

                    }
                    catch (Exception ex)
                    {
                        ShowToast(ex.Message);
                    }
                    finally { HideLoader(); }

                });
            }
        }
        public ICommand RefreshHolidayRequestCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsListRefreshing = false;
                    LoadHolidayRequestList();
                });
            }
        }
        public ICommand RefreshAppliedHolidayListCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsAppliedListRefreshing = false;
                    LoadAppliedHolidayList();
                });
            }
        }
        #endregion

        #region cTor
        public SiteManagerHolidayPageViewModel()
        {
            GetUser();
            HolidayRequestList = new ObservableCollection<HolidayListResponse>();
           // LoadHolidayRequest.Execute(null);
        }
        #endregion

        #region Mock Data
        public async void CheckLocationStatus()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status == PermissionStatus.Denied)
                {
                    ShowToast(ResourceConstants.PermissionError);
                    return;
                }
                else if (status != PermissionStatus.Granted)
                {
                    var request = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (request == PermissionStatus.Granted)
                    {
                        var isGpsOn = DependencyService.Get<ILocationService>().IsGpsAvailable();
                        if (isGpsOn)
                        {
                            LoadHolidayRequest.Execute(null);
                        }
                        else
                        {
                            ShowLocationSettingAlert();
                        }
                    }
                    else
                    {
                        ShowToast(ResourceConstants.PermissionError);
                        return;
                    }
                }
                else
                {
                    var isGpsOn = DependencyService.Get<ILocationService>().IsGpsAvailable();
                    if (isGpsOn)
                    {
                        LoadHolidayRequest.Execute(null);
                    }
                    else
                    {
                        ShowLocationSettingAlert();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {

            }
        }
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
                                    Preferences.Set(Constants.CurrentSite, JsonConvert.SerializeObject(site));
                                    //Preferences.Set(Constants.RecentSite, JsonConvert.SerializeObject(site));
                                    SiteID = site.SiteId.ToString();
                                    LoadHolidayRequestList();
                                    //Preferences.Set(Constants.RecentSite, JsonConvert.SerializeObject(site));
                                }
                            }
                            else
                            {
                                GetLastSiteData();
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
                        LoadHolidayRequestList();

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
                   
            //    }
            //    else
            //    {
            //        ShowToast(Resources.Resource,CannotSeeHoliday);
            //    }
            //}
            //else
            //{
            //    ShowToast(Resources.Resource,CannotSeeHoliday);
            //}
        }
        public async void LoadAppliedHolidayList()
        {
            try
            {
                IsAppliedListRefreshing = true;
                GetUser();
                var _req = new UserTimeSheetRequest()
                {
                    Start = 0,
                    EmpId = LoginUser.EmployeeId.ToString(),
                    Length = 20,
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
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.GetEmployeeHoliday, JsonConvert.SerializeObject(_req), true);
                if (response.Status)
                {
                    var list = ApiHelper.ConvertResult<ObservableCollection<HolidayListResponse>>(response.Result);
                    foreach (var item in list)
                    {
                        item.FromDateString = item.FromDate.ToLocalTime().ToString(Constants.DateFormat);
                        item.ToDateString = item.ToDate.ToLocalTime().ToString(Constants.DateFormat);
                        if (item.Approved)
                        {
                            item.LeaveStatus = ResourceConstants.LeaveStatusApproved;
                            item.IsSiteManagerInfoVisible = true;

                            item.StatusButtonTextColor = Constants.White;
                            item.StatusButtonBorderColor = Constants.Green;
                            item.StatusButtonBackgroundColor = Constants.Green;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(item.ApprovedBy))
                            {
                                item.LeaveStatus = ResourceConstants.LeaveStatusRejected;
                                item.IsSiteManagerInfoVisible = true;

                                item.StatusButtonTextColor = Constants.White;
                                item.StatusButtonBorderColor = Constants.Red;
                                item.StatusButtonBackgroundColor = Constants.Red;
                            }
                            else
                            {
                                item.LeaveStatus = ResourceConstants.LeaveStatusPending;
                                item.IsSiteManagerInfoVisible = false;

                                item.StatusButtonTextColor = Constants.AuthorizedColor;
                                item.StatusButtonBorderColor = Constants.AuthorizedColor;
                                item.StatusButtonBackgroundColor = Constants.White;
                            }
                        }
                    }
                    HolidayAppliedList = list;
                }
                else if (!(response.Message.ToLower() == "no records found"))
                {
                    IsNoData = true;
                    ShowToast(response.Message);
                }
                else
                {
                    IsNoData = true;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            finally
            {
                HideLoader();
                IsAppliedListRefreshing = false;
            }
        }
        public async void LoadHolidayRequestList()
        {
            try
            {
                //IsListRefreshing = true;
                GetUser();
                var _req = new SiteManagerHolidayListRequest()
                {
                    Start = 0,
                    EmpId = LoginUser.EmployeeId.ToString(),
                    Length = 20,
                    SiteId = SiteID,
                    Draw = 0,
                    Columns = new SiteManagerHolidayColumn[]
                    {
                            new SiteManagerHolidayColumn()
                            {
                                Data = "Id",
                                Name = "Id",
                                Searchable = true,
                                Orderable = false,
                                Search = new SiteManagerHolidaySearch()
                                {
                                    IsRegex = false,
                                    Value = ""
                                }
                            }
                    },
                    Order = new SiteManagerHolidayOrder[]
                    {
                        new SiteManagerHolidayOrder()
                        {
                            Column = 0,
                            Dir = "desc"
                        }

                    },
                    Search = new SiteManagerHolidaySearch()
                    {
                        Value = "",
                        IsRegex = false
                    }
                };
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.GetAllHolidayListBySiteManager, JsonConvert.SerializeObject(_req), true);
                if (response.Status)
                {
                    var list = ApiHelper.ConvertResult<ObservableCollection<HolidayListResponse>>(response.Result);
                    foreach (var item in list)
                    {
                        item.FromDateString = item.FromDate.ToString(Constants.DateFormat);
                        item.ToDateString = item.ToDate.ToString(Constants.DateFormat);
                        item.LeaveStatus = ResourceConstants.LeaveStatusRejected;
                        if (item.Approved)
                        {
                            item.IsApproveRejectButtonVisible = false;
                            item.IsLeaveStatusVisible = true;
                            item.LeaveStatus = ResourceConstants.LeaveStatusApproved;
                            item.StatusButtonTextColor = Constants.AuthorizedColor;
                            item.StatusButtonBorderColor = Constants.AuthorizedColor;
                            item.StatusButtonBackgroundColor = Constants.White;

                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(item.ApprovedBy))
                            {
                                item.IsApproveRejectButtonVisible = false;
                                item.IsLeaveStatusVisible = true;

                                item.StatusButtonTextColor = Constants.UnAuthorizedColor;
                                item.StatusButtonBorderColor = Constants.UnAuthorizedColor;
                                item.StatusButtonBackgroundColor = Constants.White;
                            }
                            else
                            {
                                item.IsApproveRejectButtonVisible = true;
                                item.IsLeaveStatusVisible = false;
                            }
                        }
                       
                    }
                    HolidayRequestList = list;

                }
                else if (!(response.Message.ToLower() == "no records found"))
                {
                    ShowToast(response.Message);
                    IsNoData = true;
                }
                else
                {
                    IsNoData = true;
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
            }
        }
        #endregion
    }
}
