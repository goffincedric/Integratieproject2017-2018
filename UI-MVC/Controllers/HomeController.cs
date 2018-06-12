using System;
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

                ViewBag.Color1 = subplatform.Settings.Find(ss => ss.SettingName.Equals(Setting.Platform.PRIMARY_COLOR))?.Value;
                ViewBag.Color2 = subplatform.Settings.Find(ss => ss.SettingName.Equals(Setting.Platform.SECONDARY_COLOR))?.Value;

                IEnumerable<Tag> homeTags = SubplatformMgr.GetTags(subplatform.Pages.Single(p => p.PageName.Equals("Home")).PageId);
                ViewBag.HeaderText = homeTags.SingleOrDefault(t => t.Name.Equals("BannerTitle"))?.Text ?? "Subplatform title";
                ViewBag.BannerSub1 = homeTags.SingleOrDefault(t => t.Name.Equals("BannerTextSub1"))?.Text ?? "BannerTextSub1";
                ViewBag.BannerSub2 = homeTags.SingleOrDefault(t => t.Name.Equals("BannerTextSub2"))?.Text ?? "BannerTextSub2";
                ViewBag.CallToAction = homeTags.SingleOrDefault(t => t.Name.Equals("call-to-action-text"))?.Text ?? "call-to-action-text";
            }
        }
        #region Search

        public ActionResult _Search(string subplatform)
        {
            Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Item> items = itemMgr.GetItems().Where(i => i.SubPlatforms.Contains(sp)).OrderBy(i => i.Name);
            return PartialView(items);
        }

        #endregion

        #region Load
        public ActionResult ChangeProfilePic()
        {
            if (User.Identity.IsAuthenticated)
            {
                Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
                if (profile != null)
                {
                    var image = VirtualPathUtility.ToAbsolute(profile.ProfileIcon);
                    return Content(image);
                }
            }

            return ChangeLogoutin();
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

        public ActionResult IsSubplatformAdmin(string subplatform)
        {
            if (User.Identity.IsAuthenticated)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
                if (profile.AdminPlatforms.Contains(Subplatform)) return Content("True");
            }

            return Content("False");
        }

        public ActionResult GetName(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (Subplatform is null) return HttpNotFound();

            string name = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME).Value;
            return Content(name);
        }

        public ActionResult GetLogo(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            byte[] array = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Image;
            if (array != null)
            {
                var base64 = Convert.ToBase64String(array);
                var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                return Content(imgSrc);
            }
            else
            {
                string url = VirtualPathUtility.ToAbsolute(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Value);
                return Content(url);
            }

        }

        public ActionResult GetBanner(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            

            byte[] array = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Image;
            if (array != null)
            {
                var base64 = Convert.ToBase64String(array);
                var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                return Content(imgSrc);

            }
            else
            {
                string url = VirtualPathUtility.ToAbsolute(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.BANNER).Value); 


            return Content(url);
            }
        }
        public ActionResult LoadDefaults(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            ViewBag.Logo = VirtualPathUtility.ToAbsolute(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_ICON_URL).Value);
            ViewBag.SiteName = Content(ViewBag.SiteName = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME).Value);
            return Content("");
        }

        public ActionResult GetThemeSetting(string subplatform)
        {
            if (User.Identity.IsAuthenticated)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                string theme = "";
                Profile profile = accountMgr.GetProfile(User.Identity.GetUserId());
                if (profile != null)
                {
                    UserSetting userSetting = accountMgr.GetUserSetting(profile.Id, Setting.Account.THEME);

                    switch (userSetting.Value.ToLower())
                    {
                        case "light":
                            theme = "LightMode";
                            break;
                        case "dark":
                            theme = "DarkMode";
                            break;
                        case "future":
                            theme = "FutureMode";
                            break;
                    }
                }
                else
                {
                    theme = Subplatform.Settings.Find(ss => ss.SettingName.Equals(Setting.Platform.DEFAULT_THEME))?.Value;
                    if (theme == null) theme = "Light";
                }

                return Content(@"/Content/Theme/" + theme + @".css");
            }


            return Content("/Content/Theme/LightMode.css");
        }

        public ActionResult GetThemeColors(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            return Content(accountMgr.GetUserSetting(User.Identity.GetUserId(), Setting.Account.THEME).Value.ToLower());
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
            ViewBag.HeaderText = SubplatformMgr.GetTag("BannerTitle", subplatform).Text;
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            Person person = itemMgr.GetPersons().Where(p => !string.IsNullOrWhiteSpace(p.TwitterName)).OrderByDescending(p => p.TrendingScore).FirstOrDefault();
            ViewBag.TweetName = "https://twitter.com/" + person.TwitterName + "?ref_src=twsrc%5Etfw";
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
            ViewBag.Tag = "#collapse";
            ViewBag.Control = "collapse";

            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Tag> tags = Subplatform.Pages.SingleOrDefault(p => p.PageName.Equals("FAQ"))?.Tags;
            if (tags is null || tags.Count() == 0) return HttpNotFound();
            return View(tags);
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
                if (!string.IsNullOrWhiteSpace(person.TwitterName))
                {
                    ViewBag.Icon = "https://twitter.com/" + person.TwitterName + "/profile_image?size=original";
                    ViewBag.Twitter = "https://twitter.com/" + person.TwitterName;
                }
                else
                {

                    if(person.Image is null)
                    {
                        ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
                    }
                    else
                    {
                        byte[] array = person.Image;
                        var base64 = Convert.ToBase64String(array);
                        var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                        ViewBag.Icon = imgSrc; 
                    }
                  
                    ViewBag.Twitter = "";
                }

                if (string.IsNullOrWhiteSpace(person.Site))
                {
                    ViewBag.Site = "";
                }
                else
                {
                    ViewBag.Site = new System.UriBuilder(person.Site).Uri;
                }



            }

            if (item is Organisation organisation)
            {
                int? count = organisation.People.Count();
                ViewBag.Leden = count is null ? 0 : count;
                ViewBag.FullName = organisation.FullName;
              

                if (organisation.Image is null)
                {
                    ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
                }
                else
                {
                    byte[] array = organisation.Image;
                    var base64 = Convert.ToBase64String(array);
                    var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                    ViewBag.Icon = imgSrc;
                }
            }

            if (item is Theme theme)
            {
                int? count = theme.Records.Count();
                ViewBag.Associaties = count is null ? 0 : count;
            
                if (theme.Image is null)
                {
                    ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
                }
                else
                {
                    byte[] array = theme.Image;
                    var base64 = Convert.ToBase64String(array);
                    var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                    ViewBag.Icon = imgSrc;
                }
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