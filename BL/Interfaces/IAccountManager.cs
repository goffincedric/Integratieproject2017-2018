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

        List<ProfileAlert> GetProfileAlerts(Subplatform subplatform, Profile profile);

        Profile AddSubscription(Profile profile, Item item);
        Profile RemoveSubscription(Profile profile, Item item);

        List<Alert> GenerateAllAlerts(IEnumerable<Item> allItems, out List<Item> itemsToUpdate);
        List<Alert> GenerateProfileAlerts(Profile profile);

        int GetUserCount();
    }
}
