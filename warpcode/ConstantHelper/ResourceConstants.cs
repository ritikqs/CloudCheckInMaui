using CCIMIGRATION.MultilanguageHelper;

namespace CCIMIGRATION.ConstantHelper
{
    /// <summary>
    /// Resource constants class that provides strongly-typed access to localized strings.
    /// This class acts as a bridge between the JSON localization files and the application code.
    /// </summary>
    public static class ResourceConstants
    {
        private static LocalizationResourceManager _localizationManager => LocalizationResourceManager.Instance;

        #region Account & Authentication
        public static string AccountVerifiedAdmin => _localizationManager["Account_verfied_Admin"];
        public static string Authenticating => _localizationManager["Authenticating"];
        public static string Authentication => _localizationManager["Authentication"];
        public static string AuthenticateAccessToLoginToApp => _localizationManager["Authenticateaccesstologintoapp"];
        public static string AuthorizedBy => _localizationManager["AuthorizedBy"];
        #endregion

        #region App Information
        public static string AppName => _localizationManager["AppName"];
        public static string ComingSoon => _localizationManager["ComingSoon"];
        public static string Success => _localizationManager["Success"];
        #endregion

        #region Buttons & Actions
        public static string Cancel => _localizationManager["Cancel"];
        public static string Done => _localizationManager["Done"];
        public static string Next => _localizationManager["Next"];
        public static string Proceed => _localizationManager["Proceed"];
        public static string Submit => _localizationManager["Submit"];
        public static string SubmitApproval => _localizationManager["SubmitApprovel"];
        public static string Send => _localizationManager["Send"];
        public static string Ok => _localizationManager["Ok"];
        public static string GetStarted => _localizationManager["GetStarted"];
        public static string GoBack => _localizationManager["GoBack"];
        public static string TurnOn => _localizationManager["TurnOn"];
        public static string ClickToStart => _localizationManager["ClickToStart"];
        public static string PressHere => _localizationManager["PressHere"];
        public static string AllowInSettings => _localizationManager["AllowInSettings"];
        #endregion

        #region Check-In/Check-Out
        public static string CheckIn => _localizationManager["CheckIn"];
        public static string CheckOut => _localizationManager["CheckOut"];
        public static string CheckInButtonText => _localizationManager["CheckInButtonText"];
        public static string CheckoutButtonText => _localizationManager["CheckoutButtonText"];
        public static string CheckOutValidation => _localizationManager["CheckOutValidation"];
        public static string ClockIn => _localizationManager["ClockIn"];
        public static string ClockOut => _localizationManager["ClockOut"];
        public static string SignIn => _localizationManager["SignIn"];
        public static string SignOut => _localizationManager["SignOut"];
        public static string ShiftTime => _localizationManager["ShiftTime"];
        public static string WorkingHours => _localizationManager["WorkingHours"];
        public static string Attendance => _localizationManager["Attendance"];
        #endregion

        #region Login & Registration
        public static string Login => _localizationManager["Login"];
        public static string LoginButtonText => _localizationManager["LoginButtonText"];
        public static string LoginWithFingerOrPin => _localizationManager["LoginWithFingerOrPin"];
        public static string Logout => _localizationManager["Logout"];
        public static string Register => _localizationManager["Register"];
        public static string RegisterFormTitle => _localizationManager["RegisterFormTitle"];
        public static string UserRegistration => _localizationManager["UserRegistration"];
        public static string Registering => _localizationManager["Registering"];
        public static string LogginIn => _localizationManager["LogginIn"];
        public static string LoggingOut => _localizationManager["Logging Out"];
        public static string DontHaveAccount => _localizationManager["DontHaveAccount"];
        public static string ThankYouForRegistering => _localizationManager["ThankYou_for_Registering"];
        #endregion

        #region Password & Security
        public static string ForgotPassword => _localizationManager["ForgotPassword"];
        public static string ResetPassword => _localizationManager["ResetPassword"];
        public static string ResetCode => _localizationManager["ResetCode"];
        public static string Pin => _localizationManager["Pin"];
        public static string CreatePinTitle => _localizationManager["CreatePinTitle"];
        public static string SavingPin => _localizationManager["SavingPin"];
        public static string FourDigitPin => _localizationManager["FourDigitPin"];
        public static string PlaceFinger => _localizationManager["Placefinger"];
        #endregion

