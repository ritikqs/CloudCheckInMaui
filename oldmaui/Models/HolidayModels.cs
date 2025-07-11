using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CloudCheckInMaui.Models.SiteManagerModel;

namespace CloudCheckInMaui.Models
{
    public class HolidayRequest
    {
        [JsonPropertyName("empID")]
        public string EmpId { get; set; }
        [JsonPropertyName("fromDate")]
        public DateTime FromDate { get; set; }
        [JsonPropertyName("toDate")]
        public DateTime ToDate { get; set; }
        [JsonPropertyName("noDaysRequired")]
        public double DaysRequired { get; set; }
    }

    public class HolidayStatusUpdate
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("approved")]
        public bool Approved { get; set; }
        [JsonPropertyName("approvedBy")]
        public string ApprovedBy { get; set; }
    }

    public class HolidayListResponse : INotifyPropertyChanged
    {
        [JsonPropertyName("approved")]
        public bool Approved { get; set; }

        [JsonPropertyName("empID")]
        public string EmpId { get; set; }

        [JsonPropertyName("approvedBy")]
        public string ApprovedBy { get; set; }

        [JsonPropertyName("noDaysRequired")]
        public double DaysRequired { get; set; }

        [JsonPropertyName("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonPropertyName("toDate")]
        public DateTime ToDate { get; set; }

        [JsonPropertyName("employee")]
        public Employee Employee { get; set; }

        [JsonPropertyName("encryptId")]
        public string EncryptId { get; set; }

        //[JsonPropertyName("id")]
        //public int Id { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public DateTime? DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public string DeletedBy { get; set; }

        private string _statusButtonBorderColor;
        [JsonIgnore]
        public string StatusButtonBorderColor
        {
            get => _statusButtonBorderColor;
            set
            {
                _statusButtonBorderColor = value;
                OnPropertyChanged();
            }
        }

        private string _statusButtonBackgroundColor;
        [JsonIgnore]
        public string StatusButtonBackgroundColor
        {
            get => _statusButtonBackgroundColor;
            set
            {
                _statusButtonBackgroundColor = value;
                OnPropertyChanged();
            }
        }

        private string _statusButtonTextColor;
        [JsonIgnore]
        public string StatusButtonTextColor
        {
            get => _statusButtonTextColor;
            set
            {
                _statusButtonTextColor = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string FromDateString { get; set; }
        [JsonIgnore]
        public string ToDateString { get; set; }

        private bool _isApproveRejectButtonVisible;
        [JsonIgnore]
        public bool IsApproveRejectButtonVisible
        {
            get => _isApproveRejectButtonVisible;
            set
            {
                _isApproveRejectButtonVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isLeaveStatusVisible;
        [JsonIgnore]
        public bool IsLeaveStatusVisible
        {
            get => _isLeaveStatusVisible;
            set
            {
                _isLeaveStatusVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isSiteManagerInfoVisible;
        [JsonIgnore]
        public bool IsSiteManagerInfoVisible
        {
            get => _isSiteManagerInfoVisible;
            set
            {
                _isSiteManagerInfoVisible = value;
                OnPropertyChanged();
            }
        }

        private string _leaveStatus;
        [JsonIgnore]
        public string LeaveStatus
        {
            get => _leaveStatus;
            set
            {
                _leaveStatus = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SiteManagerHolidayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartHolidayDate { get; set; }
        public string EndHolidayDate { get; set; }
    }

    public class SiteManagerHolidayListRequest
    {
        [JsonPropertyName("draw")]
        public long Draw { get; set; }

        [JsonPropertyName("columns")]
        public List<SiteManagerHolidayColumn> Columns { get; set; }

        [JsonPropertyName("order")]
        public List<SiteManagerHolidayOrder> Order { get; set; }

        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("length")]
        public long Length { get; set; }

        [JsonPropertyName("empId")]
        public string EmpId { get; set; }

        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }
    }

    public class SiteManagerHolidayColumn
    {
        [JsonPropertyName("data")]
        public long Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }

        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }

        [JsonPropertyName("search")]
        public SiteManagerHolidaySearch Search { get; set; }
    }

    public class SiteManagerHolidaySearch
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("isRegex")]
        public bool IsRegex { get; set; }
    }

    public class SiteManagerHolidayOrder
    {
        [JsonPropertyName("column")]
        public long Column { get; set; }

        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }
} 