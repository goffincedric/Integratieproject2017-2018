﻿using System;
using System.Net;
using System.Security.Claims;
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
  [Authorize]
  [RequireHttps]
  public class AccountController : Controller
  {
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();

    private AccountManager _accountMgr;
    private IntegratieSignInManager _signInManager;

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
        return _signInManager ?? HttpContext.GetOwinContext().Get<IntegratieSignInManager>();
      }
      private set
      {
        _signInManager = value;
      }
    }

    public AccountManager UserManager
    {
      get
      {
        return _accountMgr ?? HttpContext.GetOwinContext().GetUserManager<AccountManager>();
      }
      private set
      {
        _accountMgr = value;
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

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);
      switch (result)
      {
        case SignInStatus.Success:

          return RedirectToAction("Index", "Home");
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
        var user = new Profile { UserName = model.Username, Email = model.Email, };
        user.UserData = new UserData() { Profile = user };
        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
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



    public ActionResult Account()
    {
      //nog via pk maken
      if (Request.IsAuthenticated)
      {
        Profile profile = UserManager.GetProfile(User.Identity.GetUserName());
        return View(profile);
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }
     
    }

    [HttpPost]
    public ActionResult Account(Profile newprofile)
    {
      if (ModelState.IsValid)
      {
        UserManager.ChangeProfile(newprofile);
        Console.WriteLine("werkt");
        return View(newprofile);

      }


      Console.WriteLine("Modelfout");
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult ExternalLogin(string provider, string returnUrl)
    {
      // Request a redirect to the external login provider
      return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account"));
    }

    [AllowAnonymous]
    public async Task<ActionResult> ExternalLoginCallback()
    {
      var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
      if (loginInfo == null)
      {
        return RedirectToAction("Login");
      }

      // Sign in the user with this external login provider if the user already has a login
      var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToAction("Index","Home");
        case SignInStatus.LockedOut:
          return View("Lockout");
        //case SignInStatus.RequiresVerification:
        //  return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        case SignInStatus.Failure:
          return View("Failed");
        default:
          // If the user does not have an account, then prompt the user to create an account
          return View("Default");
          //ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
          //ViewBag.RoleList = new SelectList(UserManager.GetAllRoles(), "Name", "Name");
          //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
      }
    }

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
    private const string XsrfKey = "XsrfId";

    internal class ChallengeResult : HttpUnauthorizedResult
    {
      public ChallengeResult(string provider, string redirectUri)
          : this(provider, redirectUri, null)
      {
      }

      public ChallengeResult(string provider, string redirectUri, string userId)
      {
        LoginProvider = provider;
        RedirectUri = redirectUri;
        UserId = userId;
      }

      public string LoginProvider { get; set; }
      public string RedirectUri { get; set; }
      public string UserId { get; set; }

      public override void ExecuteResult(ControllerContext context)
      {
        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        if (UserId != null)
        {
          properties.Dictionary[XsrfKey] = UserId;
        }
        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
      }
    }

    #endregion
  }
}