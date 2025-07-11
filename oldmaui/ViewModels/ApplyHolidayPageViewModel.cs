using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using Microsoft.Maui.Controls;
using CloudCheckInMaui.Resources;
using CloudCheckInMaui.Services.ApiService;
using Newtonsoft.Json;
using CloudCheckInMaui;

namespace CloudCheckInMaui.ViewModels
{
    public class ApplyHolidayPageViewModel : BaseViewModel
    {
        #region Public/Private Properties
        private DateTime _startDate = DateTime.Now.Date;
        private DateTime _minimumDate;
        private DateTime _endDate = DateTime.Now.Date;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private bool _isHolidaySuccessAppliedViewVisible;
        private bool _isStartDatePickerVisible;
        private bool _isEndDatePickerVisible;
        private bool _isStartTimePickerVisible;
        private bool _isEndTimePickerVisible;
        private bool _canApplyHoliday = true;

        public bool IsHolidaySuccessAppliedViewVisible
        {
            get => _isHolidaySuccessAppliedViewVisible;
            set { _isHolidaySuccessAppliedViewVisible = value; OnPropertyChanged(nameof(IsHolidaySuccessAppliedViewVisible)); }
        }
        
        public DateTime MinimumDate
        {
            get => _minimumDate;
            set
            {
                _minimumDate = value;
                OnPropertyChanged(nameof(MinimumDate));
            }
        }
        
        public DateTime StartDate
        {
            get => _startDate;
            set 
            {
                if (_startDate != value)
            { 
                _startDate = value;
                OnPropertyChanged(nameof(StartDate)); 
                    IsStartDatePickerVisible = false;
                }
            }
        }
        
        public DateTime EndDate
        {
            get => _endDate;
            set 
            {
                if (_endDate != value)
            { 
                _endDate = value; 
                OnPropertyChanged(nameof(EndDate));
                    IsEndDatePickerVisible = false;
                }
            }
        }
        
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                    IsStartTimePickerVisible = false;
                }
            }
        }
        
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                    IsEndTimePickerVisible = false;
                }
            }
        }

        public bool IsStartDatePickerVisible
        {
            get => _isStartDatePickerVisible;
            set { _isStartDatePickerVisible = value; OnPropertyChanged(nameof(IsStartDatePickerVisible)); }
        }

        public bool IsEndDatePickerVisible
        {
            get => _isEndDatePickerVisible;
            set { _isEndDatePickerVisible = value; OnPropertyChanged(nameof(IsEndDatePickerVisible)); }
        }

        public bool IsStartTimePickerVisible
        {
            get => _isStartTimePickerVisible;
            set { _isStartTimePickerVisible = value; OnPropertyChanged(nameof(IsStartTimePickerVisible)); }
        }

        public bool IsEndTimePickerVisible
        {
            get => _isEndTimePickerVisible;
            set { _isEndTimePickerVisible = value; OnPropertyChanged(nameof(IsEndTimePickerVisible)); }
        }

        //public bool CanApplyHoliday
        //{
        //    get => _canApplyHoliday;
        //    set { _canApplyHoliday = value; OnPropertyChanged(nameof(CanApplyHoliday)); }
        //}
        #endregion
        
        bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(CurrentIcon));
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
                return new Command(async () =>
                {
                    ResetForm();
                    await Shell.Current.GoToAsync("//Home");
                });
            }
        }
        
        public ICommand ApplyButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.CanApplyHoliday)
                        {
                            if (ValidateInputs())
                            {
                                var response = await ApiHelper.CallApi(HttpMethod.Post,Constants.CreateHolidayRequest,JsonConvert.SerializeObject(CreateHolidayRequest()),true);
                                if (response != null)
                                {
                                    if (response.Status)
                                    {
                                    IsHolidaySuccessAppliedViewVisible = true;
                                        Microsoft.Maui.Controls.MessagingCenter.Send(this, "RefreshHolidayList");
                                    }
                                    else
                                    {
                                        await Application.Current.MainPage.DisplayAlert(AppTranslater.Translate("Error"), response.Message, AppTranslater.Translate("OK"));
                                    }
                                }
                                else
                                {
                                    await Application.Current.MainPage.DisplayAlert(AppTranslater.Translate("Error"), AppTranslater.Translate("SomethingwentWrong"), AppTranslater.Translate("OK"));
                                }
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(AppTranslater.Translate("Error"), AppTranslater.Translate("HolidayDateTimeValidatonError"), AppTranslater.Translate("OK"));
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(AppTranslater.Translate("Error"), AppTranslater.Translate("CannotApplyHoliday"), AppTranslater.Translate("OK"));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        App.HideLoader();
                    }
                });
            }
        }
        
        public ICommand BackCommand => new Command(async () => await Shell.Current.GoToAsync(".."));

        public ICommand ShowStartDatePickerCommand => new Command(() => { IsStartDatePickerVisible = true; });
        public ICommand ShowEndDatePickerCommand => new Command(() => { IsEndDatePickerVisible = true; });
        public ICommand ShowStartTimePickerCommand => new Command(() => { IsStartTimePickerVisible = true; });
        public ICommand ShowEndTimePickerCommand => new Command(() => { IsEndTimePickerVisible = true; });
        #endregion
        
        #region Constructor
        public ApplyHolidayPageViewModel()
        {
            SetDefaultDates();
            GetUser();
            IsHolidaySuccessAppliedViewVisible = false;

        }
        #endregion

        #region Methods
        public void SetDefaultDates()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            MinimumDate = DateTime.Now.Date;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await CheckCanApplyHoliday();
            });
        }
        
        private bool ValidateInputs()
        {
            var Start = Convert.ToDateTime(StartDate) + StartTime;
            var end = Convert.ToDateTime(EndDate) + EndTime;
            return Start < end;
        }
        
        private void ResetForm()
        {
            SetDefaultDates();
            IsStartDatePickerVisible = false;
            IsEndDatePickerVisible = false;
            IsStartTimePickerVisible = false;
            IsEndTimePickerVisible = false;
            IsHolidaySuccessAppliedViewVisible = false;
        }

        public async Task CheckCanApplyHoliday()
        {
            try
            {
                GetUser();
                var empId = App.LoggedInUser?.EmployeeId ?? Guid.Empty;
                Debug.WriteLine($"Checking holiday eligibility for empId: {empId}");
                var response = await ApiHelper.CallApi(HttpMethod.Get, Constants.UserCheckInOnAnySite + "?empId=" + LoginUser.EmployeeId.ToString(), null, true);
                Debug.WriteLine($"API Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
                if (response.Status)
                {
                    var data = ApiHelper.ConvertResult<RecentSiteModel>(response.Result);
                    if (data != null) App.CanApplyHoliday = data.IsCheckIn;
                    Debug.WriteLine($"CanApplyHoliday set to: {App.CanApplyHoliday}");
                }
                else
                {
                    App.CanApplyHoliday = false;
                    Debug.WriteLine("API response was null or status was false.");
                }
            }
            catch (Exception ex)
            {
                App.CanApplyHoliday = false;
                Debug.WriteLine($"Exception in CheckCanApplyHoliday: {ex.Message}");
            }
        }

        private object CreateHolidayRequest()
        {
            var empId = App.LoggedInUser?.EmployeeId ?? Guid.Empty;
            return new
            {
                EmpId = empId.ToString(),
                FromDate = StartDate.Date + StartTime,
                ToDate = EndDate.Date + EndTime,
                DaysRequired = (EndDate.Date + EndTime - (StartDate.Date + StartTime)).TotalDays + 1
            };
        }
        #endregion
    }
} 