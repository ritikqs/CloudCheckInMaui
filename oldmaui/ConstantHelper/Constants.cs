using Microsoft.Maui.Graphics;

namespace CloudCheckInMaui.ConstantHelper
{
    public static class Constants
    {
        // API Base
        public static string BaseUrl = "https://cciapi.qservicesit.com/api/";


        // User data keys
        public static readonly string UserData = "UserData";
        public static readonly string CurrentSite = "CurrentSite";
        public static readonly string RecentSite = "RecentSite";
        public static readonly string CheckInTime = "CheckInTime";
        public static readonly string UpdateTime = "UpdateTime";
        public static readonly string MonitoringTime = "MonitoringTime";
        public static readonly string AppLanguageCode = "AppLanguageCode";
        public static readonly string CheckedInSite = "CheckedInSite";
        public static readonly string OnceRegistered = "IsOnceRegistered";
        public static readonly string LocationDisclouser = "IsLocationDisclouserShown";
        public static readonly string NativeCheckOutTime = "NativeCheckOutTime";
        public static readonly string AnimateText = "AnimateText";
        public static readonly string ClockInTime = "ClockInTime";
        public static readonly string LastGeofenceChecked = "LastGeofenceChecked";

        // API Endpoints
        public static readonly string Login = "EmployeeUser/Login";
        public static readonly string Logout = "EmployeeUser/Logout";
        public static readonly string Register = "EmployeeUser/Register";
        public static readonly string ForgotPassword = "EmployeeUser/ForgetPassword";
        public static readonly string ResetPassword = "EmployeeUser/ResetPassword";
        public static readonly string ValidateRefreshToken = "EmployeeUser/ValidateRefreshToken";
        public static readonly string LoggedBy = "EmployeeUser/LoggedBy";
        public static readonly string GetAllSites = "GetAllSites";
        public static readonly string TimeSheet = "TimeSheet";
        public static readonly string CheckIn = "TimeSheet/CheckInSite";
        public static readonly string EmployeeTimeSheet = "TimeSheet/GetAllTimeSheetByEmployee";
        public static readonly string RetakeEmployeeImage = "Employee/RetakeEmployeeImage";
        public static readonly string CreateHolidayRequest = "EmployeeHoliday/InsertEmployeeHoliday";
        public static readonly string GetEmployeeHoliday = "EmployeeHoliday";
        public static readonly string GetUserDetails = "GetEmployeeDetails";
        public static readonly string CreateUpdateTimeSheet = "CreateUpdateTimeSheet";
        public static readonly string Contractor = "Contractor";
        public static readonly string Employer = "Employer";
        public static readonly string EmployeeType = "EmployeeType";
        public static readonly string Trade = "Trade";
        public static readonly string SendEvacuationalert = "Evacuation/InsertEvacuation";
        public static readonly string GetEvacuationMessages = "Evacuation/GetEvacuationAlertMessage";
        public static readonly string GetEvacuationsList = "Evacuation";
        public static readonly string GetEvacuationsListForEmployee = "Evacuation/GetAllEvactionForEmployee";
        public static readonly string MarkReadEvacuation = "Evacuation/UpdateEvacuation";
        public static readonly string GetAllStaffOnSite = "SiteManager/GetAllStaffOnSite";
        public static readonly string SyncOfflineTimeSheet = "TimeSheet/SyncOfflineSite";
        public static readonly string GeofenceSite = "Sites/GetCurrentSite";
        public static readonly string EmployeeImageStatus = "Employee/GetEmployeeImageStatus";
        public static readonly string GetMonitoringTime = "Employee/GetLocationTrackingTime";
        public static readonly string GetAllHolidayListBySiteManager = "EmployeeHoliday/GetAllEmployeeHolidayBySiteManager";
        public static readonly string UpdateEmployeeHoliday = "EmployeeHoliday/UpdateEmployeeHoliday";
        public static readonly string GetEmployeeHolidaysByMonth = "EmployeeHoliday/GetByMonth";
        public static readonly string EmployeeWeeklyhours = "Employee/GetEmployeeWeeklyHours";
        public static readonly string UserCheckInStatus = "TimeSheet/GetUserCheckInStatus";
        public static readonly string UpdateFailedLoginDetails = "EmployeeUser/InsertLoginFailedUser";
        public static readonly string CheckEmailOrNiNumberExist = "EmployeeUser/CheckEmailOrNINumberExist";
        public static readonly string UserCheckInOnAnySite = "TimeSheet/IsUserCheckInOnAnySite";
        public static readonly string Sites = "Sites";
        public static readonly string Employee = "Employee";

        // Face API
        public static readonly string FaceApiBaseUrl = "https://cci-cloudcheckin.cognitiveservices.azure.com/";
        public static readonly string FacialRecognitionAPIKey = "1IDNQktKsEMh5cMQwfTon0bWHgxUNuHqrr6vdl0nEp0D01wCMxyuJQQJ99AKACGhslBXJ3w3AAAKACOGh58S";

        // Service Keys
        public static readonly string StartService = "StartService";
        public static readonly string StopService = "StopService";

        // App Colors
        public static readonly Color AppThemeColor = Color.FromArgb("#3498db"); // or your theme color
        public static readonly Color Green = Colors.Green;
        public static readonly Color White = Colors.White;
        public static readonly Color Black = Colors.Black;
        public static readonly Color Gray = Colors.Gray;
        public static readonly Color LightGray = Colors.LightGray;
        public static readonly Color Red = Color.FromArgb("#f04a57");
        public static readonly Color AuthorizedColor = Color.FromArgb("#0d71ba");
        public static readonly Color UnAuthorizedColor = Color.FromArgb("#f48585");
        public static readonly Color PageBackground = White;

        // Defaults
        public static readonly int DefaultImageCount = 40;
        public static readonly int DefaultGeofenceInterval = 60000;

        // Date Formats
        public static readonly string DateFormat = "dd MMM h:mm tt";
        public static readonly string TimeSheetDateFormat = "dd MMM yyyy";
        public static readonly string TimeSheetTimeFormat = "h:mm tt";
    }
}
