using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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

            if (System.Web.HttpContext.Current.Request.Url.Segments.Count() > 1)
            {
                Subplatform subplatform = SubplatformMgr.GetSubplatform(System.Web.HttpContext.Current.Request.Url.Segments[1].Trim('/'));

                IEnumerable<Tag> menuTags = subplatform.Pages.SingleOrDefault(p => p.PageName.Equals("Menu"))?.Tags;
                if (menuTags is null || menuTags.Count() == 0) return;
                ViewBag.Home = menuTags.SingleOrDefault(t => t.Name.Equals("Home"))?.Text ?? "Home";
                ViewBag.Dashboard = menuTags.SingleOrDefault(t => t.Name.Equals("Dashboard"))?.Text ?? "Dashboard";
                ViewBag.WeeklyReview = menuTags.SingleOrDefault(t => t.Name.Equals("Weekly_Review"))?.Text ?? "Weekly Review";
                ViewBag.MyAccount = menuTags.SingleOrDefault(t => t.Name.Equals("Account"))?.Text ?? "Account";
                ViewBag.More = menuTags.SingleOrDefault(t => t.Name.Equals("More"))?.Text ?? "More";
                ViewBag.FAQ = menuTags.SingleOrDefault(t => t.Name.Equals("FAQ"))?.Text ?? "FAQ";
                ViewBag.Contact = menuTags.SingleOrDefault(t => t.Name.Equals("Contact"))?.Text ?? "Contact";
                ViewBag.Legal = menuTags.SingleOrDefault(t => t.Name.Equals("Legal"))?.Text ?? "Legal";
                ViewBag.Items = menuTags.SingleOrDefault(t => t.Name.Equals("Items"))?.Text ?? "Items";
                ViewBag.Persons = menuTags.SingleOrDefault(t => t.Name.Equals("Persons"))?.Text ?? "Persons";
                ViewBag.Organisations = menuTags.SingleOrDefault(t => t.Name.Equals("Organisations"))?.Text ?? "Organisations";
                ViewBag.Themes = menuTags.SingleOrDefault(t => t.Name.Equals("Themes"))?.Text ?? "Themes";
            }
        }
        
        public ActionResult _ChangeHomePage(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            List<Tag> tags = Subplatform.Pages.SingleOrDefault(p => p.PageName.Equals("Home"))?.Tags;

            HomePageViewModel homePageViewModel = new HomePageViewModel
            {
                BannerTitle = tags.SingleOrDefault(t => t.Name.Equals("BannerTitle")).Text,
                BannerTextSub1 = tags.SingleOrDefault(t => t.Name.Equals("BannerTextSub1")).Text,
                BannerTextSub2 = tags.SingleOrDefault(t => t.Name.Equals("BannerTextSub2")).Text,
                Call_to_action = tags.SingleOrDefault(t => t.Name.Equals("call-to-action-text")).Text
            };

            return PartialView(homePageViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ChangeHomePage(string subplatform, HomePageViewModel homePageViewModel)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);

            if (Subplatform is null) return HttpNotFound();
            List<Tag> tags = Subplatform.Pages.SingleOrDefault(p => p.PageName.Equals("Home"))?.Tags;
            tags.SingleOrDefault(t => t.Name.Equals("BannerTitle")).Text = homePageViewModel.BannerTitle;
            tags.SingleOrDefault(t => t.Name.Equals("BannerTextSub1")).Text = homePageViewModel.BannerTextSub1;
            tags.SingleOrDefault(t => t.Name.Equals("BannerTextSub2")).Text = homePageViewModel.BannerTextSub2;
            tags.SingleOrDefault(t => t.Name.Equals("call-to-action-text")).Text = homePageViewModel.Call_to_action;

            SubplatformMgr.ChangeTags(tags);
            return RedirectToAction("PlatformSettings", "Subplatform");
        }


        #region SubplatformSettings

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult PlatformSettings(string subplatform)
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
            ViewBag.Subplatform = SubplatformMgr.GetSubplatform(subplatform).URL;
            ViewBag.SubplatformId = SubplatformMgr.GetSubplatform(subplatform).SubplatformId;

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

                return RedirectToAction("PlatformSettings", "Subplatform");
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
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

                return RedirectToAction("PlatformSettings", "Subplatform");
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
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

                return RedirectToAction("PlatformSettings", "Subplatform");
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
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
                Task.Run(async () =>
                {
                    await accountMgr.GenerateAllAlertsAsync(itemMgr.GetItems());
                    AccountManager.IsGeneratingAlerts = false;
                });
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult CleanupDB(string subplatform)
        {
            if (!ItemManager.IsCleaning)
            {
                // Set IsCleaning flag
                ItemManager.IsCleaning = true;
                Task.Run(async () =>
                {
                    await itemMgr.CleanupOldRecordsAsync(SubplatformMgr.GetSubplatform(subplatform));
                    ItemManager.IsCleaning = false;
                });
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult Syncronize(string subplatform)
        {
            if (!ItemManager.IsSyncing)
            {
                // Set IsSyncing flag
                ItemManager.IsSyncing = true;
                Task.Run(async () =>
                {
                    await itemMgr.SyncDatabaseAsync(SubplatformMgr.GetSubplatform(subplatform));
                    ItemManager.IsSyncing = false;
                });
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
                Task.Run(async () =>
                {
                    await accountMgr.SendWeeklyReviewsAsync(SubplatformMgr.GetSubplatform(subplatform));
                    AccountManager.IsSendingWeeklyReviews = false;
                });
            }

            return RedirectToAction("PlatformSettings", "Subplatform");
        }



        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ShowSubplatform()
        {
            IEnumerable<Subplatform> subplatforms = SubplatformMgr.GetSubplatforms();

            return PartialView(subplatforms);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult EditSubplatform(int id)
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(id);

            return PartialView(subplatform);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public ActionResult EditSubplatform(Subplatform newsubplatform)
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(newsubplatform.SubplatformId);

            subplatform.Name = newsubplatform.Name;
            subplatform.URL = newsubplatform.URL;
            SubplatformMgr.ChangeSubplatform(subplatform);

            return PartialView(subplatform);
        }

        #endregion
    }
}