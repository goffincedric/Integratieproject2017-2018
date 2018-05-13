using PB.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI_MVC.Models;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System.IO;

namespace UI_MVC.Controllers
{
    public class SubplatformController : Controller
    {
        private readonly UnitOfWorkManager uow;
        private readonly SubplatformManager SubplatformMgr;
        private readonly ItemManager itemMgr;
        private readonly DashboardManager dashboardMgr;
        private readonly AccountManager accountMgr;

        public SubplatformController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            dashboardMgr = new DashboardManager(uow);
            accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
            SubplatformMgr = new SubplatformManager(uow);
        }

        public ActionResult PlatformSettings()
        {
            ViewBag.TotalUsers = accountMgr.GetUserCount().ToString();
            ViewBag.TotalPersons = itemMgr.GetPersonsCount().ToString();
            ViewBag.TotalOrganisations = itemMgr.GetOrganisationsCount().ToString();
            ViewBag.TotalThemes = itemMgr.GetThemesCount().ToString();
            ViewBag.TotalKeywords = itemMgr.GetKeywordsCount().ToString();
            ViewBag.TotalItems = itemMgr.GetItemsCount().ToString();
            ViewBag.IsSyncing = ItemManager.IsSyncing;
            return View();
        }

        public ActionResult _DeploySubplatform()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult DeploySubplatform(SubplatformViewModel subplatformViewModel)
        {
            if (ModelState.IsValid)
            {
                SubplatformMgr.AddSubplatform(subplatformViewModel.Name, subplatformViewModel.Url, subplatformViewModel.SourceAPI);
            }
            return RedirectToAction("PlatformSettings", "Subplatform");
        }


        public ActionResult _SubplatformSetting(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            SubplatformSettingViewModel huidige = new SubplatformSettingViewModel
            {
                APIsource = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOURCE_API_URL)?.Value,
                recordsBijhouden = Int32.Parse(SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DAYS_TO_KEEP_RECORDS)?.Value),
                SocialSource = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOCIAL_SOURCE)?.Value,
                SocialSourceUrl = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOCIAL_SOURCE_URL)?.Value,
                SiteName = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME)?.Value,
                Theme = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DEFAULT_THEME)?.Value
            };
            return PartialView(huidige);
        }

        [HttpPost]
        public ActionResult SubplatformSetting(string subplatform, SubplatformSettingViewModel subplatformSettingViewModel)
        {
            Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);

            List<SubplatformSetting> subplatformSettings = new List<SubplatformSetting>()
            {
                new SubplatformSetting()
                {
                    SettingName = Setting.Platform.SOCIAL_SOURCE,
                    Value = subplatformSettingViewModel.SocialSource,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting()
                {
                    SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                    Value = subplatformSettingViewModel.SocialSourceUrl,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting()
                {
                    SettingName = Setting.Platform.SITE_NAME,
                    Value = subplatformSettingViewModel.SiteName,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting()
                {
                    SettingName = Setting.Platform.DEFAULT_THEME,
                    Value = subplatformSettingViewModel.Theme,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting()
                {
                    SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                    Value = subplatformSettingViewModel.recordsBijhouden.ToString(),
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting()
                {
                    SettingName = Setting.Platform.SOURCE_API_URL,
                    Value = subplatformSettingViewModel.APIsource,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                }
            };

            SubplatformMgr.ChangeSubplatformSettings(subplatformToChange, subplatformSettings);

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [HttpPost]
        public ActionResult UploadSiteLogo(string subplatform, FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (fileViewModel.file != null)
                {
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(fileViewModel.file.FileName);

                        var name = "Site_Logo";
                        var newName = name + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Site/"), newName);
                        fileViewModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Site/" + newName;
                        SubplatformMgr.ChangeSubplatformSetting(subplatformToChange, new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SITE_ICON_URL,
                            Value = iconUrl,
                            IsEnabled = true,
                            Subplatform = subplatformToChange
                        });
                    }
                }

                return RedirectToAction("PlatformSettings", "Home");

            }

            // Show errors on page
            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        public ActionResult UploadDefaultItemLogo(string subplatform, FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (fileViewModel.file != null)
                {
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(fileViewModel.file.FileName);

                        var name = "Default_Item_Logo";
                        var newName = name + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Site/"), newName);
                        fileViewModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Site/" + newName;
                        SubplatformMgr.ChangeSubplatformSetting(subplatformToChange, new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                            Value = iconUrl,
                            IsEnabled = true,
                            Subplatform = subplatformToChange
                        });
                    }
                }

                return RedirectToAction("PlatformSettings", "Home");
            }

            // Show errors on page
            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        public ActionResult UploadDefaultUserLogo(string subplatform, FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (fileViewModel.file != null)
                {
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(fileViewModel.file.FileName);

                        var name = "Default_Item_Logo";
                        var newName = name + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Users/"), newName);
                        fileViewModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Users/" + newName;
                        SubplatformMgr.ChangeSubplatformSetting(subplatformToChange, new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                            Value = iconUrl,
                            IsEnabled = true,
                            Subplatform = subplatformToChange
                        });
                    }
                }

                return RedirectToAction("PlatformSettings", "Home");
            }

            // Show errors on page
            return RedirectToAction("PlatformSettings", "Home");
        }
    }
}