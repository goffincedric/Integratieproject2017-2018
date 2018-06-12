using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.DAL.EF;

namespace UI_MVC.Controllers
{
    /// <summary>
    ///     Controller that has control over the persistence of elements, dashboards and zones
    ///     Authorized by all roles
    /// </summary
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly AccountManager accountMgr;
        private readonly DashboardManager dashboardMgr;
        private readonly ItemManager itemMgr;
        private readonly SubplatformManager SubplatformMgr;
        private readonly UnitOfWorkManager uow;
        
        public DashboardController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            dashboardMgr = new DashboardManager(uow);
            accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
            SubplatformMgr = new SubplatformManager(uow);

            if (System.Web.HttpContext.Current.Request.Url.Segments.Count() > 1)
            {
                Subplatform subplatform = SubplatformMgr.GetSubplatform(System.Web.HttpContext.Current.Request.Url.Segments[1].Trim('/'));

                IEnumerable<Tag> menuTags = subplatform.Pages.SingleOrDefault(p => p.PageName.Equals("Menu"))?.Tags;
                if (menuTags is null || menuTags.Count() == 0) return;
                ViewBag.Home = menuTags.SingleOrDefault(t => t.Name.Equals("Home"))?.Text ?? "Home";
                ViewBag.Dashboard = menuTags.SingleOrDefault(t => t.Name.Equals("Dashboard"))?.Text ?? "Dashboard";
                ViewBag.WeeklyReview = menuTags.SingleOrDefault(t => t.Name.Equals("Weekly_Review"))?.Text ?? "Weekly Review";
                ViewBag.MyAccount = menuTags.SingleOrDefault(t => t.Name.Equals("Account"))?.Text ?? "Account";
                ViewBag.More = menuTags.SingleOrDefault(t => t.Name.Equals("More"))?.Text ?? "More";
                ViewBag.FAQ = menuTags.SingleOrDefault(t => t.Name.Equals("FAQ"))?.Text ?? "FAQ";
                ViewBag.Contact = menuTags.SingleOrDefault(t => t.Name.Equals("Contact"))?.Text ?? "Contact";
                ViewBag.Legal = menuTags.SingleOrDefault(t => t.Name.Equals("Legal"))?.Text ?? "Legal";
                ViewBag.Items = menuTags.SingleOrDefault(t => t.Name.Equals("Items"))?.Text ?? "Items";
                ViewBag.Persons = menuTags.SingleOrDefault(t => t.Name.Equals("Persons"))?.Text ?? "Persons";
                ViewBag.Organisations = menuTags.SingleOrDefault(t => t.Name.Equals("Organisations"))?.Text ?? "Organisations";
                ViewBag.Themes = menuTags.SingleOrDefault(t => t.Name.Equals("Themes"))?.Text ?? "Themes";
            }
        }

        #region Dashboard

        public ActionResult Dashboard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var user = accountMgr.GetProfile(User.Identity.GetUserId());

            Dashboard model = dashboardMgr.GetDashboards().FirstOrDefault(d =>
                d.Profile.Id == user.Id && d.Subplatform.URL.ToLower().Equals(subplatform.ToLower()));
            if (model == null)
            {
                model = new Dashboard
                {
                    Profile = user,
                    DashboardType = UserType.USER,
                    Subplatform = Subplatform,
                    Zones = new List<Zone>()
                };
                model = dashboardMgr.AddDashboard(model.Subplatform, model.Profile, model.DashboardType, model.Zones);
            }

            ;
            return View(model);
        }

        #endregion

        #region Wizard

        public ActionResult Wizard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            return View();
        }

        public ActionResult _Wizard()
        {
            return PartialView();
        }

        #endregion
    }
}