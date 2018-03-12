using System;
using System.Collections.Generic;
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

    public Profile CreateProfile(Profile profile)
    {
      ctx.Profiles.Add(profile);
      return profile;
    }

    public void DeleteProfile(string username)
    {
      Profile profile = ReadProfile(username);
      if (profile != null) ctx.Profiles.Remove(profile);
    }

    public Profile ReadProfile(string username)
    {
      return ctx.Profiles.FirstOrDefault(p => p.Username == username);
    }

    public IEnumerable<Profile> ReadProfiles()
    {
      return ctx.Profiles.AsEnumerable();
    }

    public void UpdateProfile(Profile profile)
    {
      //MemoryRepo, alle objecten worden automatisch geüpdatet in het geheugen
    }
  }
}
