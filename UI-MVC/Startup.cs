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
using System.Web;

[assembly: OwinStartup(typeof(UI_MVC.Startup))]
namespace UI_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            #region Background Task Syncing
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            SubplatformManager subplatformMgr = new SubplatformManager(uowMgr);
            ItemManager itemMgr = new ItemManager(uowMgr);
            AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uowMgr.UnitOfWork), uowMgr);

            List<Subplatform> Subplatforms = subplatformMgr.GetSubplatforms().ToList();
            DateTime endDate = DateTime.Today.AddDays(1).AddHours(2);
            Subplatforms.ForEach(s =>
            {
                string seedInterval = s.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.SEED_INTERVAL_HOURS))?.Value;
                string alertGenerationInterval = s.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS))?.Value;
                string weeklyReviewsInterval = s.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_HOURS))?.Value;
                if (!(seedInterval is null))
                {
                    JobManager.AddJob(() =>
                    {
                        itemMgr.CleanupOldRecords(s);
                        itemMgr.SyncDatabase(s);
                    },
                    (schedule) => schedule
                    .ToRunOnceAt(2, new Random().Next(0, 10))
                    .AndEvery(int.Parse(seedInterval)).Hours());
                }
                if (!(alertGenerationInterval is null))
                {
                    JobManager.AddJob(() =>
                    {
                        accountMgr.GenerateAllAlerts(s.Items);
                    },
                    (schedule) => schedule
                    .ToRunOnceAt(2, new Random().Next(11, 20))
                    .AndEvery(int.Parse(alertGenerationInterval)).Hours());
                }
                if (!(weeklyReviewsInterval is null))
                {
                    JobManager.AddJob(() =>
                    {
                        accountMgr.GenerateAllAlerts(s.Items);
                    },
                    (schedule) => schedule
                    .ToRunOnceAt(2, new Random().Next(21, 30))
                    .AndEvery(int.Parse(weeklyReviewsInterval)).Hours());
                }
            });

            // TODO: INITIALIZE (TASKS NAAR REGISTRY ZETTEN + REGISTRY INITIALISEN) https://github.com/fluentscheduler/FluentScheduler
            #endregion
        }
    }
}
