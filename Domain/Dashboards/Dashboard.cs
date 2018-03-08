using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Dashboard
  {
    public UserType DashboardType { get; set; }
    public List<Zone> Zones { get; set; }
  }
}
