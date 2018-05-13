using PB.BL.Domain.Accounts;
using PB.DAL.EF;
using PB.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL
{
    public class AlertRepo : IAlertRepo
    {
        private readonly IntegratieDbContext ctx;

        public AlertRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public AlertRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public Alert CreatAlert(Alert alert)
        {
            alert = ctx.Alerts.Add(alert);

            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return alert;
        }

        public IEnumerable<Alert> CreatAlerts(List<Alert> alerts)
        {
            IEnumerable<Alert> newAlerts = ctx.Alerts.AddRange(alerts);
            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return newAlerts;
        }

        public void DeleteAlert(int alertId)
        {
            Alert alert = ReadAlert(alertId);
            if (alert != null)
            {
                ctx.Alerts.Remove(alert);
                ctx.SaveChanges();
            }
        }

        public Alert ReadAlert(int alertId)
        {
            return ctx.Alerts
                .Include(a => a.ProfileAlerts)
                .Include(a => a.Item)
                .FirstOrDefault(a => a.AlertId == alertId);
        }

        public IEnumerable<Alert> ReadAlerts()
        {
            return ctx.Alerts
                .Include(a => a.ProfileAlerts)
                .Include(a => a.Item)
                .AsEnumerable();
        }

        public void UpdateAlert(Alert alert)
        {
            alert = ctx.Alerts.Attach(alert);

            ctx.Entry(alert).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public ProfileAlert ReadProfileAlert(int profileAlertId)
        {
            return ctx.ProfileAlerts
                .Include(pa => pa.Alert)
                .Include(pa => pa.Alert.Item)
                .FirstOrDefault(pa => pa.ProfileAlertId == profileAlertId);
                
        }

        public IEnumerable<ProfileAlert> ReadProfileAlerts()
        {
            return ctx.ProfileAlerts
                .Include(pa => pa.Alert)
                .Include(pa => pa.Alert.Item)
                .AsEnumerable();
        }

        public IEnumerable<ProfileAlert> ReadProfileAlerts(string userId)
        {
            return ctx.ProfileAlerts
                .Include(pa => pa.Alert)
                .Include(pa => pa.Alert.Item)
                .Where(pa => pa.UserId.Equals(userId))
                .AsEnumerable();
        }
        
        public void UpdateProfileAlert(ProfileAlert profileAlert)
        {
            profileAlert = ctx.ProfileAlerts.Attach(profileAlert);

            ctx.Entry(profileAlert).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
