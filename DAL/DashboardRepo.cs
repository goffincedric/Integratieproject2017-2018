using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PB.BL.Domain.Dashboards;
using PB.DAL.EF;

namespace PB.DAL
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly IntegratieDbContext ctx;

        public DashboardRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public DashboardRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public IEnumerable<Dashboard> CreateDashboards(List<Dashboard> dashboards)
        {
            ctx.Dashboards.AddRange(dashboards);
            ctx.SaveChanges();
            return dashboards;
        }

        public Dashboard CreateDashboard(Dashboard dashboard)
        {
            ctx.Dashboards.Add(dashboard);
            ctx.SaveChanges();
            return dashboard;
        }

        public void DeleteDashboard(int dashboardId)
        {
            Dashboard dashboard = ReadDashboard(dashboardId);
            ctx.Dashboards.Remove(dashboard);
            ctx.SaveChanges();
        }

        public Dashboard ReadDashboard(int dashboardId)
        {
            return ctx.Dashboards
                .Include(d => d.Zones)
                .Include(d => d.Profile)
                .Include(d => d.Subplatform)
                .FirstOrDefault(d => d.DashboardId == dashboardId);
        }

        public IEnumerable<Dashboard> ReadDashboards()
        {
            return ctx.Dashboards
                .Include(d => d.Zones)
                .Include(d => d.Profile)
                .Include(d => d.Subplatform)
                .AsEnumerable();
        }

        public void UpdateDashboard(Dashboard dashboard)
        {
            dashboard = ctx.Dashboards.Attach(dashboard);
            ctx.Entry(dashboard).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public Zone CreateZone(Zone zone)
        {
            ctx.Zones.Add(zone);
            ctx.SaveChanges();
            return zone;
        }

        public IEnumerable<Zone> ReadZones()
        {
            return ctx.Zones
                .Include(z => z.Dashboard)
                .Include(z => z.Elements)
                .AsEnumerable();
        }

        public Zone ReadZone(int zoneId)
        {
            return ctx.Zones
                .Include(z => z.Dashboard)
                .Include(z => z.Elements)
                .FirstOrDefault(z => z.ZoneId == zoneId);
        }

        public void UpdateZone(Zone zone)
        {
            zone = ctx.Zones.Attach(zone);
            ctx.Entry(zone).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteZone(int zoneId)
        {
            Zone zone = ReadZone(zoneId);
            ctx.Zones.Remove(zone);
            ctx.SaveChanges();
        }

        public Element CreateElement(Element element)
        {
            ctx.Elements.Add(element);
            ctx.SaveChanges();
            return element;
        }

        public IEnumerable<Element> ReadElements()
        {
            return ctx.Elements
                .Include(e => e.Zone)
                .AsEnumerable();
        }

        public Element ReadElement(int elementId)
        {
            return ctx.Elements
                .Include(e => e.Zone)
                .FirstOrDefault(e => e.ElementId == elementId);
        }

        public void UpdateElement(Element element)
        {
            element = ctx.Elements.Attach(element);
            ctx.Entry(element).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteElement(int elementId)
        {
            Element element = ReadElement(elementId);
            ctx.Elements.Remove(element);
            ctx.SaveChanges();
        }
    }
}