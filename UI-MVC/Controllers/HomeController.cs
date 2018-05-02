using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI_MVC.Controllers
{
    [RequireHttps]
    [RoutePrefix("{subplatform}")]
    public class HomeController : Controller
    {


        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private readonly ItemManager itemMgr = new ItemManager(uow);
        private readonly AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
        private readonly SubplatformManager SubplatformMgr = new SubplatformManager(uow);



        #region profile

        public ActionResult ChangeProfilePic()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("<i class=\"ti-user\"></i>");
            }
            else
            {
                return Content("<img class=\"w-2r bdrs-50p\" src=/Content/Images/1.jpg>");
            }
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

        #endregion

        [Route("~/")]
        public ActionResult Index2()
        {
            return RedirectToAction("Index", "Home", new { subplatform = "politieke-barometer" });
        }

        [Route("")]
        public ActionResult Index(string subplatform)
        {
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View("Index");
        }


        public ActionResult Blank(string subplatform)
        {
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View();
        }

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

        public ActionResult Test(string subplatform)
        {
            ViewBag.Title = SubplatformMgr.GetSubplatform(subplatform).Name;
            return View();
        }


        public ActionResult _Search(string subplatform)
        {
            Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Item> items = itemMgr.GetItems().Where(i => i.SubPlatforms.Contains(sp));
            return PartialView(items);
        }

        public ActionResult PlatformSettings()
        {
            ViewBag.TotalUsers = accountMgr.GetUserCount().ToString();
            ViewBag.TotalPersons = itemMgr.GetPersonsCount().ToString();
            ViewBag.TotalOrganisations = itemMgr.GetOrganisationsCount().ToString();
            ViewBag.TotalThemes = itemMgr.GetThemesCount().ToString();
            ViewBag.TotalKeywords = itemMgr.GetKeywordsCount().ToString();
            ViewBag.TotalItems = itemMgr.GetItemsCount().ToString();
            return View();
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
                    case "light": theme = "LightMode"; break;
                    case "dark": theme = "DarkMode"; break;
                    case "future": theme = "FutureMode"; break;
                }
                return Content($"/Content/Theme/{theme}.css");
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




        [Authorize]
        public ActionResult ItemDetail(string subplatform, int id)
        {
            Item item = itemMgr.GetItem(id);
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (!item.SubPlatforms.Contains(Subplatform)) return HttpNotFound();
            if (item.IconURL is null)
            {
                //ViewBag.Icon = VirtualPathUtility.ToAbsolute("~/Content/Users/user.png");
            }
            else
            {
                ViewBag.Icon = VirtualPathUtility.ToAbsolute(item.IconURL);
            }

            //ViewBag.Icon=@"~\Content\Images\Partijen\vb.png";
            if (item is Person person)
            {
                int? count = person.Records.Count();
                ViewBag.Vermeldingen = (count is null) ? 0 : count;
            }
            if (item is Organisation organisation)
            {

                // int? count = organisation.People.Count();
                //ViewBag.Leden = (count is null) ? 0 : count;
                ViewBag.Leden = 0;
                ViewBag.FullName = organisation.FullName;
            }
            if (item is Theme theme)
            {
                //int? count = theme.Records.Count();
                //ViewBag.Associaties = (count is null) ? 0 : count;
                ViewBag.Associaties = 0;
                ViewBag.Keywords = theme.Keywords.ToList();
            }
            ViewBag.Subscribed = item.SubscribedProfiles.Contains(accountMgr.GetProfile(User.Identity.GetUserId()));

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubscription(int id)
        {
            var user = accountMgr.GetProfile(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);

            if (!user.Subscriptions.Contains(item))
            {

                accountMgr.AddSubscription(user, item);
                //return View();
                return RedirectToAction("ItemDetail", "Home", new { Id = id });
            }
            return RedirectToAction("ItemDetail", "Home", new { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSubscription(int id)
        {
            var user = accountMgr.GetProfile(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);

            if (user.Subscriptions.Contains(item))
            {

                accountMgr.RemoveSubscription(user, item);
                //return View();
                return RedirectToAction("ItemDetail", "Home", new { Id = id });
            }
            return RedirectToAction("ItemDetail", "Home", new { Id = id });
        }

        [HttpPost]
        public ActionResult GenerateAlertsManually()
        {
            List<Item> itemsToUpdate = new List<Item>();
            accountMgr.GenerateAllAlerts(itemMgr.GetItems(), out itemsToUpdate);
            itemMgr.ChangeItems(itemsToUpdate);

            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        public ActionResult CleanupDB(string subplatform)
        {
            Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
            itemMgr.CleanupOldRecords(sp);
            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        public ActionResult Syncronize(string subplatform)
        {
            Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
            itemMgr.SyncDatabase(sp);
            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Site/"), fileName);
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("PlatformSettings","Home");


        }

        public ActionResult _UrlList(int id)
        {
            List<Url> urls = null;
            itemMgr.GetPerson(id).Records.ForEach(p => urls.ForEach(u => urls.Add(u)));



            return PartialView(urls);
        }
    }
}