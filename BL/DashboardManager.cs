using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using PB.DAL;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    public class DashboardManager : IDashboardManager
    {
        private IDashboardRepo DashboardRepo;

        private UnitOfWorkManager UowManager;

        public DashboardManager()
        {

        }

        public DashboardManager(UnitOfWorkManager uowMgr)
        {

            UowManager = uowMgr;
            DashboardRepo = new DashboardRepo(uowMgr.UnitOfWork);
        }

        public void InitNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (DashboardRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (UowManager == null)
                    {
                        UowManager = new UnitOfWorkManager();
                    }

                    DashboardRepo = new DashboardRepo(UowManager.UnitOfWork);
                }
                else
                {
                    DashboardRepo = new DashboardRepo();
                }
            }
        }

        public Dashboard AddDashboard(Subplatform subplatform, Profile profile, UserType dashboardType = UserType.HOME, List<Zone> zones = null)
        {
            Dashboard dashboard = new Dashboard()
            {
                DashboardType = dashboardType,
                Zones = zones ?? new List<Zone>(),
                Profile = profile,
                Subplatform = subplatform
            };
            subplatform.Dashboards.Add(dashboard);
            profile.Dashboards.Add(dashboard);

            dashboard = AddDashboard(dashboard);
            UowManager.Save();

            return dashboard;
        }

        private Dashboard AddDashboard(Dashboard dashboard)
        {
            return DashboardRepo.CreateDashboard(dashboard);
        }

        public List<Dashboard> AddDashboards(List<Dashboard> dashboards)
        {
            InitNonExistingRepo();
            dashboards = DashboardRepo.CreateDashboards(dashboards).ToList();
            UowManager.Save();
            return dashboards;
        }

        public void ChangeDashboard(Dashboard dashboard)
        {
            InitNonExistingRepo();
            DashboardRepo.UpdateDashboard(dashboard);
            UowManager.Save();
        }

        public IEnumerable<Dashboard> GetDashboards()
        {
            InitNonExistingRepo();
            return DashboardRepo.ReadDashboards();
        }

        public Dashboard GetDashboard(int dashboardId)
        {
            InitNonExistingRepo();
            return DashboardRepo.ReadDashboard(dashboardId);
        }

        public void RemoveDashboard(int dashboardId)
        {
            InitNonExistingRepo();
            DashboardRepo.DeleteDashboard(dashboardId);
            UowManager.Save();
        }

        public IEnumerable<Zone> GetZones()
        {
            InitNonExistingRepo();
            return DashboardRepo.ReadZones();
        }

        public Zone GetZone(int zoneId)
        {
            InitNonExistingRepo();
            return DashboardRepo.ReadZone(zoneId);
        }

        public Zone AddZone(Dashboard dashboard, string title)
        {
            InitNonExistingRepo();
            Zone zone = new Zone()
            {
                Dashboard = dashboard,
                Title = title,
                Elements = new List<Element>()
            };
            dashboard.Zones.Add(zone);

            zone = DashboardRepo.CreateZone(zone);
            UowManager.Save();

            return zone;
        }

        public Zone AddZone(Dashboard dashboard, string title, List<Element> elements)
        {
            InitNonExistingRepo();
            Zone zone = new Zone()
            {
                Dashboard = dashboard,
                Title = title,
                Elements = elements
            };
            dashboard.Zones.Add(zone);
            elements.ForEach(e => e.Zone = zone);

            zone = DashboardRepo.CreateZone(zone);
            UowManager.Save();

            return zone;
        }

        public void ChangeZone(Zone zone)
        {
            InitNonExistingRepo();
            DashboardRepo.UpdateZone(zone);
            UowManager.Save();
        }

        public void RemoveZone(int zoneId)
        {
            InitNonExistingRepo();
            Zone zone = GetZone(zoneId);
            zone.Dashboard.Zones.Remove(zone);
            DashboardRepo.DeleteZone(zoneId);
            UowManager.Save();
        }

        public IEnumerable<Element> GetElements()
        {
            InitNonExistingRepo();
            return DashboardRepo.ReadElements();
        }

        public Element GetElement(int elementId)
        {
            InitNonExistingRepo();
            return DashboardRepo.ReadElement(elementId);
        }

        public Element AddElement(Zone zone, Comparison comparison, int x, int y, int width, int height, bool isDraggable = true, bool isFinished = false)
        {
            InitNonExistingRepo();
            Element element = new Element()
            {
                Zone = zone,
                Comparison = comparison,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                IsDraggable = isDraggable,
                IsFinished = isFinished
            };
            zone.Elements.Add(element);

            element = DashboardRepo.CreateElement(element);
            UowManager.Save();

            return element;
        }

        public void ChangeElement(Element element)
        {
            InitNonExistingRepo();
            DashboardRepo.UpdateElement(element);
            UowManager.Save();
        }

        public void RemoveElement(int elementId)
        {
            InitNonExistingRepo();
            DashboardRepo.DeleteElement(elementId);
            UowManager.Save();
        }
    }
}
