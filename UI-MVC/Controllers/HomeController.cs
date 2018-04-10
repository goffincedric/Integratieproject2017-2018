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

namespace UI_MVC.Controllers
{
  [RequireHttps]
  public class HomeController : Controller
  {
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();



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
    public ActionResult Index()
    {

      return View();
    }

 

    public ActionResult Dashboard()
    {
      return View();
    }

    public ActionResult BasicTable()
    {
      return View();
    }


    public ActionResult Blank()
    {
      return View();
    }


    public ActionResult Charts()
    {
      return View();
    }


    public ActionResult DataTable()
    {
      return View();
    }


    public ActionResult Forms()
    {
      return View();
    }


    public ActionResult Test404()
    {
      return View();
    }


    public ActionResult Test500()
    {
      return View();
    }

    public ActionResult UI()
    {
      return View();
    }




  }
}