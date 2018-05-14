using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    /// <summary>   
    /// Controller that has control over the persistence of elements, dashboards and zones
    /// Authorized by all roles
    /// </summary
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly UnitOfWorkManager uow;
        private readonly ItemManager itemMgr;
        private readonly DashboardManager dashboardMgr;
        private readonly AccountManager accountMgr;
        private readonly SubplatformManager SubplatformMgr;


        public DashboardController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            dashboardMgr = new DashboardManager(uow);
            accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
            SubplatformMgr = new SubplatformManager(uow);
            ViewBag.Home = SubplatformMgr.GetTag("Home").Text;
            ViewBag.Dashboard = SubplatformMgr.GetTag("Dashboard").Text;
            ViewBag.WeeklyReview = SubplatformMgr.GetTag("Weekly_Review").Text;
            ViewBag.MyAccount = SubplatformMgr.GetTag("Account").Text;
            ViewBag.More = SubplatformMgr.GetTag("More").Text;
            ViewBag.FAQ = SubplatformMgr.GetTag("FAQ").Text;
            ViewBag.Contact = SubplatformMgr.GetTag("Contact").Text;
            ViewBag.Legal = SubplatformMgr.GetTag("Legal").Text;
        }

        public ActionResult Dashboard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var user = accountMgr.GetProfile(User.Identity.GetUserId());

            Dashboard model = dashboardMgr.GetDashboards().FirstOrDefault(d => d.Profile.Id == user.Id && d.Subplatform.URL.ToLower().Equals(subplatform.ToLower()));
            if (model == null)
            {
                model = new Dashboard()
                {
                    Profile = user,
                    DashboardType = UserType.USER,
                    Subplatform = Subplatform,
                    Zones = new List<Zone>()
                };
                model = dashboardMgr.AddDashboard(model.Subplatform, model.Profile, model.DashboardType, model.Zones);
            };

            return View(model);
        }


        public ActionResult Wizard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            return View();
        }

        public ActionResult _Wizard()
        {
            return PartialView();
        }


       



       
    }
}