using PB.BL.Domain.Account;
using PB.BL.Domain.Settings;
using System.Collections.Generic;

namespace PB.BL.Interfaces
{
    public interface IAccountManager
    {
        IEnumerable<Profile> GetProfiles();
        Profile GetProfile(string userId);
        //Profile AddProfile(string username, string email);
        void ChangeProfile(Profile profile);
        void RemoveProfile(string userId);

        IEnumerable<UserSetting> GetUserSettings(string userId);
        UserSetting GetUserSetting(string userId, Setting.Account accountSetting);
        Profile AddUserSetting(string userId, Setting.Account accountSetting, string value);
        void ChangeUserSetting(string userId, UserSetting userSetting);
        //void RemoveUserSetting(string username, Setting.Account accountSetting);

        void LinkAlertsToProfile(List<Alert> alerts);

        int GetUserCount();
    }
}