        #region Form Placeholders
        public static string EmailPlaceHolder => _localizationManager["EmailPlaceHolder"];
        public static string PasswordPlaceholder => _localizationManager["PasswordPlaceholder"];
        public static string PinPlaceholder => _localizationManager["PinPlaceholder"];
        public static string ConfirmPasswordPlaceholder => _localizationManager["ConfirmPasswordPlaceholder"];
        public static string FirstNamePlaceholder => _localizationManager["FirstNamePlaceholder"];
        public static string LastNamePlaceholder => _localizationManager["LastNamePlaceholder"];
        public static string DOBPlaceholder => _localizationManager["DOBPlaceholder"];
        public static string MobilePlaceHolder => _localizationManager["MobilePlaceHolder"];
        public static string PhoneNumberPlaceholder => _localizationManager["PhoneNumberPlaceholder"];
        public static string NINumberPlaceholder => _localizationManager["NINumberPlaceholder"];
        public static string NIdPlaceholder => _localizationManager["NIdPlaceholder"];
        public static string EmpPlaceholder => _localizationManager["EmpPlaceholder"];
        public static string NamePlaceHolder => _localizationManager["NamePlaceHolder"];
        public static string MaidenPlaceHolder => _localizationManager["MaidenPlaceHolder"];
        public static string OtherTradePlaceholder => _localizationManager["OtherTradePlaceholder"];
        public static string OtherContractor => _localizationManager["OtherContractor"];
        #endregion

        #region Validation Messages
        public static string EmailValidation => _localizationManager["EmailValidation"];
        public static string PasswordValidation => _localizationManager["PasswordValidation"];
        public static string ConfirmPasswordValidation => _localizationManager["ConfirmPasswordValidation"];
        public static string ConfirmPinValidation => _localizationManager["ConfirmPinValidation"];
        public static string PinValidation => _localizationManager["PinValidation"];
        public static string FNameValidation => _localizationManager["FNameValidation"];
        public static string LNameValidation => _localizationManager["LNameValidation"];
        public static string DOBValidation => _localizationManager["DOBValidation"];
        public static string PhoneValidation => _localizationManager["PhoneValidation"];
        public static string PhoneNumberError => _localizationManager["PhoneNumberError"];
        public static string NIValidation => _localizationManager["NIValidation"];
        public static string EmpIdValidation => _localizationManager["EmpIdValidation"];
        public static string NameValidation => _localizationManager["NameValidation"];
        public static string MotherNameValidation => _localizationManager["MotherNameValidation"];
        public static string CodeValidation => _localizationManager["CodeValidation"];
        public static string NoInputError => _localizationManager["NoInputError"];
        public static string InputFieldValidationError => _localizationManager["InputFieldValidationError"];
        public static string PasswordNotMatch => _localizationManager["PasswordNotMatch"];
        public static string PasswordResetSuccess => _localizationManager["PasswordResetSuccess"];
        public static string CheckEmailForResetPassword => _localizationManager["CheckEmailForResetPassword"];
        public static string ReturnToLogin => _localizationManager["ReturnToLogin"];
        #endregion

        #region Employee Information
        public static string Contractor => _localizationManager["Contractor"];
        public static string Employer => _localizationManager["Employer"];
        public static string Trade => _localizationManager["Trade"];
        public static string Other => _localizationManager["Other"];
        public static string ContractorValidation => _localizationManager["ContractorValidation"];
        public static string EmployerValidation => _localizationManager["EmployerValidation"];
        public static string TradeValidation => _localizationManager["TradeValidation"];
        public static string SelectContractorError => _localizationManager["SelectContractorError"];
        public static string SelectEmployerError => _localizationManager["SelectEmployerError"];
        public static string SelectTradeError => _localizationManager["SelectTradeError"];
        public static string SelectOtherTradeError => _localizationManager["SelectOtherTradeError"];
        public static string SelectOtherContractorError => _localizationManager["SelectOtherOntractorError"];
        public static string SearchContractor => _localizationManager["SearchContractor"];
        public static string SearchEmployer => _localizationManager["SearchEmployer"];
        public static string SearchTrade => _localizationManager["SearchTrade"];
        public static string InsuranceNumberValidation => _localizationManager["InsuranceNumberValidation"];
        public static string MotherNameRequired => _localizationManager["MotherNameRequired"];
        #endregion

