using System.Collections.Generic;
using System.Threading.Tasks;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;

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
        
        List<Alert> GenerateProfileAlerts(Profile profile);
        void GenerateAllAlerts(IEnumerable<Item> allItems);
        Task<List<Alert>> GenerateAllAlertsAsync(IEnumerable<Item> allItems);

        void SendWeeklyReviews(Subplatform subplatform);
        Task<Dictionary<Profile, List<ProfileAlert>>> SendWeeklyReviewsAsync(Subplatform subplatform);

        int GetUserCount();
    }
}