using PB.BL.Domain.Dashboards;
using System.Collections.Generic;

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


        Zone CreateZone(Zone zone);
        IEnumerable<Zone> ReadZones();
        Zone ReadZone(int zoneId);
        void UpdateZone(Zone zone);
        void DeleteZone(int zoneId);


        Element CreateElement(Element element);
        IEnumerable<Element> ReadElements();
        Element ReadElement(int elementId);
        void UpdateElement(Element element);
        void DeleteElement(int elementId);
    }
}
