using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class GeofenceCheckRequest
    {
        [JsonProperty("siteMapRefLong")]
        public string SiteMapRefLong { get; set; }

        [JsonProperty("siteMapRefLat")]
        public string SiteMapRefLat { get; set; }
    }

    public class LocationTrackTimeResponse
    {
            [JsonProperty("faceCaptureStartMessage")]
            public string FaceCaptureStartMessage { get; set; }

            [JsonProperty("faceCaptureForSeconds")]
            public long FaceCaptureForSeconds { get; set; }

            [JsonProperty("faceCaptureSavePath")]
            public string FaceCaptureSavePath { get; set; }

            [JsonProperty("employeeStartId")]
            public string EmployeeStartId { get; set; }

            [JsonProperty("imageCaptureCount")]
            public int ImageCaptureCount { get; set; }

            [JsonProperty("locationTrackTime")]
            public string LocationTrackTime { get; set; }

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
}
