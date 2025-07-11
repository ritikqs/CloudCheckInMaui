using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class LocationModel : BaseEntity
    {
        public DateTime TimeStamp { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class BaseEntity
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime? SyncDate { get; set; }
    }
}
