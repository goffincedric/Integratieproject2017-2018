using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;

namespace UI_MVC.Controllers
{
    /// <summary>
    ///     This controller rules over homepage and page that are an addition to the website or where it was not clear where to
    ///     put a certain method.
    ///     Authorization depends on used method
    /// </summary
    [RequireHttps]
    [RoutePrefix("{subplatform}")]
    public class HomeController : Controller
    {
        private readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private readonly ItemManager itemMgr;
        private readonly AccountManager accountMgr;
        private readonly SubplatformManager SubplatformMgr;


        public HomeController()
        {
            itemMgr = new ItemManager(uow);
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

            ViewBag.HeaderText = SubplatformMgr.GetTag("BannerTitle").Text;
            ViewBag.BannerSub1 = SubplatformMgr.GetTag("BannerTextSub1").Text;
            ViewBag.BannerSub2 = SubplatformMgr.GetTag("BannerTextSub2").Text;
            ViewBag.CallToAction = SubplatformMgr.GetTag("call-to-action-text").Text;
        }

        #region Search

        public ActionResult _Search(string subplatform)
        {
            Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Item> items = itemMgr.GetItems().Where(i => i.SubPlatforms.Contains(sp));
            return PartialView(items);
        }

        #endregion

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
                return Content("Login/Register");
            return Content("Logout");
        }

        public ActionResult LinkLogoutin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("/Account/Login");
            }

            RedirectToAction("Logoff", "Account");

            return Content("");
        }

        public ActionResult GetName(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            string name = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME)
                .Value;


            return Content(name);
        }

        public ActionResult GetLogo(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            string url = VirtualPathUtility.ToAbsolute(SubplatformMgr
                .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Value);
            return Content(url);
        }

        public ActionResult LoadDefaults(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            ViewBag.Logo = VirtualPathUtility.ToAbsolute(SubplatformMgr
                .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Value);
            ViewBag.SiteName = Content(ViewBag.SiteName = SubplatformMgr
                .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME).Value);
            return Content("");
        }

        public ActionResult GetThemeSetting(string subplatform)
        {
            if (User.Identity.IsAuthenticated)
            {
                string theme = "";
                Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
                if (profile != null)
                {
                    UserSetting userSetting = accountMgr.GetUserSetting(profile.Id, Setting.Account.THEME);

                    switch (userSetting.Value)
                    {
                        case "Light":
                            theme = "LightMode";
                            break;
                        case "Dark":
                            theme = "DarkMode";
                            break;
                        case "Future":
                            theme = "FutureMode";
                            break;
                    }
                }
                else
                {
                    theme = SubplatformMgr.GetSubplatform(subplatform).Settings.Find(ss => ss.SettingName.Equals(Setting.Platform.DEFAULT_THEME))?.Value;
                    if (theme == null) theme = "Light";
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

        #region ItemDetail

        public ActionResult ItemDetail(string subplatform, int id)
        {
            Item item = itemMgr.GetItem(id);
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (!item.SubPlatforms.Contains(Subplatform)) return HttpNotFound();
          

            if (item is Person person)
            {
                int? count = person.Records.Count();
                ViewBag.Vermeldingen = count is null ? 0 : count;
                ViewBag.Partij = person.Organisation is null ? "Geen partij" : person.Organisation.Name;
                if(person.TwitterName != null && person.TwitterName != "")
                {
                    ViewBag.Icon = "https://twitter.com/" + person.TwitterName + "/profile_image?size=original";
                    ViewBag.Twitter = "https://twitter.com/" + person.TwitterName;
                }
                else
                {
                    ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
                    ViewBag.Twitter = "";
                }

                if(person.Site == "" || person.Site is null)
                {
                    ViewBag.Site = "";
                }
                else
                {
                    ViewBag.Site =  "//" + person.Site;
                }
                
                
               
            }

            if (item is Organisation organisation)
            {
                int? count = organisation.People.Count();
                ViewBag.Leden = count is null ? 0 : count;
                ViewBag.FullName = organisation.FullName;
                ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
            }

            if (item is Theme theme)
            {
                int? count = theme.Records.Count();
                ViewBag.Associaties = count is null ? 0 : count;
                ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
            }

            ViewBag.Subscribed = item.SubscribedProfiles.Contains(accountMgr.GetProfile(User.Identity.GetUserId()));
            return View(item);
        }

        [Authorize(Roles = "User,Admin,SuperAdmin")]
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

        [Authorize(Roles = "User,Admin,SuperAdmin")]
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