using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class SubplatformController : Controller
    {
        private readonly AccountManager accountMgr;
        private readonly DashboardManager dashboardMgr;
        private readonly ItemManager itemMgr;
        private readonly SubplatformManager SubplatformMgr;
        private readonly UnitOfWorkManager uow;

        public SubplatformController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            dashboardMgr = new DashboardManager(uow);
            accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
            SubplatformMgr = new SubplatformManager(uow);
            ViewBag.Home = SubplatformMgr.GetTag("Home").Text;
            ViewBag.Dashboard = SubplatformMgr.GetTag("Dashboard").Text;
            ViewBag.WeeklyReview = SubplatformMgr.GetTag("Weekly_Review").Text;
            ViewBag.MyAccount = SubplatformMgr.GetTag("Account").Text;
            ViewBag.More = SubplatformMgr.GetTag("More").Text;
            ViewBag.FAQ = SubplatformMgr.GetTag("FAQ").Text;
            ViewBag.Contact = SubplatformMgr.GetTag("Contact").Text;
            ViewBag.Legal = SubplatformMgr.GetTag("Legal").Text;
        }


        public ActionResult _ChangeHomePage(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);

            HomePageViewModel homePageViewModel = new HomePageViewModel
            {
                BannerTitle = SubplatformMgr.GetTag("BannerTitle").Text,
                BannerTextSub1 = SubplatformMgr.GetTag("BannerTextSub1").Text,
                BannerTextSub2 = SubplatformMgr.GetTag("BannerTextSub2").Text,
                Call_to_action = SubplatformMgr.GetTag("call-to-action-text").Text
            };

            return PartialView(homePageViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ChangeHomePage(HomePageViewModel homePageViewModel)
        {
            Tag BannerTitle = SubplatformMgr.GetTag("BannerTitle");
            Tag BannerTextSub1 = SubplatformMgr.GetTag("BannerTextSub1");
            Tag BannerTextSub2 = SubplatformMgr.GetTag("BannerTextSub2");
            Tag Call_to_action = SubplatformMgr.GetTag("call-to-action-text");

            BannerTitle.Text = homePageViewModel.BannerTitle;
            BannerTextSub1.Text = homePageViewModel.BannerTextSub1;
            BannerTextSub2.Text = homePageViewModel.BannerTextSub2;
            Call_to_action.Text = homePageViewModel.Call_to_action;


            SubplatformMgr.ChangeTag(BannerTitle);
            SubplatformMgr.ChangeTag(BannerTextSub1);
            SubplatformMgr.ChangeTag(BannerTextSub2);
            SubplatformMgr.ChangeTag(Call_to_action);
            return RedirectToAction("PlatformSettings", "Subplatform");
        }


        #region SubplatformSettings

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult PlatformSettings()
        {
            ViewBag.TotalUsers = accountMgr.GetUserCount().ToString();
            ViewBag.TotalPersons = itemMgr.GetPersonsCount().ToString();
            ViewBag.TotalOrganisations = itemMgr.GetOrganisationsCount().ToString();
            ViewBag.TotalThemes = itemMgr.GetThemesCount().ToString();
            ViewBag.TotalKeywords = itemMgr.GetKeywordsCount().ToString();
            ViewBag.TotalItems = itemMgr.GetItemsCount().ToString();
            ViewBag.IsSyncing = ItemManager.IsSyncing;
            ViewBag.IsCleaning = ItemManager.IsCleaning;
            ViewBag.IsGeneratingAlerts = AccountManager.IsGeneratingAlerts;
            ViewBag.IsSendingWeeklyReviews = AccountManager.IsSendingWeeklyReviews;
            return View();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _SubplatformSetting(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            SubplatformSettingViewModel huidige = new SubplatformSettingViewModel
            {
                APIsource = SubplatformMgr
                    .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOURCE_API_URL)?.Value,
                recordsBijhouden = int.Parse(SubplatformMgr
                    .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DAYS_TO_KEEP_RECORDS)?.Value),
                SocialSource =
                    SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOCIAL_SOURCE)
                        ?.Value,
                SocialSourceUrl =
                    SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SOCIAL_SOURCE_URL)
                        ?.Value,
                SiteName = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SITE_NAME)
                    ?.Value,
                Theme = SubplatformMgr.GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.DEFAULT_THEME)
                    ?.Value
            };
            return PartialView(huidige);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult SubplatformSetting(string subplatform,
            SubplatformSettingViewModel subplatformSettingViewModel)
        {
            Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
            List<SubplatformSetting> subplatformSettings = new List<SubplatformSetting>
            {
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SOCIAL_SOURCE,
                    Value = subplatformSettingViewModel.SocialSource,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                    Value = subplatformSettingViewModel.SocialSourceUrl,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SITE_NAME,
                    Value = subplatformSettingViewModel.SiteName,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.DEFAULT_THEME,
                    Value = subplatformSettingViewModel.Theme,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                    Value = subplatformSettingViewModel.recordsBijhouden.ToString(),
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SOURCE_API_URL,
                    Value = subplatformSettingViewModel.APIsource,
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SEED_INTERVAL_HOURS,
                    Value = "24",
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS,
                    Value = "24",
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS,
                    Value = "7",
                    IsEnabled = true,
                    Subplatform = subplatformToChange
                }
            };
            SubplatformMgr.ChangeSubplatformSettings(subplatformToChange, subplatformSettings);
            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _SeedIntervals(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            SeedIntervals huidige = new SeedIntervals
            {
                SEED_INTERVAL_HOURS = int.Parse(SubplatformMgr
                    .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.SEED_INTERVAL_HOURS).Value),
                SEND_WEEKLY_REVIEWS_INTERVAL_DAYS = int.Parse(SubplatformMgr
                    .GetSubplatformSetting(Subplatform.SubplatformId,
                        Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS).Value),
                ALERT_GENERATION_INTERVAL_HOURS = int.Parse(SubplatformMgr
                    .GetSubplatformSetting(Subplatform.SubplatformId, Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS)
                    .Value)
            };
            return PartialView(huidige);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult SeedIntervals(string subplatform, SeedIntervals intervals)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            List<SubplatformSetting> subplatformSettings = new List<SubplatformSetting>
            {
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS,
                    Value = intervals.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS.ToString(),
                    IsEnabled = true,
                    Subplatform = Subplatform
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.SEED_INTERVAL_HOURS,
                    Value = intervals.SEED_INTERVAL_HOURS.ToString(),
                    IsEnabled = true,
                    Subplatform = Subplatform
                },
                new SubplatformSetting
                {
                    SettingName = Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS,
                    Value = intervals.ALERT_GENERATION_INTERVAL_HOURS.ToString(),
                    IsEnabled = true,
                    Subplatform = Subplatform
                }
            };

            SubplatformMgr.ChangeSubplatformSettings(Subplatform, subplatformSettings);
            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        #endregion

        #region DeploySubplatform

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _DeploySubplatform()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult DeploySubplatform(SubplatformViewModel subplatformViewModel)
        {
            if (ModelState.IsValid)
                SubplatformMgr.AddSubplatform(subplatformViewModel.Name, subplatformViewModel.Url,
                    subplatformViewModel.SourceAPI);
            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        #endregion

        #region Uploads

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult UploadSiteLogo(string subplatform, FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (fileViewModel.file != null)
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(fileViewModel.file.FileName);
                        var name = "Site_Logo";
                        var newName = name + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Site/"), newName);
                        fileViewModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Site/" + newName;
                        SubplatformMgr.ChangeSubplatformSetting(subplatformToChange, new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SITE_ICON_URL,
                            Value = iconUrl,
                            IsEnabled = true,
                            Subplatform = subplatformToChange
                        });
                    }

                return RedirectToAction("PlatformSettings", "Home");
            }

            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult UploadDefaultItemLogo(string subplatform, FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (fileViewModel.file != null)
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(fileViewModel.file.FileName);
                        var name = "Default_Item_Logo";
                        var newName = name + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Site/"), newName);
                        fileViewModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Site/" + newName;
                        SubplatformMgr.ChangeSubplatformSetting(subplatformToChange, new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                            Value = iconUrl,
                            IsEnabled = true,
                            Subplatform = subplatformToChange
                        });
                    }

                return RedirectToAction("PlatformSettings", "Home");
            }

            return RedirectToAction("PlatformSettings", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult UploadDefaultUserLogo(string subplatform, FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform subplatformToChange = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (fileViewModel.file != null)
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(fileViewModel.file.FileName);
                        var name = "Default_Item_Logo";
                        var newName = name + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Users/"), newName);
                        fileViewModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Users/" + newName;
                        SubplatformMgr.ChangeSubplatformSetting(subplatformToChange, new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                            Value = iconUrl,
                            IsEnabled = true,
                            Subplatform = subplatformToChange
                        });
                    }

                return RedirectToAction("PlatformSettings", "Home");
            }

            return RedirectToAction("PlatformSettings", "Home");
        }

        #endregion

        #region AdminSubplatformTools

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult GenerateAlertsManually()
        {
            if (!AccountManager.IsGeneratingAlerts)
            {
                AccountManager.IsGeneratingAlerts = true;
                accountMgr.GenerateAllAlertsAsync(itemMgr.GetItems()).GetAwaiter().OnCompleted(new System.Action(() =>
                {
                    AccountManager.IsGeneratingAlerts = false;
                }));
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult CleanupDB(string subplatform)
        {
            if (!ItemManager.IsCleaning)
            {
                ItemManager.IsCleaning = true;
                Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.CleanupOldRecordsAsync(sp).GetAwaiter().OnCompleted(new System.Action(() =>
                {
                    ItemManager.IsCleaning = false;
                }));
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult Syncronize(string subplatform)
        {
            if (!ItemManager.IsSyncing)
            {
                // Set IsSyncing field
                ItemManager.IsSyncing = true;
                Subplatform sp = SubplatformMgr.GetSubplatform(subplatform);
                // TODO: Tasking met JobManager
                itemMgr.SyncDatabaseAsync(sp).GetAwaiter().OnCompleted(new System.Action(() =>
                {
                    ItemManager.IsSyncing = false;
                }));
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult SendWeeklyReviews(string subplatform)
        {
            if (!AccountManager.IsSendingWeeklyReviews)
            {
                // Set IsGeneratingAlerts flag
                AccountManager.IsSendingWeeklyReviews = true;
                accountMgr.SendWeeklyReviewsAsync().GetAwaiter().OnCompleted(new System.Action(() =>
                {
                    AccountManager.IsSendingWeeklyReviews = false;
                }));
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        #endregion
    }
}