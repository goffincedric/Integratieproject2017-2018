using PB.BL;
using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using PB.DAL.EF;
using UI_MVC.Models;
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using Microsoft.Owin.Security;
using System.Net;
using Domain.Settings;

namespace UI_MVC.Controllers
{
  [RequireHttps]
  public class HomeController : Controller
  {
    private static IntegratieDbContext Context = new IntegratieDbContext();
    private static readonly IntegratieUserStore Store = new IntegratieUserStore(Context);
    private static readonly AccountManager AccountMgr = new AccountManager(Store);
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();

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

    public ActionResult GetThemeSetting()
    {
      if (User.Identity.IsAuthenticated)
      {
        string theme = "";
        Profile profile = AccountMgr.GetProfile(User.Identity.GetUserName());
        UserSetting userSetting = AccountMgr.GetUserSetting(profile.UserName, Setting.Account.THEME);

        switch (userSetting.Value)
        {
          case "light": theme = "LightMode"; break;
          case "dark": theme = "DarkMode"; break;
          case "future": theme = "FutureMode"; break;
        }
        return Content(string.Format("/Content/Theme/{0}.css", theme));
      }
      return Content("/Content/Theme/LightMode.css");
    }
    
    public ActionResult ChangeThemeSetting(string Theme)
    {
      if (User.Identity.IsAuthenticated)
      {
        Profile profile = AccountMgr.GetProfile(User.Identity.GetUserName());
        UserSetting userSetting = AccountMgr.GetUserSetting(profile.UserName, Setting.Account.THEME);

        userSetting.Value = Theme;

        AccountMgr.ChangeUserSetting(profile.UserName, userSetting);

        return View("~/Views/Home/Index.cshtml");
      }
      return View("~/Views/Home/Index.cshtml");
    }
  }      
}