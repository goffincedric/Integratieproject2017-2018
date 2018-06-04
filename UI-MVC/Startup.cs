using FluentScheduler;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

[assembly: OwinStartup(typeof(UI_MVC.Startup))]
namespace UI_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            #region Background task scheduling
            SemaphoreSlim JobSemaphore = new SemaphoreSlim(1, 1);

            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            SubplatformManager subplatformMgr = new SubplatformManager(uowMgr);
            ItemManager itemMgr = new ItemManager(uowMgr);
            AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uowMgr.UnitOfWork), uowMgr);

            List<Subplatform> Subplatforms = subplatformMgr.GetSubplatforms().ToList();

            Subplatforms.ForEach(s =>
            {
                string seedInterval = s.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.SEED_INTERVAL_HOURS))?.Value;
                string alertGenerationInterval = s.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS))?.Value;
                string weeklyReviewsInterval = s.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS))?.Value;

                if (!(seedInterval is null))
                {
                    JobManager.AddJob(() =>
                    {
                        JobSemaphore.Wait();
                        try { itemMgr.CleanupOldRecords(s); }
                        finally
                        {
                            ItemManager.IsCleaning = false;
                            JobSemaphore.Release();
                        }

                        JobSemaphore.Wait();
                        try { itemMgr.SyncDatabase(s); }
                        finally
                        {
                            ItemManager.IsSyncing = false;
                            JobSemaphore.Release();
                        }
                    },
                    (schedule) => schedule
                    .ToRunOnceAt(1, 0)
                    .AndEvery(int.Parse(seedInterval)).Hours());
                }
                if (!(alertGenerationInterval is null))
                {
                    JobManager.AddJob(() =>
                    {
                        JobSemaphore.Wait();
                        try { accountMgr.GenerateAllAlerts(itemMgr.GetItems().Where(i => i.SubPlatforms.Contains(s))); }
                        finally
                        {
                            AccountManager.IsGeneratingAlerts = false;
                            JobSemaphore.Release();
                        }
                    },
                    (schedule) => schedule
                    .ToRunOnceAt(4, 0)
                    .AndEvery(int.Parse(alertGenerationInterval)).Hours());
                }
                DateTime dateToSendWeeklyReview = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);
                if (!(weeklyReviewsInterval is null))
                {
                    JobManager.AddJob(() =>
                    {
                        JobSemaphore.Wait();
                        try { accountMgr.SendWeeklyReviews(s); }
                        finally
                        {
                            AccountManager.IsSendingWeeklyReviews = false;
                            JobSemaphore.Release();
                        }
                    },
                    (schedule) => schedule
                    .ToRunOnceAt(dateToSendWeeklyReview.AddMinutes(30).AddHours(18))
                    .AndEvery(int.Parse(weeklyReviewsInterval)).Days());
                }
            });
            #endregion
        }
    }
}
