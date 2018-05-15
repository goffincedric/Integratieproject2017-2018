using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI_MVC.Controllers
{
    /// <summary>   
    /// This controller rules over homepage and page that are an addition to the website or where it was not clear where to put a certain method. 
    /// Authorization depends on used method 
    /// </summary
    [RequireHttps]
    [RoutePrefix("{subplatform}")]
    [OutputCache(Duration = 10, VaryByParam = "none")]
    public class HomeController : Controller
    {
        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private readonly ItemManager itemMgr = new ItemManager(uow);
        private readonly AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
        private readonly SubplatformManager SubplatformMgr = new SubplatformManager(uow);

        public HomeController()
        {
            ViewBag.Home = SubplatformMgr.GetTag("Home").Text;
            ViewBag.Dashboard = SubplatformMgr.GetTag("Dashboard").Text;
            ViewBag.WeeklyReview = SubplatformMgr.GetTag("Weekly_Review").Text;
            ViewBag.MyAccount = SubplatformMgr.GetTag("Account").Text;
            ViewBag.More = SubplatformMgr.GetTag("More").Text;
            ViewBag.FAQ = SubplatformMgr.GetTag("FAQ").Text;
            ViewBag.Contact = SubplatformMgr.GetTag("Contact").Text;
            ViewBag.Legal = SubplatformMgr.GetTag("Legal").Text;
        }

        #region Load

        public ActionResult ChangeProfilePic()
        {
            Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
            var image = VirtualPathUtility.ToAbsolute(profile.ProfileIcon);
            return Content(image);
        }

        public ActionResult ChangeLogoutin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("Login/Register");
            }
            else
            {
                return Content("Logout");
            }
        }

        public ActionResult LinkLogoutin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("/Account/Login");
            }
            else
            {
                RedirectToAction("Logoff", "Account");

                return Content("");
            }
        }

        public ActionResult GetName(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            string name = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME).Value;


            return Content(name);
        }

        public ActionResult GetLogo(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            string url = VirtualPathUtility.ToAbsolute(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Value);
            return Content(url);
        }

        public ActionResult LoadDefaults(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            ViewBag.Logo = VirtualPathUtility.ToAbsolute(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Value);
            ViewBag.SiteName = Content(ViewBag.SiteName = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME).Value);
            return Content("");
        }

        public ActionResult GetThemeSetting()
        {
            if (User.Identity.IsAuthenticated)
            {
                string theme = "";
                Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
                UserSetting userSetting = accountMgr.GetUserSetting(profile.Id, Setting.Account.THEME);

                switch (userSetting.Value)
                {
                    case "Light": theme = "LightMode"; break;
                    case "Dark": theme = "DarkMode"; break;
                    case "Future": theme = "FutureMode"; break;
                }
                return Content(@"/Content/Theme/" + theme + @".css");
            }
            return Content("/Content/Theme/LightMode.css");
        }

        public ActionResult ChangeThemeSetting(string Theme)
        {
            if (User.Identity.IsAuthenticated)
            {
                Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
                UserSetting userSetting = accountMgr.GetUserSetting(profile.Id, Setting.Account.THEME);
                userSetting.Value = Theme;
                accountMgr.ChangeUserSetting(profile.Id, userSetting);
                return View("Index");
            }
            return View("Index");
        }
        #endregion

        #region Index
        [Route("~/")]
        public ActionResult Index2()
        {
            return RedirectToAction("Index", "Home", new { subplatform = "politieke-barometer" });
        }

        [Route("")]
        public ActionResult Index(string subplatform)
        {
            ViewBag.HeaderText = SubplatformMgr.GetTag("BannerTitle").Text;
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View("Index");
        }

        #endregion

        #region BasicPages
        public ActionResult Contact(string subplatform)
        {
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View();
        }

        public ActionResult FAQ(string subplatform)
        {
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View();
        }

        public ActionResult Legal(string subplatform)
        {
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View();
        }
        #endregion

        #region Search
        public ActionResult _Search(string subplatform)
        {
            Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Item> items = itemMgr.GetItems().Where(i => i.SubPlatforms.Contains(sp));
            return PartialView(items);
        }
        #endregion

        #region ItemDetail
        public ActionResult ItemDetail(string subplatform, int id)
        {
            Item item = itemMgr.GetItem(id);
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (!item.SubPlatforms.Contains(Subplatform)) return HttpNotFound();
            ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);

            if (item is Person person)
            {
                int? count = person.Records.Count();
                ViewBag.Vermeldingen = (count is null) ? 0 : count;
                ViewBag.Partij = (person.Organisation is null) ? "Geen partij" : person.Organisation.Name;
            }
            if (item is Organisation organisation)
            {
                int? count = organisation.People.Count();
                ViewBag.Leden = (count is null) ? 0 : count;
                ViewBag.FullName = organisation.FullName;
            }
            if (item is Theme theme)
            {
                int? count = theme.Records.Count();
                ViewBag.Associaties = (count is null) ? 0 : count;
            }
            ViewBag.Subscribed = item.SubscribedProfiles.Contains(accountMgr.GetProfile(User.Identity.GetUserId()));
            return View(item);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubscription(int id)
        {
            var user = accountMgr.GetProfile(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);

            if (!user.Subscriptions.Contains(item))
            {
                accountMgr.AddSubscription(user, item);
                return RedirectToAction("ItemDetail", "Home", new { Id = id });
            }
            return RedirectToAction("ItemDetail", "Home", new { Id = id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSubscription(int id)
        {
            var user = accountMgr.GetProfile(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);

            if (user.Subscriptions.Contains(item))
            {
                accountMgr.RemoveSubscription(user, item);
                return RedirectToAction("ItemDetail", "Home", new { Id = id });
            }
            return RedirectToAction("ItemDetail", "Home", new { Id = id });
        }
        #endregion

    }
}