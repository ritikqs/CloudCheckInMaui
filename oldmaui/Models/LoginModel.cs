using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCheckInMaui.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("deviceToken")]
        public string DeviceToken { get; set; }
    }
    public class LoginResponse
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("roleType")]
        public int RoleType { get; set; }
        [JsonPropertyName("token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("empId")]
        public string EmployeeId { get; set; }
        [JsonPropertyName("isCapture")]
        public bool IsCapture { get; set; }
    }
    public partial class User
    {
        [JsonPropertyName("profilePic")]
        public string ProfilePic { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public DateTime? DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public string DeletedBy { get; set; }

        [JsonPropertyName("motherMaidenName")]
        public string MotherMaidenName { get; set; }

        [JsonPropertyName("tradeID")]
        public long? TradeId { get; set; }

        [JsonPropertyName("employerID")]
        public long? EmployerId { get; set; }

        [JsonPropertyName("contractorID")]
        public long? ContractorId { get; set; }

        [JsonPropertyName("dob")]
        public DateTime? Dob { get; set; }

        [JsonPropertyName("niNumber")]
        public string NiNumber { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("loginTime")]
        public DateTime? LoginTime { get; set; }

        [JsonPropertyName("logoutTime")]
        public DateTime? LogoutTime { get; set; }

        [JsonPropertyName("azureToken")]
        public string AzureToken { get; set; }

        [JsonPropertyName("trade")]
        public string Trade { get; set; }

        [JsonPropertyName("contractor")]
        public string Contractor { get; set; }

        [JsonPropertyName("employer")]
        public string Employer { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("normalizedUserName")]
        public string NormalizedUserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("normalizedEmail")]
        public string NormalizedEmail { get; set; }

        [JsonPropertyName("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonPropertyName("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonPropertyName("securityStamp")]
        public string SecurityStamp { get; set; }

        [JsonPropertyName("concurrencyStamp")]
        public Guid ConcurrencyStamp { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("phoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonPropertyName("twoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonPropertyName("lockoutEnd")]
        public string LockoutEnd { get; set; }

        [JsonPropertyName("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonPropertyName("accessFailedCount")]
        public long? AccessFailedCount { get; set; }
        
        [JsonPropertyName("empID")]
        public Guid EmployeeId { get; set; }
        
        public string Token { get; set; }
        public string Role { get; set; }
        public int? RoleType { get; set; }
    }
    public class LogoutRequest
    {

        [JsonPropertyName("userId")]
        public int UserId { get; set; }
    }
} 