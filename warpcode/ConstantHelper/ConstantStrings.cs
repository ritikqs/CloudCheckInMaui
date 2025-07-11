using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CCIMIGRATION.ConstantHelper;

namespace CCIMIGRATION.ConstantHelper
{
    public static partial class Constants
    {
        //Dev Base Url
        public static string BaseUrl = "https://cciapi.qservicesit.com/api";
        //public static string BaseUrl = "https://cci-CCIMIGRATION.cognitiveservices.azure.com/";

        //Prod Base Url
        //public static string BaseUrl = "https://eclapi.optrax.co.uk/api";
        //Test2
        //public static string BaseUrl = "https://api.optrax.co.uk/api";

        //public static string BaseUrl = "https://api-CCIMIGRATION.azurewebsites.net";

        //ConstantValue
        public static int DefaultImageCount = 40;
        public static int DefaultGeofenceInterval = 60000;




        //FaceApi Url
        public const string FaceApiBaseUrl = "https://cci-CCIMIGRATION.cognitiveservices.azure.com/";
        //public const string FaceApiBaseUrl = "https://uksouth.api.cognitive.microsoft.com/";
        //public const string FaceApiBaseUrl = "https://eastus.api.cognitive.microsoft.com/";


        //PreferenceKeys
        public static string OnceRegistered = "IsOnceRegistered";
        public static string LocationDisclouser = "IsLocationDisclouserShown";
        public static string CurrentSite = "CurrentSite";
        public static string RecentSite = "RecentSite";
        public static string CheckInTime = "CheckInTime";
        public static string UpdateTime = "UpdateTime";
        public static string UserData = "UserData";
        public static string MonitoringTime = "MonitoringTime";
        public static string AppLanguageCode = "AppLanguageCode";
        public static string CheckedInSite = "CheckedInSite";

        public static string NativeCheckOutTime = "NativeCheckOutTime";
        public static string StartService = "StartService";
        public static string StopService = "StopService";
        public static string AnimateText = "AnimateText";
        public static string ClockInTime = "ClockInTime";
        public static string LastGeofenceChecked = "LastGeofenceChecked";
        //Dateformat
        //public static string DateFormat = "dd/MM/yyyy h:mm tt";
        public static string DateFormat = "dd MMM h:mm tt";
        public static string TimeSheetDateFormat = "dd MMM yyyy";
        public static string TimeSheetTimeFormat = "h:mm tt";
        public static string LoggingTimeFormat = "h:mm tt";

        //Scope
        public static string Employee = "/Employee";
        public static string Sites = "/Sites";


        //Endpoint Url Constants
        public static string GetUserDetails = "/GetEmployeeDetails";
        public static string GetAllSites = "/GetAllSites";
        public static string CreateUpdateTimeSheet = "/CreateUpdateTimeSheet";
        //User
        public static string Login = "/EmployeeUser/Login";
        public static string Logout = "/EmployeeUser/Logout";
        public static string Register = "/EmployeeUser/Register";
        public static string ForgotPassword = "/EmployeeUser/ForgetPassword";
        public static string ResetPassword = "/EmployeeUser/ResetPassword";
        public static string ValidateRefreshToken = "/EmployeeUser/ValidateRefreshToken";
        public static string LoggedBy = "/EmployeeUser/LoggedBy";

        public static string Contractor = "/Contractor";
        public static string Employer = "/Employer";
        public static string EmployeeType = "/EmployeeType";
        public static string Trade = "/Trade";
        public static string SendEvacuationalert = "/Evacuation/InsertEvacuation";
        public static string GetEvacuationMessages = "/Evacuation/GetEvacuationAlertMessage";
        public static string GetEvacuationsList = "/Evacuation";
        public static string GetEvacuationsListForEmployee = "/Evacuation/GetAllEvactionForEmployee";
        public static string MarkReadEvacuation = "/Evacuation/UpdateEvacuation";

