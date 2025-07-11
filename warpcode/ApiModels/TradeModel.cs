using Newtonsoft.Json;
using CCIMIGRATION.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class TradeModel : BaseViewModel
    {
        private string _title;
        [JsonProperty("title")]
        public string Title { get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

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

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedBy")]
        public object DeletedBy { get; set; }
    }
}
