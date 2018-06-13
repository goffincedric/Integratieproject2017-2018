using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;

namespace UI_MVC.Controllers
{
    /// <summary>
    ///     Controller that has control over the persistence of elements, dashboards and zones
    ///     Authorized by all roles
    /// </summary

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

                string color1 = subplatform.Settings.Find(ss => ss.SettingName.Equals(Setting.Platform.PRIMARY_COLOR))?.Value;
                string color2 = subplatform.Settings.Find(ss => ss.SettingName.Equals(Setting.Platform.SECONDARY_COLOR))?.Value;
                Color colorObj1 = ColorTranslator.FromHtml(color1);
                Color colorObj2 = ColorTranslator.FromHtml(color2);
                ViewBag.Color1HEX = color1;
                ViewBag.Color2HEX = color2;
                ViewBag.Color1RGBA = "rgba(" + Math.Round(colorObj1.R * 0.425) + "," + Math.Round(colorObj1.G * 0.425) + "," + Math.Round(colorObj1.B * 0.425) + "," + "0.125)";
                ViewBag.Color2RGBA = "rgba(" + Math.Round(colorObj2.R * 0.525) + "," + Math.Round(colorObj2.G * 0.525) + "," + Math.Round(colorObj2.B * 0.525) + "," + "0.25)";
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