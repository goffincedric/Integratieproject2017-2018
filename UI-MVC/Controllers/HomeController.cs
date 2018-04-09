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
      if (Session["UserName"] == null)
      {
        return Content("<i class=\"ti-user\"></i>");
      }
      else
      {
        return Content("<img class=\"w-2r bdrs-50p\" src=/Content/Images/1.jpg>");
      }
    }

    public ActionResult GetActiveUser()
    {
      if (Session["UserName"] == null)
      {
        return Content("Niet ingelogd");
      }
      else
      {
        string username = Session["UserName"].ToString();

        return Content(username);
      }
    }

    public ActionResult ChangeLogoutin()
    {
      if (Session["UserName"] == null)
      {
        return Content("LogIn/Register");
      }
      else
      {
        return Content("LogOut");
      }
    }

    public ActionResult LinkLogoutin()
    {
      if (Session["UserName"] == null)
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


    //private void EnsureLoggedOut()
    //{
    //  // If the request is (still) marked as authenticated we send the user to the logout action
    //  if (Request.IsAuthenticated)
    //    Logout();
    //}


    //[HttpPost]
    //[AllowAnonymous]
    //[ValidateAntiForgeryToken]
    //public async System.Threading.Tasks.Task<ActionResult> Register(RegisterViewModel newProfile)
    //{
    //  //if (accountMgr.GetProfile(newProfile.Username) != null)
    //  //{
    //  //  return RedirectToAction("Signup");
    //  //  //if username already exists
    //  //}
    //  //else
    //  //{

    //  //  if (ModelState.IsValid && newProfile.ConfirmPassword.Equals(newProfile.Password))
    //  //  {
    //  //    accountMgr.AddProfile(newProfile.UserName, newProfile.Password, newProfile.Email);

    //  //    return RedirectToAction("Signin");
    //  //  }
    //  //  return RedirectToAction("Signup");


    //  //}

    //  if (ModelState.IsValid)
    //  {
    //    var user = new Profile
    //    {
    //      UserName = newProfile.UserName,
    //      Email = newProfile.Email

    //    };
    //    var result = await accountMgr.CreateAsync(user, newProfile.Password);

    //    if (result.Succeeded)
    //    {
    //      //string code = await UserManager.Generate
          
    //      await IntegratieSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
    //      return RedirectToAction("SignIn", "Home");
    //    }
    //    return View(newProfile);
    //  }
    //}


    //[HttpGet]
    //public ActionResult Signin()
    //{
    //  var userinfo = new Profile();

    //  try
    //  {
    //    Logout();
    //    return View(userinfo);
    //  }
    //  catch
    //  {
    //    throw;
    //  }


    //}


    ////[HttpPost]
    ////[ValidateAntiForgeryToken]
    ////public ActionResult Signin(Profile entity)
    ////{
    ////  string OldHASHValue = string.Empty;
    ////  byte[] SALT = new byte[15];
    ////  try
    ////  {

    ////    // Ensure we have a valid viewModel to work with


    ////    if (!ModelState.IsValid)
    ////    {
       
    ////      return View(entity);
    ////    }
    ////    else
    ////    {

    ////      //Retrive Stored HASH Value From Database According To Username (one unique field)
    ////      var userInfo = accountMgr.GetProfile(entity.Username);

    ////      //Assign HASH Value
    ////      if (userInfo != null)
    ////      {
    ////        OldHASHValue = userInfo.Hash;
    ////        SALT = userInfo.Salt;
    ////      }

    ////      bool isLogin = accountMgr.CompareHashValue(entity.Password, entity.Username, OldHASHValue, SALT);

    ////      if (isLogin)
    ////      {
    ////        //Login Success
    ////        //For Set Authentication in Cookie (Remeber ME Option)
    ////        SignInRemember(entity.Username, entity.IsRemember);

    ////        //Set A Unique ID in session
    ////        Session["UserName"] = userInfo.Username;


    ////        // If we got this far, something failed, redisplay form
    ////        // return RedirectToAction("Index", "Dashboard");
           
    ////        return RedirectToAction("Index");
    ////      }
    ////      else
    ////      {
    ////        //Login Fail
    ////        //TempData["ErrorMSG"] = "Access Denied! Wrong Credential";

    ////        return View(entity);
    ////      }

    ////    }
    ////  }
    ////  catch
    ////  {
    ////    throw;
    ////  }
    ////}
   

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Logout()
    //{
    //  try
    //  {
    //    FormsAuthentication.SignOut();

    //    HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
    //    Session.Clear();

    //    System.Web.HttpContext.Current.Session.RemoveAll();
    //    return RedirectToAction("Index");
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //}



    //private void SignInRemember(string userName, bool isPersistent = false)
    //{

    //  //Auth Cookie niet gesaved



    //  // FormsAuthentication.SignOut();
    //  //FormsAuthentication.SetAuthCookie(userName, isPersistent);
    //  //Profile profile = new Profile();
    //  //profile = mgr.GetProfile(userName);
    //  //profile.IsRemember = isPersistent;
    //  //mgr.ChangeProfile(profile);


    //}

  }
}