using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Accounts;
using PB.DAL.EF;

namespace PB.DAL
{
    public class ProfileRepo : UserStore<Profile>, IProfileRepo
    {
        private readonly IntegratieDbContext ctx;

        public ProfileRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public ProfileRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
        }


        public Profile CreateProfile(Profile profile)
        {
            profile = ctx.Users.Add(profile);

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
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                }
            }

            return profile;
        }

        public void DeleteProfile(string userId)
        {
            Profile profile = ReadProfile(userId);
            if (profile != null)
            {
                ctx.Users.Remove(profile);
                ctx.SaveChanges();
            }
        }

        public Profile ReadProfile(string userId)
        {
            return ctx.Users
                .Include(p => p.UserData)
                .Include(p => p.Settings)
                .Include(p => p.ProfileAlerts)
                .Include(p => p.Subscriptions)
                .Include(p => p.Dashboards)
                .Include(p => p.WeeklyReviews)
                .Include(p => p.Roles)
                .FirstOrDefault(p => p.Id.Equals(userId));
        }

        public IEnumerable<Profile> ReadProfiles()
        {
            return ctx.Users
                .Include(p => p.UserData)
                .Include(p => p.Settings)
                .Include(p => p.ProfileAlerts)
                .Include(p => p.Subscriptions)
                .Include(p => p.Dashboards)
                .Include(p => p.WeeklyReviews)
                .Include(p => p.Roles)
                .AsEnumerable();
        }

        public void UpdateProfile(Profile profile)
        {
            profile = ctx.Users.Attach(profile);

            ctx.Entry(profile).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void UpdateProfiles(List<Profile> profiles)
        {
            profiles.ForEach(p =>
            {
                p = ctx.Users.Attach(p);
                ctx.Entry(p).State = EntityState.Modified;
            });

            ctx.SaveChanges();
        }
    }
}