using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using CloudCheckInMaui.ViewModels;

namespace CloudCheckInMaui.Models
{
    public class CreateEvacuationRequestModel
    {
        [Required]
        [JsonPropertyName("empId")]
        public string[]? EmpId { get; set; }

        [Required]
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        
        [JsonPropertyName("image")]
        public string? Image { get; set; }
    }
    
    public class EvacuationMessagesList : BaseModel
    {
        [Required]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("encryptId")]
        public string? EncryptId { get; set; }
    }
    
    public class EvacuationsList : BaseModel
    {
        [Required]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }
        
        [JsonPropertyName("evacuationEmployees")]
        public List<EvacuationEmployee>? EvacuationEmployees { get; set; }

        [JsonPropertyName("encryptId")]
        public string? EncryptId { get; set; }

        [JsonPropertyName("employee")]
        public string? Employee { get; set; }
    }
    
    public partial class EvacuationEmployee : BaseModel
    {
        [Required]
        [JsonPropertyName("empID")]
        public Guid EmpId { get; set; }

        [Required]
        [JsonPropertyName("evacuationID")]
        public long EvacuationId { get; set; }

        [JsonPropertyName("isChecked")]
        public bool IsChecked { get; set; }

        [JsonPropertyName("employee")]
        public string? Employee { get; set; }
    }
    
    public partial class EvacuationEmployeeListModel : BaseViewModel
    {
        [Required]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("encryptId")]
        public string? EncryptId { get; set; }

        [JsonPropertyName("employee")]
        public object? Employee { get; set; }
        
        private bool _isRead;
        [JsonPropertyName("isRead")]
        public bool IsRead 
        {
            get => _isRead;
            set 
            { 
                _isRead = value;
                OnPropertyChanged(); 
            }
        }

        [JsonPropertyName("evacuationEmployees")]
        public object? EvacuationEmployees { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("recordVersion")]
        public string? RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTimeOffset ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public object? DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public object? DeletedBy { get; set; }
    }
    
    public class UpdateEvacuation
    {
        [Required]
        [JsonPropertyName("id")]
        public long Id { get; set; }
        
        [Required]
        [JsonPropertyName("empID")]
        public string? EmpId { get; set; }
    }
} 