using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models.SiteManagerModel
{

    public partial class SiteStaffResponse
    {
        [JsonProperty("siteName")]
        public string SiteName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("employeeID")]
        public string EmployeeId { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("niNumber")]
        public string NiNumber { get; set; }

        [JsonProperty("contractor")]
        public object Contractor { get; set; }

        [JsonProperty("trade")]
        public string Trade { get; set; }

        [JsonProperty("attendanceDate")]
        public string AttendanceDate { get; set; }

        [JsonProperty("signInTime")]
        public DateTime SignInTime { get; set; }

        [JsonProperty("signOutTime")]
        public string SignOutTime { get; set; }

        [JsonProperty("signInImage")]
        public Uri SignInImage { get; set; }

        [JsonProperty("signOutImage")]
        public object SignOutImage { get; set; }

        [JsonProperty("timeOnSite")]
        public DateTime TimeOnSite { get; set; }

        [JsonProperty("timeOffSite")]
        public object TimeOffSite { get; set; }
    }
    public class SiteStaffModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Trade { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }

    }
    public class StaffModelResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<StaffModelList.Result> result { get; set; }
    }
   public class StaffModelList
    {
        public class Result
        {
            public string empID { get; set; }
            public string siteID { get; set; }
            public DateTime? timeOnSite { get; set; }
            public DateTime? timeOffSite { get; set; }
            public string checkInImage { get; set; }
            public string checkOutImage { get; set; }
            public Employee employee { get; set; }
            public Site site { get; set; }
            public string encryptId { get; set; }
            public string timeOnDate { get; set; }
            public string timeOffDate { get; set; }
            public string id { get; set; }
            public string recordVersion { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string deletedOn { get; set; }
            public bool deleted { get; set; }
            public string deletedBy { get; set; }
        }
        public class Site
        {
            public string siteID { get; set; }
            public string siteName { get; set; }
            public string siteCompany { get; set; }
            public string siteMapRefLong { get; set; }
            public string siteMapRefLat { get; set; }
            public string siteDetails { get; set; }
            public int id { get; set; }
            public string recordVersion { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string deletedOn { get; set; }
            public bool deleted { get; set; }
            public string deletedBy { get; set; }
        }
        public class Employee
        {
            public string empID { get; set; }
            public string lastName { get; set; }
            public string firstMidName { get; set; }
            public DateTime joinDate { get; set; }
            public string voiceRecKey { get; set; }
            public string empNINumber { get; set; }
            public int empVoiceEnroll { get; set; }
            public int empFaceEnroll { get; set; }
            public int empThumbEnroll { get; set; }
            public int empRifdEnroll { get; set; }
            public int picturesTakenCount { get; set; }
            public string employeeID { get; set; }
            public string employeeCaptureImage { get; set; }
            public EmployeeUser employeeUser { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string email { get; set; }
            public bool isEmailLinked { get; set; }
            public bool voiceVerified { get; set; }
            public int id { get; set; }
            public string recordVersion { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string deletedOn { get; set; }
            public bool deleted { get; set; }
            public string deletedBy { get; set; }
        }
        public class EmployeeUser
        {
            public bool emailConfirmed { get; set; }
            public string userName { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string profilePic { get; set; }
            public string phoneNumber { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string refreshToken { get; set; }
            public string motherMaidenName { get; set; }
            public string empID { get; set; }
            public int tradeID { get; set; }
            public int? employerID { get; set; }
            public int contractorID { get; set; }
            public DateTime dob { get; set; }
            public string niNumber { get; set; }
            public bool isActive { get; set; }
            public DateTime loginTime { get; set; }
            public string logoutTime { get; set; }
            public string azureToken { get; set; }
            public string loggedBy { get; set; }
            public Trade trade { get; set; }
            public string contractor { get; set; }
            public string employer { get; set; }
            public string encryptId { get; set; }
            public int id { get; set; }
            public string recordVersion { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string deletedOn { get; set; }
            public bool deleted { get; set; }
            public string deletedBy { get; set; }
        }
        public class Trade
        {
            public string title { get; set; }
            public int id { get; set; }
            public string recordVersion { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string deletedOn { get; set; }
            public bool deleted { get; set; }
            public string deletedBy { get; set; }
        }
    }
    public class StaffModel
    {
        public class GetStaffOnSiteModel
        {
            public List<Column> columns { get; set; }
            public List<Order> order { get; set; }
            public int start { get; set; }
            public int length { get; set; }
            public Search search { get; set; }
            public string empId { get; set; }
            public string siteId { get; set; }
            public string portalId { get; set; }
            public int draw { get; set; }
        }
        public class Search
        {
            public string value { get; set; }
            public bool isRegex { get; set; }
        }

        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }
        public class Order
        {
            public int column { get; set; }
            public string dir { get; set; }
        }
    }
   
}
