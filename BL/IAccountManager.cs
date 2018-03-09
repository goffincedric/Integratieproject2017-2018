using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public interface IAccountManager
  {
    IEnumerable<Profile> GetProfiles();
    Profile GetProfile(string username);
    Profile AddProfile(string username, string password, Role role = Role.USER);
    void ChangeProfile(Profile profile);
    /// <summary>
    /// ///
    /// </summary>
    /// <param name="username"></param>
    void RemoveProfile(string username);
  }
}
