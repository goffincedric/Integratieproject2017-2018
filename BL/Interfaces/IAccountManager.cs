<<<<<<< HEAD
﻿using Domain.Settings;
using PB.BL.Domain.Account;
=======
﻿using PB.BL.Domain.Account;
using PB.BL.Domain.Settings;
>>>>>>> master
using System.Collections.Generic;

namespace PB.BL.Interfaces
{
    public interface IAccountManager
    {
        IEnumerable<Profile> GetProfiles();
        Profile GetProfile(string username);
        //Profile AddProfile(string username, string email);
        void ChangeProfile(Profile profile);
        void RemoveProfile(string username);

        IEnumerable<UserSetting> GetUserSettings(string username);
        UserSetting GetUserSetting(string username, Setting.Account accountSetting);
        Profile AddUserSetting(string username, Setting.Account accountSetting, string value);
        void ChangeUserSetting(string username, UserSetting userSetting);
        //void RemoveUserSetting(string username, Setting.Account accountSetting);

        void LinkAlertsToProfile(List<Alert> alerts);

        int GetUserCount();
    }
}
