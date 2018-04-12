﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.DAL;

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

        public Dashboard AddDashboard(Subplatform subplatform, Profile profile, UserType dashboardType = UserType.HOME)
        {
            Dashboard dashboard = new Dashboard()
            {
                DashboardType = dashboardType,
                Zones = new List<Zone>(),
                Profile = profile,
                Subplatform = subplatform
            };

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
    }
}