        #region Site & Location
        public static string SelectSite => _localizationManager["SelectSite"];
        public static string SiteValidation => _localizationManager["SiteValidation"];
        public static string LoadingSites => _localizationManager["LoadingSites"];
        public static string CurrentSiteError => _localizationManager["CurrentSiteError"];
        public static string NotOnSite => _localizationManager["NotOnSite"];
        public static string NotOnActiveSite => _localizationManager["NotOnActiveSite"];
        public static string OnActiveSite => _localizationManager["OnActiveSite"];
        public static string OutOfSite => _localizationManager["OutOfSite"];
        public static string NotInSiteReach => _localizationManager["NotInSiteReach"];
        public static string YouAreIn => _localizationManager["YouAreIn"];
        public static string YouCanWorkOn => _localizationManager["YouCanWorkOn"];
        public static string SiteAndDate => _localizationManager["SiteAndDate"];
        public static string StaffOnSite => _localizationManager["StaffOnSite"];
        public static string StaffOn => _localizationManager["Staffon"];
        public static string NonAuth => _localizationManager["NonAuth"];
        #endregion

        #region Location & Permissions
        public static string EnableLocation => _localizationManager["EnableLocation"];
        public static string EnableLocationForCheckIn => _localizationManager["EnableLocationForCheckIn"];
        public static string EnableLocationAndAccess => _localizationManager["EnableLocationAndAccess"];
        public static string LocationRequired => _localizationManager["LocationRequired"];
        public static string LocationUnavailable => _localizationManager["LocationUnavailable"];
        public static string LocationConsent => _localizationManager["LocationConsent"];
        public static string LocationOnWarning => _localizationManager["LocationOnWarning"];
        public static string NeedLocationInBackground => _localizationManager["NeedLocationInBackgrund"];
        public static string OpenSettingsForLocation => _localizationManager["OpenSettingsForLocation"];
        public static string PermissionError => _localizationManager["PermissionError"];
        public static string MakeSureLocationOn => _localizationManager["MakeSureLocationOn"];
        public static string GoBackLocationWarning => _localizationManager["GoBackLocationWarning"];
        #endregion

        #region Camera & Photo Capture
        public static string Capture => _localizationManager["Capture"];
        public static string Capturing => _localizationManager["Capturing"];
        public static string Captured => _localizationManager["Captured"];
        public static string ReCapture => _localizationManager["ReCapture"];
        public static string FaceCaptureTitle => _localizationManager["FaceCaptureTitle"];
        public static string LookIntoCamera => _localizationManager["LookintoCamera"];
        public static string CameraUnavailable => _localizationManager["CameraUnavailable"];
        public static string GrantCameraPermission => _localizationManager["GrantCameraPermission"];
        public static string ErrorCapturingPhoto => _localizationManager["ErrorCapturingPhoto"];
        public static string RequiredPhotoForCheckout => _localizationManager["RequiredPhotoForCheckout"];
        public static string PhotosSubmitted => _localizationManager["PhotosSubmitted"];
        public static string PhotoRecapturedContactAdmin => _localizationManager["PhotoRecapturedContactAdmin"];
        public static string ImageDeleteMessage => _localizationManager["ImageDeleteMessage"];
        #endregion

        #region Status Messages
        public static string AlreadyCheckedIn => _localizationManager["AlreadyCheckedIn"];
        public static string AlreadyCheckedOut => _localizationManager["AlreadyCheckedOut"];
        public static string YouAreAlreadyCheckedIn => _localizationManager["YouAreAlreadyCheckedIn"];
        public static string YouAreSignedOut => _localizationManager["YouAreSignedOut"];
        public static string YouMustCheckOutFirst => _localizationManager["YouMustCheckOutFirst"];
        public static string FailedCheckIn => _localizationManager["FailedCheckIn"];
        public static string FailedCheckOut => _localizationManager["FailedCheckOut"];
        public static string FaceNotMatch => _localizationManager["FaceNotMatch"];
        public static string FingerAuthFailedMessage => _localizationManager["FingerAuthFailedMessage"];
        #endregion

