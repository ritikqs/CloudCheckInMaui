using CCIMIGRATION.ApiModels;
using CCIMIGRATION.ConstantHelper;
using CCIMIGRATION.Models;
using CCIMIGRATION.Service.ApiService;
using Newtonsoft.Json;
using System.Windows.Input;

namespace CCIMIGRATION.ViewModels
{
    public class ApplyHolidayPageViewModel : BaseViewModel
    {
        #region Public/Private Properties
        private DateTime _startDate= DateTime.Now.Date;
        private DateTime _minimumDate;
        private DateTime _endDate= DateTime.Now.Date;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private bool _isHolidaySuccessAppliedViewVisible;

        public bool IsHolidaySuccessAppliedViewVisible
        {
            get { return _isHolidaySuccessAppliedViewVisible; }
            set { _isHolidaySuccessAppliedViewVisible = value;OnPropertyChanged("IsHolidaySuccessAppliedViewVisible"); }
        }
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set
            {
                _minimumDate = value;
                OnPropertyChanged("MinimumDate");

            }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set 
            { 
                _startDate = value;
                OnPropertyChanged("StartDate"); 

            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set 
            { 
                _endDate = value; 
                OnPropertyChanged("EndDate");
            }
        }
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged("StartTime"); }
        }
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged("EndTime"); }
        }
        #endregion
        bool isSelected;
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
            get => IsSelected ? "alert_active.png" : "alert.png";
        }
        #region Commands

        public ICommand SafetySuccessDoneButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.Current.MainPage.Navigation.PopAsync();
                });
            }
        }
        public ICommand ApplyButtonCommand
        {
            get
            {
                return new Command(async() =>
                {
                    try
                    {
                        if (App.CanApplyHoliday)
                        {
                            if (ValidateInputs())
                            {
                                var response = await ApiHelper.CallApi(HttpMethod.Post, Constants.CreateHolidayRequest, JsonConvert.SerializeObject(CreateHolidayRequest()), true);
                                if (response != null)
                                {
                                    if (response.Status)
                                    {
                                        IsHolidaySuccessAppliedViewVisible = true;
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
                                ShowToast(ResourceConstants.HolidayDateTimeValidationError);
                            }
                        }
                        else
                        {
                            ShowToast(ResourceConstants.CannotApplyHoliday);
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
        
        #endregion
        #region Constructor
        public ApplyHolidayPageViewModel()
        {
            GetUser();
            //MinimumDate = DateTime.Now.Date;
            
        }
       
        #endregion
        #region Methods
        public async Task CheckCanApplyHoliday()
        {
            try
            {
                GetUser();
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.UserCheckInOnAnySite + "?empId="+LoginUser.EmployeeId.ToString(), null, true);
                if (response.Status)
                {
                    var data = ApiHelper.ConvertResult<RecentSiteModel>(response.Result);
                    if (data != null)
                    {
                        App.CanApplyHoliday = data.IsCheckIn;
                    }
                    else
                    {
                        App.CanApplyHoliday = false;
                    }
                }
                else
                {
                    App.CanApplyHoliday = false;
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
        public void SetDefaultDates()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                await CheckCanApplyHoliday();
            });
        }
        private ApplyHolidayRequest CreateHolidayRequest()
        {
            var data = new ApplyHolidayRequest()
            {
                EmpId = LoginUser.EmployeeId,
                FromDate = Convert.ToDateTime((StartDate) + StartTime),
                ToDate = Convert.ToDateTime((EndDate) + EndTime),
                NoDaysRequired = Convert.ToDouble((Convert.ToDateTime(EndDate) + EndTime - (Convert.ToDateTime(StartDate) + StartTime)).TotalDays)
            };
            return data;
        }
        private bool ValidateInputs()
        {
            var Start = Convert.ToDateTime(StartDate) + StartTime;
            var end = Convert.ToDateTime(EndDate) + EndTime;
            if (Start < end)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
