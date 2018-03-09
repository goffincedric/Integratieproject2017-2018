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


    public T AddProfile(string username, string password, string email, Role role = Role.USER)
    {
      T profile = new T()
      {
        Username = username,
        Password = password,
        Email = email,
        Role = role
      };
      return this.AddProfile(profile)
    }

    public T AddProfile(T profile)
    {
      //return repo.CreateProfile(profile);
      return profile;
    }

    public void ChangeProfile(T profile)
    {
      throw new NotImplementedException();
    }

    public T GetProfile(string username)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<T> GetProfiles()
    {
      throw new NotImplementedException();
    }

    public void RemoveProfile(string username)
    {
      throw new NotImplementedException();
    }

    public List<Alert> generateAlerts(T profile)
    {
      List<Alert> newAlerts = new List<Alert>();
      List<Item> currentAlerts = profile.Subscriptions;

      return null;
    }
  }
}
