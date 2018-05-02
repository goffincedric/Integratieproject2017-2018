﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PB.BL.Domain.Items;
using UI_MVC.Models;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using System.Linq;
using System.Data.Entity.Validation;
using PB.DAL.EF;

namespace UI_MVC.Controllers
{

    [Authorize(Roles = "User,Admin,SuperAdmin")]
    [RequireHttps]
    public class AccountController : Controller
    {
        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private AccountManager _accountMgr;
        private IntegratieSignInManager _signInManager;
        private SubplatformManager _subplatformMgr;

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

        public SubplatformManager SubplatformMgr
        {
            get
            {
                return _subplatformMgr ?? new SubplatformManager(HttpContext.GetOwinContext().Get<IntegratieDbContext>()); ;
            }
            private set
            {
                _subplatformMgr = value;
            }
        }

        #region LoginRegister
        [AllowAnonymous]
        public ActionResult Login(string subplatform, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Subplatform = subplatform;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string subplatform, LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Index");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult Register(string subplatform)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            ViewBag.Subplatform = subplatform;
            return View(registerViewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string subplatform, RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Profile { UserName = model.Username, Email = model.Email, };
                user.UserData = new UserData() { Profile = user };
                user.Settings = new List<UserSetting>
                {
                    new UserSetting()
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.THEME,
                        Value = "light"
                    }
                };
                
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Assign Role to user
                    await UserManager.AddToRoleAsync(user.Id, "User");
                    //Login
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", "Home", new { subplatform });
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string subplatform)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home", new { subplatform });
        }
        #endregion

        #region Notification
        public ActionResult GetNotificationCount(string subplatform)
        {
            Profile user = UserManager.GetProfile(User.Identity.GetUserId());
            int alertCount = user.ProfileAlerts.FindAll(pa => !pa.IsRead && pa.Alert.Item.SubPlatforms.Find(s => s.URL.ToLower().Equals(subplatform)) != null).Count;
            return Content(String.Format("{0}", alertCount));
        }

        public ActionResult ClickNotification(int id)
        {
            Profile profile = UserManager.GetProfile(User.Identity.GetUserId());
            ProfileAlert profileAlert = profile.ProfileAlerts.Find(pa => pa.AlertId == id);
            profileAlert.IsRead = true;

            UserManager.ChangeProfile(profile);

            int itemId = profileAlert.Alert.ItemId;

            return RedirectToAction("ItemDetail", "Home", new { id = itemId });
        }

        public ActionResult _NotificationDropdown(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var model = UserManager.GetProfileAlerts(Subplatform, UserManager.GetProfile(User.Identity.GetUserId()));
            return PartialView(model);
        }

        public ActionResult Notifications(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var model = UserManager.GetProfileAlerts(Subplatform, UserManager.GetProfile(User.Identity.GetUserId()));
            return View(model);
        }

        #endregion

        #region Account
        public ActionResult Account()
        {
            //nog via pk maken
            if (Request.IsAuthenticated)
            {
                AccountEditModel account = new AccountEditModel(UserManager.GetProfile(User.Identity.GetUserId()));
                return View(account);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // TODO: MOET PUT ZIJN I.P.V. POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account(AccountEditModel editedAccount)
        {
            Profile newProfile = UserManager.GetProfile(User.Identity.GetUserId());
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

        public ActionResult _ResetPassword()
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


            if (UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
            {
                return RedirectToAction("Account", "Account");
            }


            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Account", "Account");
            }
            AddErrors(result);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _DeleteProfile()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProfile(string subplatform, DeleteProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Account", "Account");
            }
            var user = UserManager.GetProfile(User.Identity.GetUserId());

            UserManager.RemoveProfile(user.Id);

            LogOff(subplatform);

            return RedirectToAction("Index", "Home");
        }

        // TODO: REMOVE USER ROLE FROM AUTHORIZE 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("User,Admin,SuperAdmin"))]
        public ActionResult DeleteProfileAdmin(string subplatform, string userId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AdminCrud", "Home");
            }
            var user = UserManager.GetProfile(userId);


            UserManager.RemoveProfile(user.Id);

            LogOff(subplatform);

            return RedirectToAction("AdminCrud", "Home");
        }

        #endregion



        public ViewResult UserBeheer()
        {
            IEnumerable<Profile> profiles = UserManager.GetProfiles();
            return View(profiles);
        }

        #region ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string subplatform, string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { subplatform, returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string subplatform, string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login", "Account", new { subplatform, returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Index");
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.Subplatform = subplatform;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(string subplatform, ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var name = info.Email.Split('@')[0];
                var user = new Profile
                {
                    UserName = name,
                    Email = model.Email
                };
                user.UserData = new UserData() { Profile = user };
                user.Settings = new List<UserSetting>
                {
                    new UserSetting()
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.THEME,
                        Value = "light"
                    }
                };

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
            ViewBag.Subplatform = subplatform;
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