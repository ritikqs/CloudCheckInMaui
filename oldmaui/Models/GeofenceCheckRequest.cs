using Newtonsoft.Json;

namespace CloudCheckInMaui.Models
{
    public class GeofenceCheckRequest
    {
        [JsonProperty("siteMapRefLat")]
        public string SiteMapRefLat { get; set; }

        [JsonProperty("siteMapRefLong")]
        public string SiteMapRefLong { get; set; }
    }
} 