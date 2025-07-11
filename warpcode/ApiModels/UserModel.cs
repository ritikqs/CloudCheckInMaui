using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class UserDetailRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
    public class UserDetails
    {
        [JsonProperty("empId")]
        public string EmpID { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("firstMidName")]
        public string FirstMidName { get; set; }
        [JsonProperty("joinDate")]
        public DateTime JoinDate { get; set; }
        [JsonProperty("voiceRecKey")]
        public string VoiceRecKey { get; set; }
        [JsonProperty("empNINumber")]
        public string EmpNINumber { get; set; }
        [JsonProperty("empVoiceEnroll")]
        public int EmpVoiceEnroll { get; set; }
        [JsonProperty("empFaceEnroll")]
        public int EmpFaceEnroll { get; set; }
        [JsonProperty("empThumbEnroll")]
        public int EmpThumbEnroll { get; set; }
        [JsonProperty("empRifdEnroll")]
        public int EmpRifdEnroll { get; set; }
        [JsonProperty("picturesTakenCount")]
        public int PicturesTakenCount { get; set; }
        [JsonProperty("employeeID")]
        public string EmployeeID { get; set; }
        [JsonProperty("portalId")]
        public string PortalId { get; set; }
        [JsonProperty("portal")]
        public object Portal { get; set; }
        [JsonProperty("voiceVerified")]
        public bool VoiceVerified { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
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
        public string DeletedBy { get; set; }
    }
}
