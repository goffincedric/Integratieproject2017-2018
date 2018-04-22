using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using System.Collections.Generic;

namespace PB.BL.Interfaces
{
    public interface IDashboardManager
    {
        IEnumerable<Dashboard> GetDashboards();
        Dashboard AddDashboard(Subplatform subplatform, Profile profile, UserType dashboardType = UserType.HOME);
        List<Dashboard> AddDashboards(List<Dashboard> dashboards);
        Dashboard GetDashboard(int dashboardId);
        void ChangeDashboard(Dashboard dashboard);
        void RemoveDashboard(int dashboardId);

        //void AddZone(int dashboardId, string Name, bool IsFull, int Width, int Height, int Position);
        //void RemoveZone(int zoneId);
    }
}
