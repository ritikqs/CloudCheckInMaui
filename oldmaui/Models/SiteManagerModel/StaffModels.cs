using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CloudCheckInMaui.Models.SiteManagerModel
{
    public class StaffResponse
    {
        [JsonPropertyName("siteName")]
        public string SiteName { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("employeeID")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("niNumber")]
        public string NiNumber { get; set; }

        [JsonPropertyName("contractor")]
        public object Contractor { get; set; }

        [JsonPropertyName("trade")]
        public string Trade { get; set; }

        [JsonPropertyName("attendanceDate")]
        public string AttendanceDate { get; set; }

        [JsonPropertyName("signInTime")]
        public DateTime SignInTime { get; set; }

        [JsonPropertyName("signOutTime")]
        public string SignOutTime { get; set; }

        [JsonPropertyName("signInImage")]
        public Uri SignInImage { get; set; }

        [JsonPropertyName("signOutImage")]
        public object SignOutImage { get; set; }

        [JsonPropertyName("timeOnSite")]
        public DateTime TimeOnSite { get; set; }

        [JsonPropertyName("timeOffSite")]
        public object TimeOffSite { get; set; }
    }

    public class StaffModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Trade { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
    }

    public class StaffListRequest
    {
        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("columns")]
        public List<Column> Columns { get; set; }

        [JsonPropertyName("order")]
        public List<Order> Order { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("search")]
        public Search Search { get; set; }

        [JsonPropertyName("empId")]
        public string EmpId { get; set; }

        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }

        [JsonPropertyName("portalId")]
        public string PortalId { get; set; }
    }

    public class Search
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("isRegex")]
        public bool IsRegex { get; set; }
    }

    public class Column
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }

        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }

        [JsonPropertyName("search")]
        public Search Search { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }

        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }

    public class Employee
    {
        [JsonPropertyName("empID")]
        public string EmpId { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("firstMidName")]
        public string FirstMidName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("joinDate")]
        public DateTime JoinDate { get; set; }

        [JsonPropertyName("voiceRecKey")]
        public string VoiceRecKey { get; set; }

        [JsonPropertyName("empNINumber")]
        public string EmpNINumber { get; set; }

        [JsonPropertyName("empVoiceEnroll")]
        public int EmpVoiceEnroll { get; set; }

        [JsonPropertyName("empFaceEnroll")]
        public int EmpFaceEnroll { get; set; }

        [JsonPropertyName("empThumbEnroll")]
        public int EmpThumbEnroll { get; set; }

        [JsonPropertyName("empRifdEnroll")]
        public int EmpRifdEnroll { get; set; }

        [JsonPropertyName("picturesTakenCount")]
        public int PicturesTakenCount { get; set; }

        [JsonPropertyName("employeeID")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("employeeCaptureImage")]
        public string EmployeeCaptureImage { get; set; }

        [JsonPropertyName("employeeUser")]
        public EmployeeUser EmployeeUser { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("isEmailLinked")]
        public bool IsEmailLinked { get; set; }

        [JsonPropertyName("voiceVerified")]
        public bool VoiceVerified { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public string DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public string DeletedBy { get; set; }
    }

    public class EmployeeUser
    {
        [JsonPropertyName("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("profilePic")]
        public string ProfilePic { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("motherMaidenName")]
        public string MotherMaidenName { get; set; }

        [JsonPropertyName("empID")]
        public string EmpId { get; set; }

        [JsonPropertyName("tradeID")]
        public int TradeId { get; set; }

        [JsonPropertyName("employerID")]
        public int? EmployerId { get; set; }

        [JsonPropertyName("contractorID")]
        public int ContractorId { get; set; }

        [JsonPropertyName("dob")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("niNumber")]
        public string NiNumber { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("loginTime")]
        public DateTime LoginTime { get; set; }

        [JsonPropertyName("logoutTime")]
        public string LogoutTime { get; set; }

        [JsonPropertyName("azureToken")]
        public string AzureToken { get; set; }

        [JsonPropertyName("loggedBy")]
        public string LoggedBy { get; set; }

        [JsonPropertyName("trade")]
        public Trade Trade { get; set; }

        [JsonPropertyName("contractor")]
        public string Contractor { get; set; }

        [JsonPropertyName("employer")]
        public string Employer { get; set; }

        [JsonPropertyName("encryptId")]
        public string EncryptId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public string DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public string DeletedBy { get; set; }
    }

    public class Trade
    {
        [JsonPropertyName("tradeName")]
        public string TradeName { get; set; }

        [JsonPropertyName("encryptId")]
        public string EncryptId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public string DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public string DeletedBy { get; set; }
    }
} 