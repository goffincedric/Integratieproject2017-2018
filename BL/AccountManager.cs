using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class AccountManager : IAccountManager
  {
    IProfileRepoHC ProfileRepo = new ProfileRepoHC();


    public Profile AddProfile(string username, string password, string email, Role role = Role.USER)
    {
      Profile profile = new Profile()
      {
        Username = username,
        Password = password,
        Email = email,
        Role = role
      };
      return this.AddProfile(profile);
    }

    private Profile AddProfile(Profile profile)
    {
      return ProfileRepo.CreateProfile(profile);
    }

    public void ChangeProfile(Profile profile)
    {
      ProfileRepo.UpdateProfile(profile);
    }

    public Profile GetProfile(string username)
    {
      return ProfileRepo.ReadProfile(username);
    }

    public IEnumerable<Profile> GetProfiles()
    {
      return ProfileRepo.ReadProfiles();
    }

    public void RemoveProfile(string username)
    {
      ProfileRepo.DeleteProfile(username);
    }

    public List<Alert> generateAlerts(Profile profile)
    {
      List<Alert> newAlerts = new List<Alert>();
      List<Item> currentAlerts = profile.Subscriptions;



      return newAlerts;
    }
  }
}
