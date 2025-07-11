using CloudCheckInMaui.ConstantHelper;
using CloudCheckInMaui.Models;
using CloudCheckInMaui.Services.ApiService;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CloudCheckInMaui.ViewModels
{
    public class HolidayViewModel : BaseViewModel
    {
        private ObservableCollection<HolidayListResponse> _holidayRequests = new ObservableCollection<HolidayListResponse>();
        public ObservableCollection<HolidayListResponse> HolidayRequests
        {
            get => _holidayRequests;
            set => SetProperty(ref _holidayRequests, value);
        }

        private bool _isListRefreshing;
        public bool IsListRefreshing
        {
            get => _isListRefreshing;
            set => SetProperty(ref _isListRefreshing, value);
        }

        public HolidayViewModel()
        {
            HolidayRequests = new ObservableCollection<HolidayListResponse>();
        }

        public async void LoadHolidayRequests()
        {
            try
            {
                IsListRefreshing = true;
                var loginUser = App.LoggedInUser;
                if (loginUser == null)
                {
                    // Handle not logged in
                    return;
                }

                var _req = new UserTimeSheetRequest()
                {
                    Start = 0,
                    EmpId = loginUser.EmployeeId.ToString(),
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

                var response = await ApiHelper.CallApi(HttpMethod.Post, ConstantHelper.Constants.GetEmployeeHoliday, System.Text.Json.JsonSerializer.Serialize(_req), true);
                if (response.Status)
                {
                    var list = ApiHelper.ConvertResult<ObservableCollection<HolidayListResponse>>(response.Result);
                    foreach (var item in list)
            {
                        item.FromDateString = item.FromDate.ToLocalTime().ToString(Constants.DateFormat);
                        item.ToDateString = item.ToDate.ToLocalTime().ToString(Constants.DateFormat);
                        if (item.Approved)
                        {
                            item.LeaveStatus = "Approved"; // Replace with resource if needed
                            item.IsSiteManagerInfoVisible = true;
                            item.StatusButtonTextColor = ConstantHelper.Constants.White.ToString();
                            item.StatusButtonBorderColor = ConstantHelper.Constants.Green.ToString();
                            item.StatusButtonBackgroundColor = ConstantHelper.Constants.Green.ToString();
                }
                else
                {
                            if (!string.IsNullOrEmpty(item.ApprovedBy))
            {
                                item.LeaveStatus = "Rejected";
                                item.IsSiteManagerInfoVisible = true;
                                item.StatusButtonTextColor = ConstantHelper.Constants.White.ToString();
                                item.StatusButtonBorderColor = ConstantHelper.Constants.Red.ToString();
                                item.StatusButtonBackgroundColor = ConstantHelper.Constants.Red.ToString();
                }
                else
                {
                                item.LeaveStatus = "Pending";
                                item.IsSiteManagerInfoVisible = false;
                                item.StatusButtonTextColor = ConstantHelper.Constants.AuthorizedColor.ToString();
                                item.StatusButtonBorderColor = ConstantHelper.Constants.AuthorizedColor.ToString();
                                item.StatusButtonBackgroundColor = ConstantHelper.Constants.White.ToString();
                            }
                        }
                    }
                    HolidayRequests = list;
                }
                else if (!(response.Message?.ToLower() == "no records found"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsListRefreshing = false;
            }
        }
    }
} 