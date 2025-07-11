using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCheckInMaui.Models
{
    public class CheckInTimesheetUpdateRequest
    {
        [JsonProperty("employeeID")]
        public string EmployeeId { get; set; }

        [JsonProperty("siteID")]
        public string SiteId { get; set; }

        [JsonProperty("timeOnSite")]
        public DateTime? TimeOnSite { get; set; }

        [JsonProperty("checkInImage")]
        public string CheckInImage { get; set; }
    }
    
    public class TimesheetUpdateRequest
    {
        [JsonProperty("employeeID")]
        public string EmployeeId { get; set; }

        [JsonProperty("isNonAuthTimeOffSite")]
        public bool IsNonAuthTimeOffSite { get; set; }
        
        [JsonProperty("siteID")]
        public string SiteId { get; set; }

        [JsonProperty("timeOnSite")]
        public DateTime? TimeOnSite { get; set; }

        [JsonProperty("timeOffSite")]
        public DateTime? TimeOffSite { get; set; }
        
        [JsonProperty("checkOutImage")]
        public string CheckOutImage { get; set; }

        [JsonProperty("workingHours")]
        public double WorkingHours { get; set; }
    }
    
    public class TimeSheetRequest
    {
        public string SiteName { get; set; }

        public string OnSite_Time { get; set; }

        public string OffSite_Time { get; set; }

        public string Date { get; set; }

        public string NonAuthOffSiteTime { get; set; }
        
        public bool IsNonAuthTime { get; set; }
    }
    
    public class UserTimeSheetRequest
    {
        [JsonProperty("draw")]
        public long Draw { get; set; }

        [JsonProperty("columns")]
        public Column[] Columns { get; set; }

        [JsonProperty("order")]
        public Order[] Order { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("search")]
        public Search Search { get; set; }

        [JsonProperty("empId")]
        public string EmpId { get; set; }
    }
    
    public partial class Column
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("searchable")]
        public bool Searchable { get; set; }

        [JsonProperty("orderable")]
        public bool Orderable { get; set; }

        [JsonProperty("search")]
        public Search Search { get; set; }
    }
    
    public partial class Search
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("isRegex")]
        public bool IsRegex { get; set; }
    }

    public partial class Order
    {
        [JsonProperty("column")]
        public long Column { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }
    }
    
    public class WeeklyHoursRequest
    {
        [JsonProperty("empId")]
        public string EmpId { get; set; }
    }
    
    public class WeeklyhoursResult
    {
        [JsonProperty("result")]
        public string Result { get; set; }
    }
    
    public class CheckInStatusResponse
    {
        [JsonProperty("checkInTime")]
        public DateTime CheckInTime { get; set; }

        [JsonProperty("isCheckIn")]
        public bool IsCheckIn { get; set; }

        [JsonProperty("siteId")]
        public Guid SiteId { get; set; }
    }

    public class TimesheetModel
    {
        public string SiteName { get; set; }
        public string Date { get; set; }
        public string OnSite_Time { get; set; }
        public string OffSite_Time { get; set; }
        public bool IsNonAuthTime { get; set; }
        public double TotalHours { get; set; }
        public string Status { get; set; }
    }
} 