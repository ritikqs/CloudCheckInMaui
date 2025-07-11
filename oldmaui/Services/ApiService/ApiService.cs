using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using CloudCheckInMaui.Models;
using System.Diagnostics;

namespace CloudCheckInMaui.Services.ApiService
{
    public class ApiService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        #region Timesheet Operations
        public static async Task<List<TimesheetModel>> GetTimesheetList()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, ApiEndpoints.TimesheetList);
                return response.Status ? response.GetData<List<TimesheetModel>>() : new List<TimesheetModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting timesheet list: {ex.Message}");
                throw;
            }
        }

        public static async Task<bool> AddTimesheet(TimesheetModel timesheet)
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Post, ApiEndpoints.TimesheetAdd, 
                    JsonSerializer.Serialize(timesheet, _jsonOptions));
                return response.Status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding timesheet: {ex.Message}");
                throw;
            }
        }

        public static async Task<bool> UpdateTimesheet(TimesheetModel timesheet)
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Put, ApiEndpoints.TimesheetUpdate, 
                    JsonSerializer.Serialize(timesheet, _jsonOptions));
                return response.Status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating timesheet: {ex.Message}");
                throw;
            }
        }

        public static async Task<bool> DeleteTimesheet(int timesheetId)
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Delete, $"{ApiEndpoints.TimesheetDelete}/{timesheetId}");
                return response.Status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting timesheet: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Holiday Operations
        public static async Task<List<HolidayListResponse>> GetHolidayList()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, ApiEndpoints.HolidayList);
                return response.Status ? response.GetData<List<HolidayListResponse>>() : new List<HolidayListResponse>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting holiday list: {ex.Message}");
                throw;
            }
        }

        public static async Task<List<HolidayListResponse>> GetEmployeeHolidays(string employeeId)
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, $"{ApiEndpoints.EmployeeHolidays}/{employeeId}");
                return response.Status ? response.GetData<List<HolidayListResponse>>() : new List<HolidayListResponse>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting employee holidays: {ex.Message}");
                throw;
            }
        }

        public static async Task<bool> RequestHoliday(HolidayRequest request)
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Post, ApiEndpoints.HolidayRequest, 
                    JsonSerializer.Serialize(request, _jsonOptions));
                return response.Status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error requesting holiday: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Alert Operations
        public static async Task<List<EvacuationEmployeeListModel>> GetAlertsList()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, ApiEndpoints.AlertsList);
                return response.Status ? response.GetData<List<EvacuationEmployeeListModel>>() : new List<EvacuationEmployeeListModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting alerts list: {ex.Message}");
                throw;
            }
        }

        public static async Task<int> GetUnreadAlertCount(string employeeId)
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, $"{ApiEndpoints.AlertsUnread}/{employeeId}");
                return response.Status ? response.GetData<int>() : 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting unread alert count: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Message Operations
        public static async Task<List<Message>> GetMessagesList()
        {
            try
            {
                var response = await ApiHelper.CallApi(HttpMethod.Get, ApiEndpoints.MessagesList);
                return response.Status ? response.GetData<List<Message>>() : new List<Message>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting messages list: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
} 