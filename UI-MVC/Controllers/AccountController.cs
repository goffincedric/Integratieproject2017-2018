using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Domain.Accounts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using PB.DAL.EF;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    /// <summary>
    ///     Controller for everything that has to handle with account or to get an account
    ///     Authorized by all roles
    /// </summary>
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private AccountManager _accountMgr;
        private IntegratieSignInManager _signInManager;
        private readonly SubplatformManager SubplatformMgr;
        private ItemManager _itemMgr;


        public AccountController()
        {
            SubplatformMgr = new SubplatformManager(uow);

            ViewBag.Home = SubplatformMgr.GetTag("Home").Text;
            ViewBag.Dashboard = SubplatformMgr.GetTag("Dashboard").Text;
            ViewBag.WeeklyReview = SubplatformMgr.GetTag("Weekly_Review").Text;
            ViewBag.MyAccount = SubplatformMgr.GetTag("Account").Text;
            ViewBag.More = SubplatformMgr.GetTag("More").Text;
            ViewBag.FAQ = SubplatformMgr.GetTag("FAQ").Text;
            ViewBag.Contact = SubplatformMgr.GetTag("Contact").Text;
            ViewBag.Legal = SubplatformMgr.GetTag("Legal").Text;
            ViewBag.Items = SubplatformMgr.GetTag("Items").Text;
            ViewBag.Persons = SubplatformMgr.GetTag("Persons").Text;
            ViewBag.Organisations = SubplatformMgr.GetTag("Organisations").Text;
            ViewBag.Themes = SubplatformMgr.GetTag("Themes").Text;
        }

        public AccountController(AccountManager userManager, IntegratieSignInManager signInManager,
            ItemManager itemManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _itemMgr = itemManager;
        }

        public IntegratieSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<IntegratieSignInManager>();
            private set => _signInManager = value;
        }

        public AccountManager UserManager
        {
            get => _accountMgr ?? HttpContext.GetOwinContext().GetUserManager<AccountManager>();
            private set => _accountMgr = value;
        }

        public ItemManager ItemMgr
        {
            get => _itemMgr ?? new ItemManager(HttpContext.GetOwinContext().Get<IntegratieDbContext>());
            private set => _itemMgr = value;
        }

        #region Alerts

        public ActionResult WeeklyReview()
        {
            WeeklyReview weeklyReview = UserManager.GetLatestWeeklyReview(User.Identity.GetUserId());
            if (weeklyReview is null) return View(weeklyReview);

            Person person = ItemMgr.GetPerson(weeklyReview.TopPersonId);
            if (person.IconURL is null)
                ViewBag.Icon = VirtualPathUtility.ToAbsolute("~/Content/Images/Users/user.png");
            else
                ViewBag.Icon = person.IconURL;

            return View(weeklyReview);
        }

        #endregion

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
            if (!ModelState.IsValid) return View(model);

            var result =
                await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
            
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
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (ModelState.IsValid)
            {
                var user = new Profile
                {
                    UserName = model.Username,
                    Email = model.Email,
                    ProfileIcon =
                        SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId,
                            Setting.Platform.DEFAULT_NEW_USER_ICON).Value,
                    CreatedOn = DateTime.Today
                };
                user.UserData = new UserData { Profile = user };
                user.Settings = new List<UserSetting>
                {
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.THEME,
                        Value = SubplatformMgr
                            .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DEFAULT_THEME).Value,
                        boolValue = false
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_ANDROID_NOTIFICATIONS,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_SITE_NOTIFICATIONS,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_EMAIL_NOTIFICATIONS,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_WEEKLY_REVIEW_VIA_MAIL,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    }
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Assign Role to user
                    await UserManager.AddToRoleAsync(user.Id, "User");
                    //Login
                    await SignInManager.SignInAsync(user, false, false);

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
            int alertCount = 0;
            if (User.Identity.IsAuthenticated)
            {
                if (user != null)
                {
                    alertCount = user.ProfileAlerts.FindAll(pa => !pa.IsRead && pa.Alert.Item.SubPlatforms.Find(s => s.URL.ToLower().Equals(subplatform)) != null).Count;
                }
            }
            
            return Content(string.Format("{0}", alertCount));
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
            var model = UserManager.GetSiteProfileAlerts(Subplatform, User.Identity.GetUserId());
            return PartialView(model);
        }

        public ActionResult Notifications(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var model = UserManager.GetSiteProfileAlerts(Subplatform, User.Identity.GetUserId());
            return View(model);
        }

        #endregion

        #region Account

        public ActionResult Account(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            //nog via pk maken
            if (Request.IsAuthenticated)
            {
                Profile profile = UserManager.GetProfile(User.Identity.GetUserId());
                ViewBag.ProfileImage = profile.ProfileIcon is null
                    ? VirtualPathUtility.ToAbsolute(SubplatformMgr
                        .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DEFAULT_NEW_USER_ICON).Value)
                    : VirtualPathUtility.ToAbsolute(profile.ProfileIcon);
                AccountEditModel account = new AccountEditModel(profile);
                return View(account);
            }

            return RedirectToAction("Index", "Home");
        }

        // TODO: MOET PUT ZIJN I.P.V. POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account(AccountEditModel editedAccount)
        {
            string _FileName = "";
            Profile newProfile = UserManager.GetProfile(User.Identity.GetUserId());

            if (editedAccount.file != null)
            {
                if (editedAccount.file.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(editedAccount.file.FileName);

                    var username = newProfile.UserName;
                    var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                    string _path = Path.Combine(Server.MapPath("~/Content/Images/Users/"), newName);
                    editedAccount.file.SaveAs(_path);
                    newProfile.ProfileIcon = @"~/Content/Images/Users/" + newName;
                }
            }
            else
            {
                newProfile.ProfileIcon = newProfile.ProfileIcon;
            }

            newProfile.UserData.LastName = editedAccount.LastName;
            newProfile.UserData.FirstName = editedAccount.FirstName;
            newProfile.Email = editedAccount.Email;
            //newProfile.UserData.Telephone = editedAccount.Telephone;
            //newProfile.UserData.Gender = editedAccount.Gender;
            newProfile.UserData.Street = editedAccount.Street;
            newProfile.UserData.City = editedAccount.City;
            newProfile.UserData.Province = editedAccount.Province;
            newProfile.UserData.PostalCode = editedAccount.PostalCode;


            if (ModelState.IsValid)
            {
                UserManager.ChangeProfile(newProfile);
                return RedirectToAction("Account", "Account");
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
            if (!ModelState.IsValid) return RedirectToAction("Account", "Account");
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());


            if (UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password) ==
                PasswordVerificationResult.Failed) return RedirectToAction("Account", "Account");


            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.NewPassword);
            if (result.Succeeded) return RedirectToAction("Account", "Account");
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
            if (!ModelState.IsValid) return RedirectToAction("Account", "Account");
            var user = UserManager.GetProfile(User.Identity.GetUserId());

            UserManager.RemoveProfile(user.Id);

            LogOff(subplatform);

            return RedirectToAction("Index", "Home");
        }

        // TODO: REMOVE USER ROLE FROM AUTHORIZE 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public ActionResult DeleteProfileAdmin(string userId)
        {
            if (!ModelState.IsValid || userId.Equals(User.Identity.GetUserId()))
                return RedirectToAction("UserBeheer", "Account");

            var user = UserManager.GetProfile(userId);

            UserManager.RemoveProfile(user.Id);

            return RedirectToAction("UserBeheer", "Account");
        }

        public ViewResult UserSettings()
        {
            IEnumerable<Item> Subscriptions = UserManager.GetProfile(User.Identity.GetUserId()).Subscriptions;
            return View(Subscriptions);
        }

        #endregion

        #region UserBeheer

        public ViewResult UserBeheer()
        {
            return View();
        }

        public ActionResult _UserTable()
        {
            string roleId = UserManager.GetAllRoles().Where(r => r.Name.Equals("User")).First().Id;
            IEnumerable<Profile> profiles = UserManager.GetProfiles().ToList()
                .Where(p => p.Roles.ToList().Any(r => r.RoleId.Equals(roleId)));
            return PartialView(profiles);
        }


        public ActionResult _AdminTable()
        {
            string roleId = UserManager.GetAllRoles().Where(r => r.Name.Equals("Admin")).First().Id;
            IEnumerable<Profile> profiles = UserManager.GetProfiles().ToList()
                .Where(p => p.Roles.ToList().Any(r => r.RoleId.Equals(roleId)));
            return PartialView(profiles);
        }

        public ActionResult _SuperAdminTable()
        {
            string roleId = UserManager.GetAllRoles().Where(r => r.Name.Equals("SuperAdmin")).First().Id;
            IEnumerable<Profile> profiles = UserManager.GetProfiles().ToList()
                .Where(p => p.Roles.ToList().Any(r => r.RoleId.Equals(roleId)));
            return PartialView(profiles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VoteToAdmin(string id)
        {
            UserManager.AddToRole(id, "Admin");
            UserManager.RemoveFromRole(id, "User");
            return RedirectToAction("UserBeheer", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromAdmin(string id)
        {
            UserManager.AddToRole(id, "User");
            UserManager.RemoveFromRole(id, "Admin");
            return RedirectToAction("UserBeheer", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeSuperAdmin(string id)
        {
            UserManager.AddToRole(id, "SuperAdmin");
            UserManager.RemoveFromRole(id, "Admin");
            return RedirectToAction("UserBeheer", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromSuperAdmin(string id)
        {
            UserManager.AddToRole(id, "User");
            UserManager.RemoveFromRole(id, "SuperAdmin");
            return RedirectToAction("UserBeheer", "Account");
        }

        public ActionResult _UserSettingDetails()
        {
            IEnumerable<UserSetting> settings = UserManager.GetProfile(User.Identity.GetUserId()).Settings;
            UserSettingViewModel oldSettings = new UserSettingViewModel
            {
                WANTS_EMAIL_NOTIFICATIONS = settings.ElementAt(0).boolValue,
                WANTS_ANDROID_NOTIFICATIONS =
                    settings.ElementAt(1).boolValue,
                WANTS_SITE_NOTIFICATIONS = settings.ElementAt(2).boolValue,
                WANTS_WEEKLY_REVIEW_VIA_MAIL = settings.ElementAt(4).boolValue
            };
            return PartialView(oldSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _UserSettingDetails(UserSettingViewModel newSettings)
        {
            IEnumerable<UserSetting> settings = UserManager.GetProfile(User.Identity.GetUserId()).Settings;
            Profile oldProfile = UserManager.GetProfile(User.Identity.GetUserId());
            Profile newProfile = null;
            newProfile = oldProfile;
            oldProfile.Settings.ElementAt(0).boolValue = newSettings.WANTS_EMAIL_NOTIFICATIONS;
            oldProfile.Settings.ElementAt(1).boolValue = newSettings.WANTS_ANDROID_NOTIFICATIONS;
            oldProfile.Settings.ElementAt(2).boolValue = newSettings.WANTS_SITE_NOTIFICATIONS;
            oldProfile.Settings.ElementAt(4).boolValue = newSettings.WANTS_WEEKLY_REVIEW_VIA_MAIL;
            UserManager.ChangeProfile(newProfile);
            return RedirectToAction("UserSettings", "Account");
        }

        public ActionResult EditUser(string id)
        {
            Profile profile = UserManager.GetProfile(id.ToString());
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(string id,  Profile newProfile)
        {
            Profile profile = UserManager.GetProfile(id);
           
            profile.Email = newProfile.Email;
            profile.UserName = newProfile.UserName;
            UserManager.ChangeProfile(profile); 
            return View(profile);
        }

        #endregion

        #region ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string subplatform, string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new { subplatform, returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string subplatform, string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToAction("Login", "Account", new { subplatform, returnUrl });

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
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
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(string subplatform,
            ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (ModelState.IsValid)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null) return RedirectToAction("Login", "Account");

                var name = info.Email.Split('@')[0];
                var user = new Profile
                {
                    UserName = name,
                    Email = model.Email,
                    ProfileIcon =
                        SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId,
                            Setting.Platform.DEFAULT_NEW_USER_ICON).Value,
                    CreatedOn = DateTime.Today
                };
                user.UserData = new UserData { Profile = user };
                user.Settings = new List<UserSetting>
                {
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.THEME,
                        Value = SubplatformMgr
                            .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DEFAULT_THEME).Value,
                        boolValue = false
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_ANDROID_NOTIFICATIONS,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_SITE_NOTIFICATIONS,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_EMAIL_NOTIFICATIONS,
                        Value = null, //moet nog boolean worden
                        boolValue = true
                    },
                    new UserSetting
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.WANTS_WEEKLY_REVIEW_VIA_MAIL,
                        Value = null, //moet nog boolean worden
                        boolValue = true
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
                        await SignInManager.SignInAsync(user, false, false);
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

        #region Export

        [HttpPost]
        public JsonResult Export()
        {
            IEnumerable<Profile> profiles = UserManager.GetProfiles().ToList();
            var serializerSettings = new JsonSerializerSettings();
            string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            string _path = Path.Combine(Server.MapPath("~/Content/Export/"), "Users.json");
            System.IO.File.WriteAllText(_path, json);
            return Json(new { fileName = "Users.json", errorMessage = "" });
        }

        [HttpGet]
        public ActionResult ExportUsers(string file)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Content/Export/"), file);
            return File(fullPath, "application/", file);
        }

        #endregion

        #region Helpers

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
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
                if (UserId != null) properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}