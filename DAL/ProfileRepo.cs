using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Account;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

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
            //Console.WriteLine("UOW MADE PROFILEREPO");
        }


        public Profile CreateProfile(Profile profile)
        {
            ctx.Users.Add(profile);

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
            return profile;
        }

        public void DeleteProfile(string username)
        {
            Profile profile = ReadProfile(username);
            if (profile != null)
            {
                UserData data = profile.UserData;
                ctx.Users.Remove(profile);
                ctx.UserData.Remove(data);
                ctx.SaveChanges();
            }
        }

        public Profile ReadProfile(string username)
        {
            return ctx.Users
                .Include("UserData")
                .Include("Alerts")
                .Include("Subscriptions")
                .Include("Settings")
                .FirstOrDefault(p => p.UserName == username);
        }

        public IEnumerable<Profile> ReadProfiles()
        {
            return ctx.Users
                .Include("UserData")
                .Include("Alerts")
                .Include("Subscriptions")
                .Include("Settings")
                .AsEnumerable();
        }

        public void UpdateProfile(Profile profile)
        {
            profile = ctx.Users.Attach(profile);

            ctx.Entry(profile).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public int ReadProfileCount()
        {
            return ctx.Users.Count();
        }
    }
}
