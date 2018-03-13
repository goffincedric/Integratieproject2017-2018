using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Account;

namespace PB.DAL
{
  public class ProfileRepoHC : IProfileRepoHC
  {
    private List<Profile> repoProfiles = new List<Profile>();

    public Profile CreateProfile(Profile profile)
    {
      repoProfiles.Add(profile);
      return profile;
    }

    public void DeleteProfile(string username)
    {
      Profile profile = ReadProfile(username);
      if (profile != null) repoProfiles.Remove(profile);
    }

    public Profile ReadProfile(string username)
    {
      return repoProfiles.FirstOrDefault(p => p.Username == username);
    }

    public IEnumerable<Profile> ReadProfiles()
    {
      return repoProfiles.AsEnumerable();
    }

    public void UpdateProfile(Profile profile)
    {
      //MemoryRepo, alle objecten worden automatisch geüpdatet in het geheugen
    }
  }
}
