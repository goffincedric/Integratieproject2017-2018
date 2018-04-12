using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Dashboards;
using PB.DAL.EF;

namespace PB.DAL
{
    public class DashboardRepo : IDashboardRepo
    {
        private IntegratieDbContext ctx;

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
                .Include("Zones")
                .FirstOrDefault(d => d.DashboardId == dashboardId);
        }

        public IEnumerable<Dashboard> ReadDashboards()
        {
            return ctx.Dashboards
                .Include("Zones")
                .AsEnumerable();
        }

        public void UpdateDashboard(Dashboard dashboard)
        {
            ctx.Dashboards.Attach(dashboard);
            ctx.SaveChanges();
        }
    }
}
