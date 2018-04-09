using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PB.BL;
using PB.BL.Domain.Account;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{

  [RequireHttps]
  public class AccountController : Controller
  {
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();

    private AccountManager accountMgr;
    private IntegratieSignInManager signInManager;

    public AccountController()
    {

    }

    public AccountController(AccountManager userManager, IntegratieSignInManager signInManager)
    {
      UserManager = userManager;
      SignInManager = signInManager;
    }

    public IntegratieSignInManager SignInManager
    {
      get
      {
        return signInManager ?? HttpContext.GetOwinContext().Get<IntegratieSignInManager>();
      }
      private set
      {
        signInManager = value;
      }
    }

    public AccountManager UserManager
    {
      get
      {
        return accountMgr ?? HttpContext.GetOwinContext().GetUserManager<AccountManager>();
      }
      private set
      {
        accountMgr = value;
      }
    }


    [AllowAnonymous]
    public ActionResult Login()
    {

      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      //var status = CheckRCaptcha();

      //if (status == false)
      //{
      //  return View();
      //}

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);
      switch (result)
      {
        case SignInStatus.Success:

          return RedirectToAction("Index");
        case SignInStatus.LockedOut:
          return View("Lockout");
        //case SignInStatus.RequiresVerification:
        //  return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        case SignInStatus.Failure:
        default:
          ModelState.AddModelError("", "Invalid login attempt.");
          return View(model);
      }
    }


    [AllowAnonymous]
    public ActionResult Register()
    {
      RegisterViewModel registerViewModel = new RegisterViewModel();
     
      return View(registerViewModel);
    }


    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = new Profile { UserName = model.UserName, Email = model.Email, };
        user.UserData = new UserData() { Profile = user };
        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          ////Add claim to user
          //await UserManager.AddClaimAsync(user.Id, new Claim(ClaimTypes.DateOfBirth, model.BirthDate.ToShortDateString()));
          ////Send an email with this link
          //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
          //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
          //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
          //Assign Role to user    
          await UserManager.AddToRoleAsync(user.Id, "User");
          //Login
          await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

          return RedirectToAction("Index", "Home");
        }
        AddErrors(result);


      }
      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff()
    {
      AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      return RedirectToAction("Index", "Home");
    }


    //public ActionResult Account()
    //{

    //  Profile profile;
    //  if (Session["UserName"] != null)
    //  {
    //    profile = accountMgr.GetProfile(Session["UserName"].ToString());

    //  }
    //  else
    //  {
    //    profile = null;
    //  }


    //  return View(profile);
    //}

    //[HttpPost]
    //public ActionResult Account(Profile newprofile)
    //{
    //  if (ModelState.IsValid)
    //  {
    //    accountMgr.ChangeProfile(newprofile);
    //    Console.WriteLine("werkt");
    //    return View(newprofile);

    //  }


    //  Console.WriteLine("Modelfout");
    //  return View();
    //}



    #region Helpers
    private IAuthenticationManager AuthenticationManager
    {
      get
      {
        return HttpContext.GetOwinContext().Authentication;
      }
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError("", error);
      }
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      return RedirectToAction("Index", "Home");
    }

    #endregion
  }
}