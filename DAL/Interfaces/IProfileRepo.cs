using System.Collections.Generic;
using PB.BL.Domain.Accounts;

namespace PB.DAL
{
    public interface IProfileRepo
    {
        IEnumerable<Profile> ReadProfiles();
        Profile CreateProfile(Profile profile);
        Profile ReadProfile(string userId);
        void UpdateProfile(Profile profile);
        void UpdateProfiles(List<Profile> profiles);
        void DeleteProfile(string userId);
    }
}