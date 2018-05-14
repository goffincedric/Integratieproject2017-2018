using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
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

        ProfileAlert GetProfileAlert(int profileAlertId);
        List<ProfileAlert> GetSiteProfileAlerts(Subplatform subplatform, string userId);
        List<ProfileAlert> GetWebAPIProfileAlerts(Subplatform subplatform, string userId);
        void ChangeProfileAlert(ProfileAlert profileAlert);

        Profile AddSubscription(Profile profile, Item item);
        Profile RemoveSubscription(Profile profile, Item item);

        Dictionary<Profile, List<ProfileAlert>> SendWeeklyReviews();
        List<Alert> GenerateAllAlerts(IEnumerable<Item> allItems);
        List<Alert> GenerateProfileAlerts(Profile profile);

        int GetUserCount();
    }
}
