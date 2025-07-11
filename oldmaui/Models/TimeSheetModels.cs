using System;

namespace CloudCheckInMaui.Models
{
    public class CheckInRequest
    {
        public Guid EmpId { get; set; }
        public Guid SiteId { get; set; }
        public string Image { get; set; }
    }

    public class CheckOutRequest
    {
        public Guid EmpId { get; set; }
        public string Image { get; set; }
    }

    public class TimesheetResponse
    {
        public Site Site { get; set; }
        public DateTime TimeOnSite { get; set; }
        public DateTime? TimeOffSite { get; set; }
        public DateTime? NonAutTimeOffSite { get; set; }
    }

    public class TimesheetDataResponse
    {
        public TimesheetResponse[] Data { get; set; }
    }

    public class WeeklyHoursResponse
    {
        public string TotalHours { get; set; }
    }

    public class Site
    {
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
    }
} 