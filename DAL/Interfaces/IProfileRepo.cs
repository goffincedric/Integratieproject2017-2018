using PB.BL.Domain.Account;
using System.Collections.Generic;

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
