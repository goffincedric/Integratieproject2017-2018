using PB.BL;
using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace UI_MVC.Controllers
{
  public class HomeController : Controller {
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
  
    private static readonly AccountManager mgr = new AccountManager(uow); 
   

  
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Email()
    {
      return View(); 
    }

    public ActionResult Compose()
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


    public ActionResult Calender()
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


    public ActionResult GoogleMaps()
    {
      return View();
    }

    public ActionResult Signin()
    {
      return View();
    }

    public ActionResult Signup()
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


    public ActionResult VectorMaps()
    {
      return View();
    }

    public ActionResult Chat()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Register(Profile newProfile)
    {
      if (ModelState.IsValid)
      {
        Profile profile = mgr.AddProfile(newProfile.Username, newProfile.Email, newProfile.Password);
        return RedirectToAction("Index"); 
       
        
      }
      return View(); 
    }
   
  }
}