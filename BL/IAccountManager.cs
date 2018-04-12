using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Account;
using Domain.Settings;

namespace PB.BL
{
    public interface IAccountManager
    {
        IEnumerable<Profile> GetProfiles();
        Profile GetProfile(string username);
        Profile AddProfile(string username, string email);
        void ChangeProfile(Profile profile);
        void RemoveProfile(string username);

        IEnumerable<UserSetting> GetUserSettings(string username);
        UserSetting GetUserSetting(string username, Setting.Account accountSetting);
        Profile AddUserSetting(string username, Setting.Account accountSetting, string value);
        void ChangeUserSetting(string username, UserSetting userSetting);
        void RemoveUserSetting(string username, Setting.Account accountSetting);

        void LinkAlertsToProfile(List<Alert> alerts);
    }
}
