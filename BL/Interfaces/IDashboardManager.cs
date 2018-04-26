using PB.BL.Domain.Accounts;
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

        IEnumerable<Zone> GetZones();
        Zone GetZone(int zoneId);
        Zone AddZone(Dashboard dashboard, string title);
        void ChangeZone(Zone zone);
        void RemoveZone(int zoneId);

        IEnumerable<Element> GetElements();
        Element GetElement(int elementId);
        Element AddElement(Zone zone, Comparison comparison, int x, int y, int width, int height, bool isDraggable);
        void ChangeElement(Element element);
        void RemoveElement(int elementId);
    }
}
