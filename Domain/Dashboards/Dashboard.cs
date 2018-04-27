using PB.BL.Domain.Accounts;
using PB.BL.Domain.Platform;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblDashboard")]
    public class Dashboard
    {
        [Key]
        public int DashboardId { get; set; }
        public UserType DashboardType { get; set; }
        public List<Zone> Zones { get; set; }
        public string UserId { get; set; }
        public int? SubplatformId { get; set; } // TODO: int? => int
        
        public Profile Profile { get; set; }
        public Subplatform Subplatform { get; set; }
    }
}
