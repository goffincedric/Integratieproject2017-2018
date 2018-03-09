using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Dashboards
{
  public class Dashboard
  {
    [Key]
    public int DashboardId { get; set; }
    public UserType DashboardType { get; set; }
    public List<Zone> Zones { get; set; }
  }
}
