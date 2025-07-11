using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class OfflineTimeSheetSyncModel
    {
        [JsonProperty("empId")]
        public string EmpId { get; set; }

        [JsonProperty("siteId")]
        public string SiteId { get; set; }

        [JsonProperty("offlineTime")]
        public OfflineTime[] OfflineTime { get; set; }
    }
    public class OfflineTime
    {
        [JsonProperty("timeInSite")]
        public DateTime TimeInSite { get; set; }

        [JsonProperty("siteMapRefLat")]
        public string SiteMapRefLat { get; set; }

        [JsonProperty("siteMapRefLong")]
        public string SiteMapRefLong { get; set; }
    }
    public class OfflineSyncResponse
    {
        [JsonProperty("checkInTime")]
        public DateTime? CheckInTime { get; set; }

        [JsonProperty("IsCheckedIn")]
        public bool IsCheckedIn { get; set; }

        [JsonProperty("isSiteChanged")]
        public bool IsSiteChanged { get; set; }
        [JsonProperty("site")]
        public SiteModel Site { get; set; }

    }
}
