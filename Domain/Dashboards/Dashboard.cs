using PB.BL.Domain.Account;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblDashboard")]
    public class Dashboard
    {
        [Key]
        public int DashboardId { get; set; }
        public UserType DashboardType { get; set; }
        public List<Zone> Zones { get; set; }
        
        public Profile Profile { get; set; }
        public Subplatform Subplatform { get; set; }
    }
}
