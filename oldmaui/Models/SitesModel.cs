using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CloudCheckInMaui.Models
{
    public partial class SiteModel
    {
        [JsonProperty("siteID")]
        public Guid SiteId { get; set; }

        [JsonProperty("siteName")]
        public string SiteName { get; set; }

        [JsonProperty("siteCompany")]
        public string SiteCompany { get; set; }

        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("town")]
        public string Town { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("siteMapRefLong")]
        public string SiteMapRefLong { get; set; }

        [JsonProperty("siteMapRefLat")]
        public string SiteMapRefLat { get; set; }

        [JsonProperty("mobileNetwork")]
        public object MobileNetwork { get; set; }

        [JsonProperty("portalId")]
        public Guid PortalId { get; set; }

        [JsonProperty("portal")]
        public object Portal { get; set; }

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
    
    public class SiteListModel
    {
        [JsonProperty("timeOnSite")]
        public DateTime TimeOnSite { get; set; }

        [JsonProperty("timeOffSite")]
        public DateTime? TimeOffSite { get; set; }
            
        [JsonProperty("nonAuthTimeOffSite")]
        public DateTime? NonAutTimeOffSite { get; set; }
            
        [JsonProperty("site")]
        public SiteData Site { get; set; }
    }
    
    public class SiteData
    {
        [JsonProperty("siteName")]
        public string SiteName { get; set; }
    }
} 