        public static string GetAllStaffOnSite = "/SiteManager/GetAllStaffOnSite";


        //TimeSheet
        public static string TimeSheet = "/TimeSheet";
        public static string SyncOfflineTimeSheet = "/TimeSheet/SyncOfflineSite";
        //geofence    
        public static string GeofenceSite = "/Sites/GetCurrentSite";
        public static string CheckIn = "/TimeSheet/CheckInSite";
        //Employee
        public static string EmployeeTimeSheet = "/TimeSheet/GetAllTimeSheetByEmployee";
        public static string EmployeeImageStatus = "/Employee/GetEmployeeImageStatus";
        public static string RetakeEmployeeImage = "/Employee/RetakeEmployeeImage";
        public static string CreateHolidayRequest = "/EmployeeHoliday/InsertEmployeeHoliday";
        public static string GetMonitoringTime = "/Employee/GetLocationTrackingTime";
        public static string GetAllHolidayListBySiteManager = "/EmployeeHoliday/GetAllEmployeeHolidayBySiteManager";
        public static string GetEmployeeHoliday = "/EmployeeHoliday";
        public static string UpdateEmployeeHoliday = "/EmployeeHoliday/UpdateEmployeeHoliday";
        public static string GetEmployeeHolidaysByMonth = "/EmployeeHoliday/GetByMonth";
        public static string EmployeeWeeklyhours = "/Employee/GetEmployeeWeeklyHours";
        public static string UserCheckInStatus = "/TimeSheet/GetUserCheckInStatus";
        public static string UpdateFailedLoginDetails = "/EmployeeUser/InsertLoginFailedUser";
        public static string CheckEmailOrNiNumberExist = "/EmployeeUser/CheckEmailOrNINumberExist";
        public static string UserCheckInOnAnySite = "/TimeSheet/IsUserCheckInOnAnySite";
        //Keys
        // public const string FacialRecognitionAPIKey = "b95ecfc8c76340f2bf2920f40a2612db";
        //public const string FacialRecognitionAPIKey = "2a796f5998ea485da631921cbe2bcdde";
        public const string FacialRecognitionAPIKey = "1IDNQktKsEMh5cMQwfTon0bWHgxUNuHqrr6vdl0nEp0D01wCMxyuJQQJ99AKACGhslBXJ3w3AAAKACOGh58S";


        //Helper Pattern
        public static string NameRegex = "^[A-Za-z][a-zA-Z]*$";
        public static string NumericText = "^[0-9]+$";
        public static string AlphanumericRegex = "^[A-Z0-9a-z][a-zA-Z0-9]*$";


        // Message Strings - Now using ResourceConstants for localization support
        // These properties provide backward compatibility while using the new localization system
        
