using System;
using Newtonsoft.Json;

namespace CCIMIGRATION.ApiModels
{
    public class FailedLoginUpdateRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("zipByte")]
        public string ZipByte { get; set; }
    }
}
