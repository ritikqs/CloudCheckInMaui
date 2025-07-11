using System;
using System.Text.Json.Serialization;

namespace CloudCheckInMaui.Models
{
    public class AlertModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("employeeId")]
        public int EmployeeId { get; set; }
    }
} 