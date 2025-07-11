using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CloudCheckInMaui.Models
{
    public class EventRequestModel
    {
        [JsonPropertyName("managerId")]
        public string ManagerId { get; set; }

        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }
    }
    
    public partial class EventResponse
    {
        [JsonPropertyName("day")]
        public DateTime Day { get; set; }

        [JsonPropertyName("employeesOnLeave")]
        public EmployeesOnLeave[] EmployeesOnLeave { get; set; }
    }

    public partial class EmployeesOnLeave
    {
        [JsonPropertyName("empId")]
        public string EmpId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        
        [JsonPropertyName("approvedBy")]
        public string ApprovedBy { get; set; }
    }
} 