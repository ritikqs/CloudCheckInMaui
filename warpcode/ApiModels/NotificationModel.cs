using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.ApiModels
{
    public class NotificationModel
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("contents")]
        public Contents Contents { get; set; }

        [JsonProperty("headings")]
        public Contents Headings { get; set; }

        [JsonProperty("include_player_ids")]
        public string[] IncludePlayerIds { get; set; }
       
    }
    public partial class Contents
    {
        [JsonProperty("en")]
        public string En { get; set; }
    }
}
