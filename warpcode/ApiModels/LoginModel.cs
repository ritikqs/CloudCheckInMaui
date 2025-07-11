using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }
        
    }
    public class LoginResponse
    {
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("roleType")]
        public int RoleType { get; set; }
        [JsonProperty("token")]
        public string AccessToken { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("empId")]
        public string EmployeeId { get; set; }
        [JsonProperty("isCapture")]
        public bool IsCapture { get; set; }
    }
    public partial class User
    {
        [JsonProperty("profilePic")]
        public string ProfilePic { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonProperty("createdOn")]
        public DateTime? CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [JsonProperty("deletedOn")]
        public DateTime? DeletedOn { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public string DeletedBy { get; set; }

        [JsonProperty("motherMaidenName")]
        public string MotherMaidenName { get; set; }

        [JsonProperty("tradeID")]
        public long? TradeId { get; set; }

        [JsonProperty("employerID")]
        public long? EmployerId { get; set; }

        [JsonProperty("contractorID")]
        public long? ContractorId { get; set; }

        [JsonProperty("dob")]
        public DateTime? Dob { get; set; }

        [JsonProperty("niNumber")]
        public string NiNumber { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("loginTime")]
        public DateTime? LoginTime { get; set; }

        [JsonProperty("logoutTime")]
        public DateTime? LogoutTime { get; set; }

        [JsonProperty("azureToken")]
        public string AzureToken { get; set; }

        [JsonProperty("trade")]
        public string Trade { get; set; }

        [JsonProperty("contractor")]
        public string Contractor { get; set; }

        [JsonProperty("employer")]
        public string Employer { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("normalizedUserName")]
        public string NormalizedUserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("normalizedEmail")]
        public string NormalizedEmail { get; set; }

        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("securityStamp")]
        public string SecurityStamp { get; set; }

        [JsonProperty("concurrencyStamp")]
        public Guid ConcurrencyStamp { get; set; }

        [JsonProperty("phoneNumber")]
        public long? PhoneNumber { get; set; }

        [JsonProperty("phoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonProperty("twoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonProperty("lockoutEnd")]
        public string LockoutEnd { get; set; }

        [JsonProperty("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonProperty("accessFailedCount")]
        public long? AccessFailedCount { get; set; }
        public string EmployeeId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int? RoleType { get; set; }
    }
    public class LogoutRequest
    {

        [JsonProperty("userId")]
        public int UserId { get; set; }
    }
}
