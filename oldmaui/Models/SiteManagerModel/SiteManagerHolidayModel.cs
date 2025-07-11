using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CloudCheckInMaui.Models.SiteManagerModel
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
        [JsonPropertyName("draw")]
        public long Draw { get; set; }

        [JsonPropertyName("columns")]
        public SiteManagerHolidayColumn[] Columns { get; set; }

        [JsonPropertyName("order")]
        public SiteManagerHolidayOrder[] Order { get; set; }

        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("length")]
        public long Length { get; set; }

        [JsonPropertyName("search")]
        public SiteManagerHolidaySearch Search { get; set; }

        [JsonPropertyName("empId")]
        public string EmpId { get; set; }
        
        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }
    }
    
    public partial class SiteManagerHolidayColumn
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }

        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }

        [JsonPropertyName("search")]
        public SiteManagerHolidaySearch Search { get; set; }
    }
    
    public partial class SiteManagerHolidaySearch
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("isRegex")]
        public bool IsRegex { get; set; }
    }

    public partial class SiteManagerHolidayOrder
    {
        [JsonPropertyName("column")]
        public long Column { get; set; }

        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }
} 