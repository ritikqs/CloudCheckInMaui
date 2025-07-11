using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class TimesheetPageViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private ObservableCollection<TimeSheetRequest> timesheetList;
        private bool _isListRefreshing;
        private bool _isNextPageLoading;
        private bool isSelected;
   
        public bool IsNextPageLoading
        {
            get { return _isNextPageLoading; }
            set { _isNextPageLoading = value; OnPropertyChanged("IsNextPageLoading"); }
        }
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value; OnPropertyChanged("IsListRefreshing"); }
        }
        public ObservableCollection<TimeSheetRequest> TimesheetList
        {
            get { return timesheetList; }
            set { timesheetList = value; OnPropertyChanged("TimesheetList"); }
        }
        #endregion
       public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(CurrentIcon);
              
            }
        }
        public string CurrentIcon
        {
            get => IsSelected ? "timesheet_active.png" : "timesheet.png";
        }
        #region Constructor
        public TimesheetPageViewModel()
        {
            TimesheetList = new ObservableCollection<TimeSheetRequest>();
            IsNextPageLoading = false;
            GetUser();
            IsSelected = true;
        }
        #endregion
        #region Commands


        public ICommand RefreshTimeSheetCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        IsListRefreshing = true;
                        TimesheetList.Clear();
                        GetUser();
                        GetTimeSheet();
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                });
            }
        }
        #endregion
        #region Methods
        public async void GetTimeSheet()
        {
            try
            {
                TimesheetList.Clear();
                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.EmployeeTimeSheet, JsonConvert.SerializeObject(CreateTimeSheetRequest()), true);
                if (response.Status)
                {
                    var list = ApiHelper.ConvertResult<List<SiteListModel>>(response.Result);
                    foreach (var item in list)
                    {
                        if (item.NonAutTimeOffSite?.ToUniversalTime() != null)
                        {
                            TimesheetList.Add(new TimeSheetRequest
                            {
                                SiteName = item.Site.SiteName,
                                OnSite_Time = item.TimeOnSite.ToLocalTime().ToString(Constants.TimeSheetTimeFormat),
                                OffSite_Time = item.NonAutTimeOffSite?.ToLocalTime().ToString(Constants.TimeSheetTimeFormat),
                                IsNonAuthTime = true,
                                Date = item.TimeOnSite.ToLocalTime().ToString(Constants.TimeSheetDateFormat)
                            });
                        }
                        else
                        {
                            TimesheetList.Add(new TimeSheetRequest
                            {
                                SiteName = item.Site.SiteName,
                                OnSite_Time = item.TimeOnSite.ToLocalTime().ToString(Constants.TimeSheetTimeFormat),
                                OffSite_Time = item.TimeOffSite?.ToLocalTime().ToString(Constants.TimeSheetTimeFormat),
                                IsNonAuthTime = false,
                                Date = item.TimeOnSite.ToLocalTime().ToString(Constants.TimeSheetDateFormat)
                            });
                        }
                        
                    }
                }
                else if(!(response.Message.ToLower() == "no records found"))
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
                IsNextPageLoading = false;
                HideLoader();
                IsListRefreshing = false;
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
        #endregion
    }
}
