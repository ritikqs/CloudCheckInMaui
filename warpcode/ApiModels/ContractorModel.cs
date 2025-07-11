using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class ContractorModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("recordVersion")]
        public string RecordVersion { get; set; }
        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; set; }
        [JsonProperty("deletedOn")]
        public object DeletedOn { get; set; }
        [JsonProperty("Deleted")]
        public bool Deleted { get; set; }
        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }

    }
}
