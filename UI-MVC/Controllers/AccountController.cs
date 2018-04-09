using System;
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

      SignInManager = signInManager;
      UserManager = userManager;
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
          var result = await this.UserManager.CreateAsync(user, model.Password);
          if (result.Succeeded)
          {
            
            await UserManager.AddClaimAsync(user.Id, new Claim("UserName", user.UserName));
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

      Profile profile;
      if (User.Identity.GetUserName() != null)
      {
        profile = _accountMgr.GetProfile(User.Identity.GetUserName());

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
        _accountMgr.ChangeProfile(newprofile);
        Console.WriteLine("werkt");
        return View(newprofile);

      }


      Console.WriteLine("Modelfout");
      return View();
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

    #endregion
  }
}