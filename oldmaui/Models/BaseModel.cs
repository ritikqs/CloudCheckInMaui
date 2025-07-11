using System;
using System.Text.Json.Serialization;

namespace CloudCheckInMaui.Models
{
    public abstract class BaseModel
    {
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
} 