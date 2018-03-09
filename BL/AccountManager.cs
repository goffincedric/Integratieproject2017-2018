using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class AccountManager : IAccountManager
  {
    public Profile AddProfile(string username, string password, Role role = Role.USER)
    {
      throw new NotImplementedException();
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


  }
}