        #region Holiday & Leave Management
        public static string Holiday => _localizationManager["Holiday"];
        public static string ApplyForHoliday => _localizationManager["ApplyForHoliday"];
        public static string ApplyHolidayText => _localizationManager["ApplyHolidayText"];
        public static string RequestHoliday => _localizationManager["RequestHoliday"];
        public static string StartHoliday => _localizationManager["StartHoliday"];
        public static string EndHoliday => _localizationManager["EndHoliday"];
        public static string HolidayCalender => _localizationManager["HolidayCalender"];
        public static string HolidayRequestSubmitted => _localizationManager["HolidayRequestSubimtted"];
        public static string HolidayApprovedSuccess => _localizationManager["HolidayApprovedSuccess"];
        public static string HolidayRejectSuccess => _localizationManager["HolidayRejectSuccess"];
        public static string HolidayDateTimeValidationError => _localizationManager["HolidayDateTimeValidatonError"];
        public static string NoHolidayEventFound => _localizationManager["NoHolidayEventFound"];
        public static string CannotApplyHoliday => _localizationManager["CannotApplyHoliday"];
        public static string CannotSeeHoliday => _localizationManager["CannotSeeHoliday"];
        public static string YouMustCheckInForHoliday => _localizationManager["YouMustCheckInForHoliday"];
        public static string AlreadyApproved => _localizationManager["AlreadyApproved"];
        public static string LeaveStatusApproved => _localizationManager["LeaveStatusApproved"];
        public static string LeaveStatusPending => _localizationManager["LeaveStatusPending"];
        public static string LeaveStatusRejected => _localizationManager["LeaveStatusRejected"];
        public static string IsOnLeaveOn => _localizationManager["Isonleaveon"];
        #endregion

        #region Date & Time
        public static string StartDate => _localizationManager["StartDate"];
        public static string EndDate => _localizationManager["EndDate"];
        public static string StartTime => _localizationManager["StartTime"];
        public static string EndTime => _localizationManager["EndTime"];
        public static string Start => _localizationManager["Start"];
        public static string End => _localizationManager["End"];
        public static string From => _localizationManager["From"];
        public static string To => _localizationManager["To"];
        public static string Date => _localizationManager["Date"];
        public static string Pause => _localizationManager["Pause"];
        public static string Stop => _localizationManager["Stop"];
        public static string ThisWeek => _localizationManager["ThisWeek"];
        public static string YouHaveWorked => _localizationManager["YouHaveWorked"];
        #endregion

        #region Months
        public static string January => _localizationManager["January"];
        public static string February => _localizationManager["February"];
        public static string March => _localizationManager["March"];
        public static string April => _localizationManager["April"];
        public static string May => _localizationManager["May"];
        public static string June => _localizationManager["June"];
        public static string July => _localizationManager["July"];
        public static string August => _localizationManager["August"];
        public static string September => _localizationManager["September"];
        public static string October => _localizationManager["October"];
        public static string November => _localizationManager["November"];
        public static string December => _localizationManager["December"];
        #endregion

        #region Menu Items
        public static string MenuHome => _localizationManager["MenuHome"];
        public static string MenuTimesheet => _localizationManager["MenuTimesheet"];
        public static string MenuHoliday => _localizationManager["MenuHoliday"];
        public static string MenuEvacuation => _localizationManager["MenuEvacuation"];
        public static string MenuCalender => _localizationManager["MenuCalender"];
        public static string MenuVisitors => _localizationManager["MenuVisitors"];
        public static string MenuLogout => _localizationManager["MenuLogout"];
        #endregion

