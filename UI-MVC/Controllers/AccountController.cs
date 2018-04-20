using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Domain.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PB.BL;
using PB.BL.Domain.Account;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{

    [Authorize(Roles = "User,Admin,SuperAdmin")]
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

        #region LoginRegister



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
                user.Settings = new List<UserSetting>();
                user.Settings.Add(new UserSetting() { Profile = user, IsEnabled = true, SettingName = Setting.Account.THEME, Value = "light" });
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
        #endregion

        #region Account
        public ActionResult Account()
        {
            //nog via pk maken
            if (Request.IsAuthenticated)
            {
                AccountEditModel account = new AccountEditModel(UserManager.GetProfile(User.Identity.GetUserName()));
                return View(account);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account(AccountEditModel editedAccount)
        {
            Profile newProfile = UserManager.GetProfile(User.Identity.GetUserName());
            newProfile.UserData.LastName = editedAccount.LastName;
            newProfile.UserData.FirstName = editedAccount.FirstName;
            newProfile.Email = editedAccount.Email;
            newProfile.UserData.Telephone = editedAccount.Telephone;
            newProfile.UserData.Gender = editedAccount.Gender;
            newProfile.UserData.Street = editedAccount.Street;
            newProfile.UserData.City = editedAccount.City;
            newProfile.UserData.Province = editedAccount.Province;
            newProfile.UserData.PostalCode = editedAccount.PostalCode;
            newProfile.UserData.BirthDate = DateTime.ParseExact(editedAccount.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (ModelState.IsValid)
            {
                UserManager.ChangeProfile(newProfile);
                return View(editedAccount);
            }
            return View();
        }

        public ActionResult ResetPassword()
        {

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Account", "Account");
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if(UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
            {
                return RedirectToAction("Account","Account");
            }
          
            
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Account", "Account");
            }
            AddErrors(result);
            return RedirectToAction("Index","Home");
        }

        public ActionResult DeleteProfile()
        {
            return PartialView();
        }

        public ActionResult _NotificationDropdown()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProfile(DeleteProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Account", "Account");
            }
            var user = UserManager.GetProfile(User.Identity.GetUserName());

            UserManager.RemoveProfile(user.UserName);

            LogOff();

            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles=("Admin,SuperAdmin"))]
        //public ActionResult DeleteProfile(string username)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return RedirectToAction("AdminCrud", "Home");
        //    }
        //    var user = UserManager.GetProfile(username);

        //    UserManager.RemoveProfile(user.UserName);

        //    LogOff();

        //    return RedirectToAction("AdminCrud", "Home");
        //}

        #endregion


        public PartialViewResult _UserPartialTable()
        {
            IEnumerable<Profile> profiles = UserManager.GetProfiles();
            return PartialView(profiles);
        }

        #region ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToAction("ExternalLoginFailure", "Account");
                }

                var name = info.Email.Split('@')[0];
                var user = new Profile { UserName = name, Email = model.Email };
                user.UserData = new UserData() { Profile = user };
                user.Settings = new List<UserSetting>();
                user.Settings.Add(new UserSetting() { Profile = user, IsEnabled = true, SettingName = Setting.Account.THEME, Value = "light" });
                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    //Assign Role to user Here      
                    await UserManager.AddToRoleAsync(user.Id, "User");
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        #endregion

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