        #region Localized Message Strings
        public static string NoInputError => ResourceConstants.NoInputError;
        public static string InputFieldValidationError => ResourceConstants.InputFieldValidationError;
        public static string NoInternet => ResourceConstants.NoInternet;
        public static string CheckEmailForResetPassword => ResourceConstants.CheckEmailForResetPassword;
        public static string PasswordNotMatch => ResourceConstants.PasswordNotMatch;
        public static string PasswordResetSuccess => ResourceConstants.PasswordResetSuccess;
        public static string SafetyMeasureErrorMessage => ResourceConstants.SafetyMeasureErrorMessage;
        public static string NotOnSite => ResourceConstants.NotOnSite;
        public static string EnableLocationForCheckIn => ResourceConstants.EnableLocationForCheckIn;
        public static string AlreadyCheckedIn => ResourceConstants.AlreadyCheckedIn;
        public static string AlreadyCheckedOut => ResourceConstants.AlreadyCheckedOut;
        public static string SelectTradeError => ResourceConstants.SelectTradeError;
        public static string SelectOtherTradeError => ResourceConstants.SelectOtherTradeError;
        public static string SelectOtherContractorError => ResourceConstants.SelectOtherContractorError;
        public static string SelectContractorError => ResourceConstants.SelectContractorError;
        public static string PhoneNumberError => ResourceConstants.PhoneNumberError;
        public static string SelectEmployerError => ResourceConstants.SelectEmployerError;
        public static string RequiredPhotoForCheckout => ResourceConstants.RequiredPhotoForCheckout;
        public static string FailedCheckIn => ResourceConstants.FailedCheckIn;
        public static string FailedCheckOut => ResourceConstants.FailedCheckOut;
        public static string CurrentSiteError => ResourceConstants.CurrentSiteError;
        public static string HolidayDateTimeValidationError => ResourceConstants.HolidayDateTimeValidationError;
        public static string ErrorGettingTime => ResourceConstants.ErrorGettingTime;
        public static string CannotApplyHoliday => ResourceConstants.CannotApplyHoliday;
        public static string CannotSeeHoliday => ResourceConstants.CannotSeeHoliday;
        public static string CannotSeeEmployees => ResourceConstants.CannotSeeEmployees;
        public static string SomethingWentWrongTryAgain => ResourceConstants.SomethingWentWrongTryAgain;
        public static string YouMustCheckInForHoliday => ResourceConstants.YouMustCheckInForHoliday;
        public static string AlreadyApproved => ResourceConstants.AlreadyApproved;
        public static string NotOnActiveSite => ResourceConstants.NotOnActiveSite;
        public static string OnActiveSite => ResourceConstants.OnActiveSite;
        public static string LeaveStatusApproved => ResourceConstants.LeaveStatusApproved;
        public static string LeaveStatusRejected => ResourceConstants.LeaveStatusRejected;
        public static string LeaveStatusPending => ResourceConstants.LeaveStatusPending;
        public static string SelectUserForEvacuation => ResourceConstants.SelectUserForEvacuation;
        public static string AddEvacuationMessage => ResourceConstants.AddEvacuationMessage;
        public static string EvacuationAlertSuccess => ResourceConstants.EvacuationAlertSuccess;
        public static string ErrorUploadingImage => ResourceConstants.ErrorUploadingImage;
        public static string ErrorCompressing => ResourceConstants.ErrorCompressing;
        public static string ErrorUploadingAzure => ResourceConstants.ErrorUploadingAzure;
        public static string GrantCameraPermission => ResourceConstants.GrantCameraPermission;
        public static string ErrorGettingLocation => ResourceConstants.ErrorGettingLocation;
        public static string NoHolidayEventFound => ResourceConstants.NoHolidayEventFound;
        public static string HolidayApprovedSuccess => ResourceConstants.HolidayApprovedSuccess;
        public static string HolidayRejectSuccess => ResourceConstants.HolidayRejectSuccess;
        public static string OfflineSyncError => ResourceConstants.OfflineSyncError;
        public static string AppName => ResourceConstants.AppName;
        public static string ImageDeleteMessage => ResourceConstants.ImageDeleteMessage;
        public static string ReCapture => ResourceConstants.ReCapture;
        public static string Cancel => ResourceConstants.Cancel;
        public static string LocationUnavailable => ResourceConstants.LocationUnavailable;
        public static string OutOfSite => ResourceConstants.OutOfSite;
        public static string YouAreSignedOut => ResourceConstants.YouAreSignedOut;
        public static string LocationOnWarning => ResourceConstants.LocationOnWarning;
        public static string NeedLocationInBackground => ResourceConstants.NeedLocationInBackground;
        public static string GoBackLocationWarning => ResourceConstants.GoBackLocationWarning;
        public static string EnableLocation => ResourceConstants.EnableLocation;
        public static string OpenSettingsForLocation => ResourceConstants.OpenSettingsForLocation;
        public static string Ok => ResourceConstants.Ok;
        #endregion

    }
    public class Database
    {
        public const string DatabaseFilename = "OptraxDb.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
