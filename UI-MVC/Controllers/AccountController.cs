using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB.BL;
using PB.BL.Domain.Account;

namespace UI_MVC.Controllers
{

  [RequireHttps]
  public class AccountController : Controller
  {
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
    private static readonly AccountManager accountMgr = new AccountManager(uow);

  
    public ActionResult Account()
    {

      Profile profile;
      if (Session["UserName"] != null)
      {
        profile = accountMgr.GetProfile(Session["UserName"].ToString());

      }
      else
      {
        profile = null;
      }


      return View(profile);
    }

    [HttpPost]
    public ActionResult Account(Profile newprofile)
    {
      if (ModelState.IsValid)
      {
        accountMgr.ChangeProfile(newprofile);
        Console.WriteLine("werkt");
        return View(newprofile);
       
      }


      Console.WriteLine("Modelfout");
      return View();
    }

  }
}