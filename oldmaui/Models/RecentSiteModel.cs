using System;

namespace CloudCheckInMaui.Models
{
    public partial class RecentSiteModel
    {
        public Guid SiteId { get; set; }
        public bool IsCheckIn { get; set; }
        public SiteDetails SiteDetails { get; set; }
    }

    public partial class SiteDetails
    {
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteCompany { get; set; }
        public string SiteMapRefLong { get; set; }
        public string SiteMapRefLat { get; set; }
        public object SiteDetailsSiteDetails { get; set; }
        public bool IsAreaExist { get; set; }
        public long Id { get; set; }
        public string RecordVersion { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public object DeletedOn { get; set; }
        public bool Deleted { get; set; }
        public object DeletedBy { get; set; }
    }
} 