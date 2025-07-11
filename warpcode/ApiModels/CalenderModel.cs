using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class EventRequestModel
    {
        [JsonProperty("managerId")]
        public string ManagerId { get; set; }

        [JsonProperty("siteId")]
        public string SiteId { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }
    public partial class EventResponse
    {
        [JsonProperty("day")]
        public DateTime Day { get; set; }

        [JsonProperty("employeesOnLeave")]
        public EmployeesOnLeave[] EmployeesOnLeave { get; set; }
    }

    public partial class EmployeesOnLeave
    {
        [JsonProperty("empId")]
        public string EmpId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("approvedBy")]
        public string ApprovedBy { get; set; }
    }
}
