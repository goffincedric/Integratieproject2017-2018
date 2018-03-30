using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Account;
using PB.DAL.EF;

namespace PB.DAL
{
  public class ProfileRepo : IProfileRepo
  {
    private IntegratieDbContext ctx;

    public ProfileRepo()
    {
      ctx = new IntegratieDbContext();
    }

    public ProfileRepo(UnitOfWork uow)
    {
      ctx = uow.Context;
      Console.WriteLine("UOW MADE PROFILEREPO");
    }

    public Profile CreateProfile(Profile profile)
    {
      ctx.Profiles.Add(profile);

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
        ctx.Profiles.Remove(profile);
        ctx.SaveChanges();
      }
    }

<<<<<<< HEAD
        public Profile ReadProfile(string username)
        {
            return ctx.Profiles
                .Include("Alerts")
                .Include("Subscriptions")
                .FirstOrDefault(p => p.Username == username);
        }

        public IEnumerable<Profile> ReadProfiles()
        {
            return ctx.Profiles
                .Include("Alerts")
                .Include("Subscriptions")
                .AsEnumerable();
        }
=======
    public Profile ReadProfile(string username)
    {
      return ctx.Profiles
          .Include("Alerts")
          .Include("Subscriptions")
          .FirstOrDefault(p => p.Username == username);
    }

    public IEnumerable<Profile> ReadProfiles()
    {
      return ctx.Profiles
          .Include("Alerts")
          .Include("Subscriptions")
          .AsEnumerable();
    }
>>>>>>> master

    public void UpdateProfile(Profile profile)
    {
      ctx.Profiles.Attach(profile);
      ctx.Entry(profile).State = System.Data.Entity.EntityState.Modified;
      ctx.SaveChanges();
    }
  }
}
