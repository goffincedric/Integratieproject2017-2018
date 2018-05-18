using System.Collections.Generic;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;

namespace PB.BL.Interfaces
{
    public interface IDashboardManager
    {
        IEnumerable<Dashboard> GetDashboards();

        Dashboard AddDashboard(Subplatform subplatform, Profile profile, UserType dashboardType = UserType.HOME,
            List<Zone> zones = null);

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
        Element AddElement(Zone zone, int x, int y, int width, int height, bool isFinished, bool isDraggable);
        void ChangeElement(Element element);
        void RemoveElement(int elementId);
    }
}