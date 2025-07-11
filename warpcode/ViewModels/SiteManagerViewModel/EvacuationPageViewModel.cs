using CCIMIGRATION.ApiModels;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Interface;
using CCIMIGRATION.Models;
using CCIMIGRATION.Models.SiteManagerModel;
using CCIMIGRATION.Service.ApiService;
using CCIMIGRATION.Services;
using CCIMIGRATION.SiteManagerViews;
using CCIMIGRATION.ViewModels;
using Newtonsoft.Json;
// using Plugin.Media; // REMOVED: Replace with Microsoft.Maui.Essentials.MediaPicker
// using Plugin.Media.Abstractions; // REMOVED: Replace with Microsoft.Maui.Essentials.MediaPicker
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCIMIGRATION.SiteManagerViewModel
{
    public class EvacuationPageViewModel:BaseViewModel
    {
        #region Private Properties
        private ObservableCollection<EvacuatedModel> evacuatedList;
        private ObservableCollection<EvacuationMessagesList> evacuationmessagelist;
        private ObservableCollection<EvacuationsList> evacuationsList = new ObservableCollection<EvacuationsList>();
        private bool _isListRefreshing;
        private string siteID;
        private string _alertMessage;
        private bool _isEvacuationMessagePopupVisible;
        private EvacuationMessagesList _selectedEvacuationMessage;
        private string _evacuationImage;
        private bool _isImageAvailable;
        private bool _isEvacuationDetailPopupVisible;
        private List<EvacuationEmployee> _selectedEvacuationEmlpoyees;
        #endregion


        #region Public Properties
        public List<EvacuationEmployee> SelectedEvacuationEmlpoyees
        {
            get { return _selectedEvacuationEmlpoyees; }
            set
            {
                _selectedEvacuationEmlpoyees = value;
                OnPropertyChanged();
            }
        }
        public bool IsEvacuationDetailPopupVisible
        {
            get { return _isEvacuationDetailPopupVisible; }
            set { _isEvacuationDetailPopupVisible = value; OnPropertyChanged(); }
        }
        public ObservableCollection<EvacuationsList> EvacuationsList
        {
            get { return evacuationsList; }
            set { evacuationsList = value; OnPropertyChanged(); }
        }

        public bool IsImageAvailable
        {
            get { return _isImageAvailable; }
            set { _isImageAvailable = value; OnPropertyChanged(); }
        }

        public string EvacuationImage
        {
            get { return _evacuationImage; }
            set { _evacuationImage = value; OnPropertyChanged(); }
        }
        public EvacuationMessagesList SelectedEvacuationMessage
        {
            get { return _selectedEvacuationMessage; }
            set 
            { 
                _selectedEvacuationMessage = value;
                OnPropertyChanged();
                if (SelectedEvacuationMessage != null)
                {
                    AlertMessage = SelectedEvacuationMessage.Description;
                }
            }
        }
        public List<string> Selectedusers;
        public bool IsEvacuationMessagePopupVisible
        {
            get { return _isEvacuationMessagePopupVisible; }
            set { _isEvacuationMessagePopupVisible = value; OnPropertyChanged(); }
        }

        public string AlertMessage
        {
            get { return _alertMessage; }
            set { _alertMessage = value; OnPropertyChanged(); }
        }
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; OnPropertyChanged("SiteID"); }
        }
        public ObservableCollection<EvacuationMessagesList> EvacuationMessagesList
        {
            get { return evacuationmessagelist; }
            set 
            { 
                evacuationmessagelist = value;
                OnPropertyChanged(); 
            }
        }
        
        public ObservableCollection<EvacuatedModel> EvacuatedUserList
        {
            get { return evacuatedList; }
            set { evacuatedList = value;OnPropertyChanged(); }
        }
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommands
        public ICommand HideDetailPopup
        {
            get
            {
                return new Command(() =>
                {
                    IsEvacuationDetailPopupVisible = false;
                });
            }
        }
        public ICommand ViewDetailsCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    var item = (EvacuationsList)obj;
                    
                    SelectedEvacuationEmlpoyees = item.EvacuationEmployees;
                    IsEvacuationDetailPopupVisible = true;
                });
            }
        }

        public ICommand CreateEvacuationCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.Current.MainPage.Navigation.PushAsync(new Evacuation() { BindingContext = this });
                    CheckGeofence();
                });
            }
        }
        public ICommand CancelEvacuaationCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsEvacuationMessagePopupVisible = false;
                });
            }
        }
        public ICommand HideEvacutionDetailPopupCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsEvacuationDetailPopupVisible = false;
                });
            }
        }
        public ICommand SendAlertButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        Selectedusers = new List<string>();
                        var list = EvacuatedUserList.Where(x => x.IsChecked).ToList();
                        if (list.Count > 0)
                        {
                            AlertMessage = String.Empty;
                            IsEvacuationMessagePopupVisible = true;
                            GetEvacuationMessages();
                            foreach (var item in list)
                            {
                                Selectedusers.Add(item.Id.ToString());
                            }
                        }
                        else
                        {
                            ShowToast(ResourceConstants.SelectUserForEvacuation);
                        }
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                    
                });
            }
        }
        public ICommand RefreshEvacuatedListCommand
        {
            get
            {
                return new Command(async() =>
                {
                    IsListRefreshing = false;
                    await LoadOnSiteStaff();
                });
            }
        }
        public ICommand RemovePhotoCommand
        {
            get
            {
                return new Command(() =>
                {
                    EvacuationImage = "";
                    IsImageAvailable = false;
                });
            }
        }
        public ICommand ClickPhotoCommand
        {
            get
            {
                return new Command(async() =>
                {
                    try
                    {
                        if (MediaHelper.IsCameraAvailable)
                        {
                            var photo = await MediaHelper.TakePhotoAsync(maxWidthHeight: 2000, compressionQuality: 20);
                            if (photo != null)
                            {
                                EvacuationImage = photo.FullPath;
                                IsImageAvailable = true;
                            }
                            else
                            {
                                ShowToast("Error in image capturing. Please recapture.");
                            }
                        }
                        else
                        {
                            ShowToast("Camera Not Available");
                        }
                       
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                   
                });
            }
        }
        public ICommand SendEvacuationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(AlertMessage) && !String.IsNullOrEmpty(EvacuationImage))
                        {
                            ShowLoader("");
                            var imagebase = Convert.ToBase64String(File.ReadAllBytes(EvacuationImage));
                            var _req = new CreateEvacuationRequestModel()
                            {
                                EmpId = Selectedusers.ToArray(),
                                Message = AlertMessage,
                                UserId = LoginUser.EmployeeId,
                                Image = Convert.ToBase64String(File.ReadAllBytes(EvacuationImage))
                            };
                            var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.SendEvacuationalert, JsonConvert.SerializeObject(_req), true);
                            if (response.Status)
                            {
                                IsEvacuationMessagePopupVisible = false;
                                foreach (var item in EvacuatedUserList)
                                {
                                    item.IsChecked = false;
                                }
                                ShowToast(ResourceConstants.EvacuationAlertSuccess);
                            }
                            else
                            {
                                ShowToast(response.Message);
                            }
                        }
                        else
                        {
                            //ShowToast(Resource.AddEvacutationMessage);
                            ShowToast("Please add evacuation image and message");
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        ShowToast(ex.Message);
                    }
                    finally
                    {
                        HideLoader();
                        //
                    }
                });
            }
        }

        #endregion

        #region CTor
        public EvacuationPageViewModel()
        {
            GetUser();
            EvacuationMessagesList = new ObservableCollection<EvacuationMessagesList>();
            //CheckGeofence();
            IsImageAvailable = false;
        }
        #endregion

        #region Methods
        public async void LoadEvacuationDetails(int id)
        {
            try
            {
                var _req = new StaffModel.GetStaffOnSiteModel()
                {
                    draw = 0,
                    empId = LoginUser.EmployeeId.ToString(),
                    length = 50,
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
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.GetEvacuationsListForEmployee, JsonConvert.SerializeObject(_req), true);
                if (response.Status)
                {
                    //EvacuationsList = ApiHelper.ConvertResult<ObservableCollection<EvacuationsList>>(response.Result);
                    IsEvacuationDetailPopupVisible = true;
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
            }
        }
        public async void GetEvacuationMessages()
        {
            try
            {
                ShowLoader("");
                
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.GetEvacuationMessages, null, true);
                if (response.Status)
                {
                    EvacuationMessagesList.Clear();
                    EvacuationMessagesList = ApiHelper.ConvertResult<ObservableCollection<EvacuationMessagesList>>(response.Result);
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
            }
        }
        public async void CheckGeofence()
        {
            try
            {
                var status = await CheckAndRequestPermissionAsync(new Permissions.LocationAlways());
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
            catch (Exception ex)
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
            //        await LoadOnSiteStaff();
            //    }
            //    else
            //    {
            //        ShowToast(Resource.CannotSeeEmployees);
            //    }
            //}
            //else
            //{
            //    ShowToast(Resource.CannotSeeEmployees);
            //}
        }
        public async Task LoadOnSiteStaff()
        {
            try
            {
                ShowLoader("");
                EvacuatedUserList = new ObservableCollection<EvacuatedModel>();
                EvacuatedUserList.Clear();
                var _req = new StaffModel.GetStaffOnSiteModel()
                {
                    draw = 0,
                    empId = LoginUser.EmployeeId.ToString(),
                    length = 50,
                    siteId = SiteID,
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
                    var staffList = new ObservableCollection<EvacuatedModel>();
                    foreach (var item in list)
                    {
                        staffList.Add(new EvacuatedModel
                        {
                            Name = item.FirstName + " " + item.LastName,
                            Id = item.EmployeeId.ToString(),
                            TradeName = item.Trade.ToString()
                        });

                    }
                    EvacuatedUserList = staffList;
                }
                else
                {
                    ShowToast(response.Message);
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
        public async Task LoadEvacuations()
        {
            try
            {
                ShowLoader("");
                EvacuationsList.Clear();
                var _req = new StaffModel.GetStaffOnSiteModel()
                {
                    draw = 0,
                    empId = LoginUser.EmployeeId.ToString(),
                    length = 50,
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
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.GetEvacuationsList, JsonConvert.SerializeObject(_req), true);
                if (response.Status)
                {
                    EvacuationsList = ApiHelper.ConvertResult<ObservableCollection<EvacuationsList>>(response.Result);
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
            }
        }
        #endregion
    }
}
