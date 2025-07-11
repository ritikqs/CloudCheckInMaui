namespace CloudCheckInMaui.Services.ApiService
{
    public static class ApiEndpoints
    {
        // Authentication
        public const string Login = "account/login";
        public const string Logout = "account/logout";
        public const string Register = "account/register";
        public const string ForgotPassword = "account/forgotpassword";
        public const string ResetPassword = "account/resetpassword";
        public const string ValidateRefreshToken = "account/validaterefreshtoken";
        public const string CheckEmail = "account/checkemail";

        // Employee
        public const string EmployeeImageStatus = "employee/imagestatus";
        public const string RetakeEmployeeImage = "employee/retakeimage";
        public const string UpdateEmployeeImage = "employee/updateimage";
        public const string EmployeeTimeSheet = "TimeSheet/GetAllTimeSheetByEmployee";

        // Timesheet
        public const string TimesheetList = "timesheet/list";
        public const string TimesheetAdd = "timesheet/add";
        public const string TimesheetUpdate = "timesheet/update";
        public const string TimesheetDelete = "timesheet/delete";

        // Holiday
        public const string HolidayList = "holiday/list";
        public const string HolidayRequest = "holiday/request";
        public const string HolidayUpdateStatus = "holiday/updatestatus";
        public const string EmployeeHolidays = "holiday/employee";

        // Alerts
        public const string AlertsList = "alert/evacuationlist";
        public const string AlertsUnread = "alert/unread";
        public const string AlertsEvacuation = "alert/evacuation";
        public const string AlertsMarkAsRead = "alert/markasread";
        public const string AlertsEmployeesList = "alert/employeeslist";
        public const string AlertsEvacuationMessages = "alert/evacuationmessages";

        // Messages
        public const string MessagesList = "messages";
    }
} 