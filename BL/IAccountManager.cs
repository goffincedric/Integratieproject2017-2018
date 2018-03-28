using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Account;

namespace PB.BL
{
  public interface IAccountManager
  {
    IEnumerable<Profile> GetProfiles();
    Profile GetProfile(string username);
    Profile AddProfile(string username, string password, string hash, byte[] Salt, string email, Role role = Role.USER);
    void ChangeProfile(Profile profile);
    void RemoveProfile(string username);

    void LinkAlertsToProfile(List<Alert> alerts); 
  }
}
