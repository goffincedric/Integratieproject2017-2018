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
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            SubplatformManager subplatformMgr = new SubplatformManager(uowMgr);
            ItemManager itemMgr = new ItemManager(uowMgr);
            AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uowMgr.UnitOfWork), uowMgr);
            SemaphoreSlim JobSemaphore = new SemaphoreSlim(1, 1); // alle semaphores naar 1 semafoor omzette

            List<Subplatform> Subplatforms = subplatformMgr.GetSubplatforms().ToList();
            DateTime endDate = DateTime.Today.AddDays(1).AddHours(2);

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
                    //.ToRunOnceAt(9, 10)
                    //.AndEvery(int.Parse(seedInterval)).Hours());
                    .ToRunOnceAt(DateTime.Now.AddMinutes(2))
                    .AndEvery(10).Minutes());
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
                    //.ToRunOnceAt(9, 30)
                    //.AndEvery(int.Parse(alertGenerationInterval)).Hours());
                    .ToRunOnceAt(DateTime.Now.AddMinutes(5))
                    .AndEvery(10).Minutes());
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
                    //.ToRunOnceAt(dateToSendWeeklyReview.AddMinutes(new Random().Next(50, 60)))
                    //.AndEvery(int.Parse(weeklyReviewsInterval)).Hours());
                    .ToRunOnceAt(DateTime.Now.AddMinutes(8))
                    .AndEvery(10).Minutes());
                }
            });
            #endregion
        }
    }
}
