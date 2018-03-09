using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class AccountManager : IAccountManager
  {


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

    public Profile AddProfile(Profile profile)
    {
      //return repo.CreateProfile(profile);
      return profile;
    }

    public void ChangeProfile(Profile profile)
    {
      throw new NotImplementedException();
    }

    public Profile GetProfile(string username)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Profile> GetProfiles()
    {
      throw new NotImplementedException();
    }

    public void RemoveProfile(string username)
    {
      throw new NotImplementedException();
    }

    public List<Alert> generateAlerts(Profile profile)
    {
      List<Alert> newAlerts = new List<Alert>();
      List<Item> currentAlerts = profile.Subscriptions;

      return null;
    }
  }
}
