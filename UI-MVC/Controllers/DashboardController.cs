using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private readonly ItemManager itemMgr = new ItemManager(uow);
        private readonly DashboardManager dashboardMgr = new DashboardManager(uow);
        private readonly AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);

        public ActionResult Dashboard()
        {
            Dashboard model = new Dashboard();

            var user = accountMgr.GetProfile(User.Identity.GetUserId());

            //add subplatform later
            model = dashboardMgr.GetDashboards().ToList().FirstOrDefault(d => d.Profile.Id == user.Id);

            return View(model);
        }
    }
}