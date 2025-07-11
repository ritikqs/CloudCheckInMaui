using Newtonsoft.Json;
using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class ApplyHolidayRequest
    {

        [JsonProperty("empID")]
        public string EmpId { get; set; }

        [JsonProperty("noDaysRequired")]
        public double NoDaysRequired { get; set; }

        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime ToDate { get; set; }

    }

    public class ApplyHolidayResponse
    {

    }
    public class HolidayListResponse : BaseViewModel
    {
        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [JsonProperty("empID")]
        public Guid EmpId { get; set; }

        [JsonProperty("approvedBy")]
        public string ApprovedBy { get; set; }

        [JsonProperty("noDaysRequired")]
        public long NoDaysRequired { get; set; }

        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime ToDate { get; set; }

        [JsonProperty("employee")]
        public Employee Employee { get; set; }

        [JsonProperty("encryptId")]
        public string EncryptId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonProperty("deletedOn")]
        public object DeletedOn { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }
        private string _statusButtonBorderColor;
        [JsonIgnore]
        public string StatusButtonBorderColor
        {
            get { return _statusButtonBorderColor; }
            set { _statusButtonBorderColor = value;
                OnPropertyChanged();
            }
        }
        private string _statusButtonBackgroundColor;

        [JsonIgnore]
        public string StatusButtonBackgroundColor
        {
            get { return _statusButtonBackgroundColor; }
            set { _statusButtonBackgroundColor = value;OnPropertyChanged(); }
        }
        private string _statusButtonTextColor;
        [JsonIgnore]
        public string StatusButtonTextColor { get { return _statusButtonTextColor; } set { _statusButtonTextColor = value;OnPropertyChanged(); } }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        private bool _isApproveRejectButtonVisible;
        private bool _isLeaveStatusVisible;
        private bool _isSiteManagerInfoVisible;
        private string _leaveStatus;
        public bool IsApproveRejectButtonVisible
        {
            get
            {
                return _isApproveRejectButtonVisible;
            }
            set
            {
                _isApproveRejectButtonVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsSiteManagerInfoVisible
        {
            get
            {
                return _isSiteManagerInfoVisible;
            }
            set
            {
                _isSiteManagerInfoVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsLeaveStatusVisible
        {
            get
            {
                return _isLeaveStatusVisible;
            }
            set
            {
                _isLeaveStatusVisible = value;
                OnPropertyChanged();
            }
        }
        public string LeaveStatus
        {
            get
            {
                return _leaveStatus;
            }
            set
            {
                _leaveStatus = value;
                OnPropertyChanged();
            }
        }

    }
    public partial class Employee
    {
        [JsonProperty("empID")]
        public Guid EmpId { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("firstMidName")]
        public string FirstMidName { get; set; }

        [JsonProperty("joinDate")]
        public DateTimeOffset JoinDate { get; set; }

        [JsonProperty("voiceRecKey")]
        public string VoiceRecKey { get; set; }

        [JsonProperty("empNINumber")]
        public string EmpNiNumber { get; set; }

        [JsonProperty("empVoiceEnroll")]
        public long EmpVoiceEnroll { get; set; }

        [JsonProperty("empFaceEnroll")]
        public long EmpFaceEnroll { get; set; }

        [JsonProperty("empThumbEnroll")]
        public long EmpThumbEnroll { get; set; }

        [JsonProperty("empRifdEnroll")]
        public long EmpRifdEnroll { get; set; }

        [JsonProperty("picturesTakenCount")]
        public long PicturesTakenCount { get; set; }

        [JsonProperty("employeeID")]
        public string EmployeeId { get; set; }

        [JsonProperty("employeeCaptureImage")]
        public object EmployeeCaptureImage { get; set; }

        [JsonProperty("employeeUser")]
        public object EmployeeUser { get; set; }

        [JsonProperty("startDate")]
        public object StartDate { get; set; }

        [JsonProperty("endDate")]
        public object EndDate { get; set; }

        [JsonProperty("email")]
        public object Email { get; set; }

        [JsonProperty("isEmailLinked")]
        public bool IsEmailLinked { get; set; }

        [JsonProperty("voiceVerified")]
        public bool VoiceVerified { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonProperty("deletedOn")]
        public object DeletedOn { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }
    }
    public class ApproveRejectHolidayModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [JsonProperty("approvedBy")]
        public string ApprovedBy { get; set; }
    }
}
