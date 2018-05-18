using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblZone")]
    public class Zone
    {
        [Key] public int ZoneId { get; set; }

        public string Title { get; set; }
        public int DashboardId { get; set; }

        public Dashboard Dashboard { get; set; }
        public List<Element> Elements { get; set; }
    }
}