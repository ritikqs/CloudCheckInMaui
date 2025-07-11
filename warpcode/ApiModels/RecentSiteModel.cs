using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.ApiModels
{
    public partial class RecentSiteModel
    {
        [JsonProperty("siteId")]
        public Guid SiteId { get; set; }

        [JsonProperty("isCheckIn")]
        public bool IsCheckIn { get; set; }

        [JsonProperty("siteDetails")]
        public SiteDetails SiteDetails { get; set; }
    }

    public partial class SiteDetails
    {
        [JsonProperty("siteID")]
        public Guid SiteId { get; set; }

        [JsonProperty("siteName")]
        public string SiteName { get; set; }

        [JsonProperty("siteCompany")]
        public string SiteCompany { get; set; }

        [JsonProperty("siteMapRefLong")]
        public string SiteMapRefLong { get; set; }

        [JsonProperty("siteMapRefLat")]
        public string SiteMapRefLat { get; set; }

        [JsonProperty("siteDetails")]
        public object SiteDetailsSiteDetails { get; set; }

        [JsonProperty("isAreaExist")]
        public bool IsAreaExist { get; set; }

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
}
