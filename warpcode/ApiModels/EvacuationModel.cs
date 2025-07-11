using Newtonsoft.Json;
using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class CreateEvacuationRequestModel
    {
        [JsonProperty("empId")]
        public string[] EmpId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
    }
    public class EvacuationMessagesList
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("encryptId")]
        public object EncryptId { get; set; }

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
    public class EvacuationsList
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("evacuationEmployees")]
        public List<EvacuationEmployee> EvacuationEmployees { get; set; }

        [JsonProperty("encryptId")]
        public string EncryptId { get; set; }

        [JsonProperty("employee")]
        public string Employee { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonProperty("createdOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTimeOffset ModifiedOn { get; set; }

        [JsonProperty("deletedOn")]
        public object DeletedOn { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }
    }
    public partial class EvacuationEmployee
    {
        [JsonProperty("empID")]
        public Guid EmpId { get; set; }

        [JsonProperty("evacuationID")]
        public long EvacuationId { get; set; }

        [JsonProperty("isChecked")]
        public bool IsChecked { get; set; }

        [JsonProperty("employee")]
        public string Employee { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonProperty("createdOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTimeOffset ModifiedOn { get; set; }

        [JsonProperty("deletedOn")]
        public object DeletedOn { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }
    }
    public partial class EvacuationEmployeeListModel : BaseViewModel
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("encryptId")]
        public object EncryptId { get; set; }

        [JsonProperty("employee")]
        public object Employee { get; set; }
        private bool _isRead;
        [JsonProperty("isRead")]
        public bool IsRead 
        {
            get { return _isRead; }
            set { _isRead = value;OnPropertyChanged(); }
        }

        [JsonProperty("evacuationEmployees")]
        public object EvacuationEmployees { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonProperty("createdOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("modifiedOn")]
        public DateTimeOffset ModifiedOn { get; set; }

        [JsonProperty("deletedOn")]
        public object DeletedOn { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }
    }
    public class UpdateEvacuation
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("empID")]
        public string EmpId { get; set; }
    }
}
