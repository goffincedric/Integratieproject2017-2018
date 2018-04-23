using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System.Collections.Generic;
using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
       

        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private readonly ItemManager itemMgr = new ItemManager(uow);
        private readonly AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);

        

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

                return Content("\"\"");
            }
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Blank()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Legal()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }


        public ActionResult _Search()
        {
            IEnumerable<Person> persons = itemMgr.GetPersons();
            return PartialView(persons);
            
        }

        public ActionResult AdminCrud()
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

                return View("~/Views/Home/Index.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
        }



       

        public ActionResult ItemDetail(int id)
        {
            Item item = itemMgr.GetItem(id);
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
                return RedirectToAction("ItemDetail", "Home", new { Id = id });
            }
            return RedirectToAction("ItemDetail", "Home", new { Id=id});
        }

    }
}