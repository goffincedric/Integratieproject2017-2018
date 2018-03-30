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

namespace UI_MVC.Controllers
{
  public class HomeController : Controller
  {
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();

    private static readonly AccountManager mgr = new AccountManager(uow);

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



    //public FileContentResult GetUserIcon()
    //{
    //  if (Session["UserName"] == null)
    //  {
    //    string fileName = HttpContext.Server.MapPath(@"~/Content/Images/user_icon.png");
    //    byte[] imageData = null;
    //    FileInfo fileInfo = new FileInfo(fileName);
    //    long imageFileLength = fileInfo.Length;
    //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
    //    BinaryReader br = new BinaryReader(fs);
    //    imageData = br.ReadBytes((int)imageFileLength);
    //    return File(imageData, "image/png");
    //  }
    //  else
    //  {
    //    string fileName = HttpContext.Server.MapPath(@"~/Content/Images/1.jpg");
    //    byte[] imageData = null;
    //    FileInfo fileInfo = new FileInfo(fileName);
    //    long imageFileLength = fileInfo.Length;
    //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
    //    BinaryReader br = new BinaryReader(fs);
    //    imageData = br.ReadBytes((int)imageFileLength);
    //    return File(imageData, "image/jpg");

    //  }
    //}


    public ActionResult ChangeProfilePic()
    {
      if (Session["UserName"] == null)
      {
        return Content("<i class=\"ti-user\"></i>");
      }
      else
      {
        return Content("<img class=\"w - 2r bdrs - 50p\" src=/Content/Images/1.jpg>");
      }
    }

    public ActionResult ChangeLogoutin()
    {
      if (Session["UserName"] == null)
      {
        return Content("Login/Signup");
      }
      else
      {
        return Content("Logout");
      }
    }

    public ActionResult LinkLogoutin()
    {
      if (Session["UserName"] == null)
      {
        return Content("/home/signin");
      }
      else
      {
        EnsureLoggedOut();
        return Content("\"\"");
      }
    }
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
      if (mgr.GetProfile(newProfile.Username) != null)
      {
        return RedirectToAction("Signup");
        //if username already exists
      }
      else
      {

        if (ModelState.IsValid)
        {
          mgr.AddProfile(newProfile.Username, newProfile.Password, newProfile.Email);

          return RedirectToAction("Signin");
        }
        return RedirectToAction("Signup");

        //if (ModelState.IsValid)
        //{

        //    byte[] SALT = Get_SALT(15);
        //    newProfile.Salt = SALT;
        //    newProfile.Hash = Get_HASH_SHA512(newProfile.Password, newProfile.Username, SALT);
        //    newProfile.UserData = new UserData() { Profile = newProfile, Username = newProfile.Username };

        //    mgr.AddProfile(newProfile);
        //    return RedirectToAction("Signin");


        //}
        //return RedirectToAction("Signup");
      }
    }


    [HttpGet]
    public ActionResult Signin()
    {
      var userinfo = new Profile();

      try
      {
        EnsureLoggedOut();
        return View(userinfo);
      }
      catch
      {
        throw;
      }


    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Signin(Profile entity)
    {
      string OldHASHValue = string.Empty;
      byte[] SALT = new byte[15];
      try
      {

        // Ensure we have a valid viewModel to work with


        if (!ModelState.IsValid)
        {
          return View(entity);
        }
        else
        {

          //Retrive Stored HASH Value From Database According To Username (one unique field)
          var userInfo = mgr.GetProfile(entity.Username);

          //Assign HASH Value
          if (userInfo != null)
          {
            OldHASHValue = userInfo.Hash;
            SALT = userInfo.Salt;
          }

          bool isLogin = mgr.CompareHashValue(entity.Password, entity.Username, OldHASHValue, SALT);

          if (isLogin)
          {
            //Login Success
            //For Set Authentication in Cookie (Remeber ME Option)
            SignInRemember(entity.Username, entity.IsRemember);

            //Set A Unique ID in session
            Session["UserName"] = userInfo.Username;


            // If we got this far, something failed, redisplay form
            // return RedirectToAction("Index", "Dashboard");
            ViewBag.ImageUrl = "~/Content/Images/1.jpg";
            return RedirectToAction("Index");
          }
          else
          {
            //Login Fail
            //TempData["ErrorMSG"] = "Access Denied! Wrong Credential";

            return View(entity);
          }

        }
      }
      catch
      {
        throw;
      }
    }
    private void EnsureLoggedOut()
    {
      if (Request.IsAuthenticated)
      {
        Logout();
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Logout()
    {
      try
      {
        FormsAuthentication.SignOut();

        HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
        Session.Clear();

        System.Web.HttpContext.Current.Session.RemoveAll();
        return RedirectToAction("Index");
      }
      catch
      {
        throw;
      }

    }

    private void SignInRemember(string userName, bool isPersistent = false)
    {

      //Auth Cookie niet gesaved



      // FormsAuthentication.SignOut();
      //FormsAuthentication.SetAuthCookie(userName, isPersistent);
      //Profile profile = new Profile();
      //profile = mgr.GetProfile(userName);
      //profile.IsRemember = isPersistent;
      //mgr.ChangeProfile(profile);


    }

  }
}