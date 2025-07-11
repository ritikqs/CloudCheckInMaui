using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models.SiteManagerModel
{
    public class SiteManagerHolidayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartHolidayDate { get; set; }
        public string EndHolidayDate { get; set; }
    }
    public class SiteManagerHolidayListRequest
    {
        [JsonProperty("draw")]
        public long Draw { get; set; }

        [JsonProperty("columns")]
        public SiteManagerHolidayColumn[] Columns { get; set; }

        [JsonProperty("order")]
        public SiteManagerHolidayOrder[] Order { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("search")]
        public SiteManagerHolidaySearch Search { get; set; }

        [JsonProperty("empId")]
        public string EmpId { get; set; }
        [JsonProperty("siteId")]
        public string SiteId { get; set; }

    }
    public partial class SiteManagerHolidayColumn
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("searchable")]
        public bool Searchable { get; set; }

        [JsonProperty("orderable")]
        public bool Orderable { get; set; }

        [JsonProperty("search")]
        public SiteManagerHolidaySearch Search { get; set; }
    }
    public partial class SiteManagerHolidaySearch
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("isRegex")]
        public bool IsRegex { get; set; }
    }

    public partial class SiteManagerHolidayOrder
    {
        [JsonProperty("column")]
        public long Column { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }
    }
}