        #region Page Titles
        public static string PageTitleHoliday => _localizationManager["PageTitleHoliday"];
        public static string PageTitleVisitors => _localizationManager["PageTitleVisitors"];
        public static string PageTitleEvacuation => _localizationManager["PageTitleEvacuation"];
        #endregion

        #region TimeSheet
        public static string TimeSheets => _localizationManager["TimeSheets"];
        public static string LoadingTimeSheet => _localizationManager["LoadingTimeSheet"];
        #endregion

        #region Evacuation & Alerts
        public static string SendAlert => _localizationManager["SendAlert"];
        public static string AlertMessage => _localizationManager["AlertMessage"];
        public static string Alerts => _localizationManager["Alerts"];
        public static string AddEvacuationMessage => _localizationManager["AddEvacutationMessage"];
        public static string SelectUserForEvacuation => _localizationManager["SelectUserForEvacuation"];
        public static string EvacuationAlertSuccess => _localizationManager["EvacuationAlertSuccess"];
        public static string Sent => _localizationManager["Sent"];
        public static string Received => _localizationManager["Received"];
        #endregion

        #region Visitors
        public static string AddNewVisitor => _localizationManager["AddNewVisitor"];
        public static string CannotSeeEmployees => _localizationManager["CannotSeeEmployees"];
        #endregion

        #region Safety & Confirmation
        public static string IConfirmSafetyMeasures => _localizationManager["IConfirmSafetyMeasures"];
        public static string ReviewAndConfirm => _localizationManager["ReviewAndConfirm"];
        public static string SafetyMeasureErrorMessage => _localizationManager["SafetyMesaureErrorMessage"];
        public static string NotAllowedToWorkOnSite => _localizationManager["NotAllowedToWorkonSite"];
        public static string HaveSafeDay => _localizationManager["HaveSafeDay"];
        #endregion

        #region Loading & Status
        public static string Loading => _localizationManager["Loading"];
        public static string Loading1 => _localizationManager["Loading1"];
        public static string SyncingTime => _localizationManager["SyncingTime"];
        public static string TimeSynced => _localizationManager["TimeSynced"];
        public static string NoData => _localizationManager["NoData"];
        public static string NoEvents => _localizationManager["NoEvents"];
        #endregion

        #region Error Messages
        public static string SomethingWentWrong => _localizationManager["SomethingwentWrong"];
        public static string SomethingWentWrongTryAgain => _localizationManager["SomethingWentWrongTryAgain"];
        public static string NoInternet => _localizationManager["NoInternet"];
        public static string ErrorGettingLocation => _localizationManager["ErrorGettingLocation"];
        public static string ErrorGettingTime => _localizationManager["ErrorGettingTime"];
        public static string ErrorCompressing => _localizationManager["ErrorCompressing"];
        public static string ErrorUploadingImage => _localizationManager["ErrorUploadingImage"];
        public static string ErrorUploadingAzure => _localizationManager["ErrorUploadingAzure"];
        public static string OfflineSyncError => _localizationManager["OfflineSyncError"];
        #endregion

        #region Language & Localization
        public static string SelectLanguage => _localizationManager["SelectLanguage"];
        public static string ChangeLanguage => _localizationManager["ChangeLanguage"];
        #endregion

        #region General UI
        public static string Hello => _localizationManager["Hello"];
        public static string Messages => _localizationManager["Messages"];
        public static string Calender => _localizationManager["Calender"];
        #endregion

        #region Colors (UI Theme)
        public static string Red => _localizationManager["Red"];
        public static string LightGray => _localizationManager["LightGray"];
        public static string InputFieldPlaceholderColor => _localizationManager["InputFieldPlaceholderColor"];
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets a localized string by key. Use this method when you need dynamic resource access.
        /// </summary>
        /// <param name="key">The resource key</param>
        /// <returns>The localized string or the key if not found</returns>
        public static string GetString(string key)
        {
            return _localizationManager[key] ?? key;
        }

        /// <summary>
        /// Checks if a resource key exists
        /// </summary>
        /// <param name="key">The resource key to check</param>
        /// <returns>True if the key exists, false otherwise</returns>
        public static bool HasKey(string key)
        {
            try
            {
                var value = _localizationManager[key];
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
