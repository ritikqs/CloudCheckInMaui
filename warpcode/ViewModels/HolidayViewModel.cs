using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class HolidayViewModel : BaseViewModel
    {
        #region Private/Public Variables
        private ObservableCollection<HolidayListResponse> _holidayList;
        private bool _isListRefreshing;
        public bool IsListRefreshing
        {
            get { return _isListRefreshing; }
            set { _isListRefreshing = value; OnPropertyChanged(); }
        }

        public ObservableCollection<HolidayListResponse> HolidayList
        {
            get { return _holidayList; }
            set { _holidayList = value;OnPropertyChanged(); }
        }

        #endregion
        #region Constructor
        public HolidayViewModel()
        {
            HolidayList = new ObservableCollection<HolidayListResponse>();
        }
        #endregion
        #region Commands
        public ICommand RefreshHolidayListCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        LoadHolidayRequests();
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

        public async void LoadHolidayRequests()
        {
            try
            {
                IsListRefreshing = true;
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
                    foreach(var item in list)
                    {
                        item.FromDateString =(Convert.ToDateTime(item.FromDate).ToUniversalTime()).ToLocalTime().ToString(Constants.DateFormat);
                        item.ToDateString = (Convert.ToDateTime(item.ToDate).ToUniversalTime()).ToLocalTime().ToString(Constants.DateFormat);
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
                    HolidayList = list;
                }
                else if (!(response.Message.ToLower() == "no records found"))
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
                IsListRefreshing = false;
            }
        }

        #endregion

    }
}
