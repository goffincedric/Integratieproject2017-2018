using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    /// <summary>   
    /// Controller that has control over the persistence of elements, dashboards and zones
    /// Authorized by all roles
    /// </summary
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly UnitOfWorkManager uow;
        private readonly ItemManager itemMgr;
        private readonly DashboardManager dashboardMgr;
        private readonly AccountManager accountMgr;
        private readonly SubplatformManager SubplatformMgr;


        public DashboardController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            dashboardMgr = new DashboardManager(uow);
            accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
            SubplatformMgr = new SubplatformManager(uow);
        }

        //TODO: Delete zones created in beginning
        public ActionResult Dashboard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var user = accountMgr.GetProfile(User.Identity.GetUserId());

            Dashboard model = dashboardMgr.GetDashboards().FirstOrDefault(d => d.Profile.Id == user.Id && d.Subplatform.URL.ToLower().Equals(subplatform.ToLower()));
            if (model == null)
            {
                model = new Dashboard()
                {
                    Profile = user,
                    DashboardType = UserType.USER,
                    Subplatform = Subplatform,
                    Zones = new List<Zone>()
                    //{
                    //    new Zone()
                    //    {
                    //        Title = "Main Trends",
                    //        Elements = new List<Element>()
                    //        {
                    //            new Element()
                    //            {
                    //                X = 0,
                    //                Y = 3,
                    //                Width = 5,
                    //                Height = 5,
                    //                Comparison = new Comparison()
                    //            },
                    //            new Element()
                    //            {
                    //                X = 0,
                    //                Y = 0,
                    //                Width = 2,
                    //                Height = 3,
                    //                Comparison = new Comparison()
                    //            }
                    //        }
                    //    },
                    //    new Zone()
                    //    {
                    //        Title = "Personal Trends",
                    //        Elements = new List<Element>()
                    //        {
                    //            new Element()
                    //            {
                    //                X = 0,
                    //                Y = 3,
                    //                Width = 6,
                    //                Height = 5,
                    //                Comparison = new Comparison()
                    //            },
                    //            new Element()
                    //            {
                    //                X = 0,
                    //                Y = 0,
                    //                Width = 2,
                    //                Height = 4,
                    //                Comparison = new Comparison()
                    //            }
                    //        }
                    //    }
                    //}
                };
                model = dashboardMgr.AddDashboard(model.Subplatform, model.Profile, model.DashboardType, model.Zones);
            };

            return View(model);
        }


        public ActionResult Wizard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            return View();
        }

        public ActionResult _Wizard()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(AccountEditModel editedAccount)
        {
            //string _FileName = "";
            //Profile newProfile = UserManager.GetProfile(User.Identity.GetUserId());

            //if (editedAccount.file != null)
            //{
            //    if (editedAccount.file.ContentLength > 0)
            //    {
            //        _FileName = Path.GetFileName(editedAccount.file.FileName);

            //        var username = newProfile.UserName.ToString();
            //        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
            //        string _path = Path.Combine(Server.MapPath("~/Content/Images/Users/"), newName);
            //        editedAccount.file.SaveAs(_path);
            //        newProfile.ProfileIcon = @"~/Content/Images/Users/" + newName;
            //    }
            //}
            //else
            //{
            //    newProfile.ProfileIcon = newProfile.ProfileIcon;
            //}

            //newProfile.UserData.LastName = editedAccount.LastName;
            //newProfile.UserData.FirstName = editedAccount.FirstName;
            //newProfile.Email = editedAccount.Email;
            ////newProfile.UserData.Telephone = editedAccount.Telephone;
            ////newProfile.UserData.Gender = editedAccount.Gender;
            //newProfile.UserData.Street = editedAccount.Street;
            //newProfile.UserData.City = editedAccount.City;
            //newProfile.UserData.Province = editedAccount.Province;
            //newProfile.UserData.PostalCode = editedAccount.PostalCode;



            //if (ModelState.IsValid)
            //{
            //    UserManager.ChangeProfile(newProfile);
            //    return RedirectToAction("Account", "Account");
            //}
            return View();
        }



        public ActionResult _DeploySubplatform()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult DeploySubplatform(SubplatformViewModel subplatformViewModel)
        {
            SubplatformMgr.AddSubplatform(subplatformViewModel.name, subplatformViewModel.url, subplatformViewModel.sourceAPI);
            return RedirectToAction("PlatformSettings", "Home");
        }


        public ActionResult _SubplatformSetting(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            SubplatformSettingViewModel huidige = new SubplatformSettingViewModel();
            huidige.APIsource = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOURCE_API_URL).Value;
            huidige.recordsBijhouden = Int32.Parse(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DAYS_TO_KEEP_RECORDS).Value);
            huidige.SocialSource = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOCIAL_SOURCE).Value;
            huidige.SocialSourceUrl = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOCIAL_SOURCE_URL).Value;
            huidige.SiteName = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME).Value;
            huidige.Theme = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DEFAULT_THEME).Value;
            return PartialView(huidige);
        }

        [HttpPost]
        public ActionResult SubplatformSetting(string subplatform, SubplatformSettingViewModel subplatformSettingViewModel)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);

            SubplatformMgr.ChangeSubplatformSetting(subplatform, new SubplatformSetting()
            {
                SettingName = Setting.Platform.SOCIAL_SOURCE,
                Value = subplatformSettingViewModel.SocialSource,
                IsEnabled = true,
                Subplatform = Subplatform
            });
            SubplatformMgr.ChangeSubplatformSetting(subplatform, new SubplatformSetting()
            {
                SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                Value = subplatformSettingViewModel.SocialSourceUrl,
                IsEnabled = true,
                Subplatform = Subplatform
            });
            SubplatformMgr.ChangeSubplatformSetting(subplatform, new SubplatformSetting()
            {
                SettingName = Setting.Platform.SITE_NAME,
                Value = subplatformSettingViewModel.SiteName,
                IsEnabled = true,
                Subplatform = Subplatform
            });
            SubplatformMgr.ChangeSubplatformSetting(subplatform, new SubplatformSetting()
            {
                SettingName = Setting.Platform.DEFAULT_THEME,
                Value = subplatformSettingViewModel.Theme,
                IsEnabled = true,
                Subplatform = Subplatform
            });
            SubplatformMgr.ChangeSubplatformSetting(subplatform, new SubplatformSetting()
            {
                SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                Value = subplatformSettingViewModel.recordsBijhouden.ToString(),
                IsEnabled = true,
                Subplatform = Subplatform
            });
            SubplatformMgr.ChangeSubplatformSetting(subplatform, new SubplatformSetting()
            {
                SettingName = Setting.Platform.SOURCE_API_URL,
                Value = subplatformSettingViewModel.APIsource,
                IsEnabled = true,
                Subplatform = Subplatform
            });

          
            return RedirectToAction("PlatformSettings", "Home");
        }
    }
}