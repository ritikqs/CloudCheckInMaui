using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CloudCheckInMaui.Models
{
    public class LocationTrackTimeResponse
    {
        [JsonPropertyName("faceCaptureStartMessage")]
        public string FaceCaptureStartMessage { get; set; }

        [JsonPropertyName("faceCaptureForSeconds")]
        public long FaceCaptureForSeconds { get; set; }

        [JsonPropertyName("faceCaptureSavePath")]
        public string FaceCaptureSavePath { get; set; }

        [JsonPropertyName("employeeStartId")]
        public string EmployeeStartId { get; set; }

        [JsonPropertyName("imageCaptureCount")]
        public int ImageCaptureCount { get; set; }

        [JsonPropertyName("locationTrackTime")]
        public string LocationTrackTime { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("deletedOn")]
        public object DeletedOn { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("deletedBy")]
        public object DeletedBy { get; set; }
    }
} 