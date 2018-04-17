using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL
{
  public interface IProfileRepo
  {
    IEnumerable<Profile> ReadProfiles();
    Profile CreateProfile(Profile profile);
    Profile ReadProfile(string username);
    void UpdateProfile(Profile profile);
    void DeleteProfile(string username);
    
  }
}
