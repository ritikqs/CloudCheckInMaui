using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class RegistrationRequestModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("zipByte")]
        public string ZipByte { get; set; }

        [JsonProperty("motherMaidenName")]
        public string MotherMaidenName { get; set; }

        [JsonProperty("tradeID")]
        public long TradeId { get; set; }
        [JsonProperty("employeeTypeID")]
        public long EmployeeTypeId { get; set; }

        [JsonProperty("contractorID")]
        public long ContractorId { get; set; }

        [JsonProperty("niNumber")]
        public string NiNumber { get; set; }

        [JsonProperty("dob")]
        public DateTime Dob { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonProperty("facepersistancyId")]
        public string FacePersistencyId { get; set; }
        [JsonProperty("otherTrade")]
        public string OtherTrade { get; set; }
        [JsonProperty("otherContractor")]
        public string OtherContractor { get; set; }
    }
    public class RetakePhotoRequest
    {
        [JsonProperty("zipByte")]
        public string ZipByte { get; set; }
        [JsonProperty("employeeId")]
        public string EmployeeId { get; set; }
        [JsonProperty("facePersistencyId")]
        public string FacePersistencyId { get; set; }
    }
    public class RegistrationResponseModel
    {
        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("profilePic")]
        public object ProfilePic { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("refreshToken")]
        public object RefreshToken { get; set; }

        [JsonProperty("motherMaidenName")]
        public string MotherMaidenName { get; set; }

        [JsonProperty("empID")]
        public Guid EmpId { get; set; }

        [JsonProperty("tradeID")]
        public long TradeId { get; set; }

        [JsonProperty("employerID")]
        public object EmployerId { get; set; }

        [JsonProperty("employeeTypeID")]
        public long EmployeeTypeId { get; set; }

        [JsonProperty("contractorID")]
        public long ContractorId { get; set; }

        [JsonProperty("dob")]
        public DateTime Dob { get; set; }

        [JsonProperty("niNumber")]
        public string NiNumber { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("loginTime")]
        public DateTime LoginTime { get; set; }

        [JsonProperty("logoutTime")]
        public object LogoutTime { get; set; }

        [JsonProperty("azureToken")]
        public Guid AzureToken { get; set; }

        [JsonProperty("loggedBy")]
        public object LoggedBy { get; set; }

        [JsonProperty("trade")]
        public object Trade { get; set; }

        [JsonProperty("contractor")]
        public object Contractor { get; set; }

        [JsonProperty("employer")]
        public object Employer { get; set; }

        [JsonProperty("employeeType")]
        public object EmployeeType { get; set; }

        [JsonProperty("encryptId")]
        public object EncryptId { get; set; }

        [JsonProperty("employeeImage")]
        public object EmployeeImage { get; set; }

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
    public class PopupPicker
    {
        public string Title { get; set; }
        public string Id { get; set; }
    }

    public class EmailExistCheck
    {
        [JsonProperty("emailOrNINumber")]
        public string EmailOrNiNumber { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }

    }
}
