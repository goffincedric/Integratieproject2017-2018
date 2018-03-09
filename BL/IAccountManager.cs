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
    IEnumerable<T> GetProfiles();
    T GetProfile(string username);
    T AddProfile(string username, string password, string email, Role role = Role.USER);
    void ChangeProfile(T profile);
    void RemoveProfile(string username);
  }
}
