using PB.BL.Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL
{
    public interface IDashboardRepo
    {
        IEnumerable<Dashboard> ReadDashboards();
        Dashboard CreateDashboard(Dashboard dashboard);
        IEnumerable<Dashboard> CreateDashboards(List<Dashboard> dashboards);
        Dashboard ReadDashboard(int dashboardId);
        void UpdateDashboard(Dashboard dashboard);
        void DeleteDashboard(int dashboardId);
    }
}
