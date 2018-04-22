using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using PB.DAL;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    //This class talks with the SupportCenterUserStore and tells it which
    //data to store, it also handles some logic and settings
    public class AccountManager : UserManager<Profile>, IAccountManager
    {
        private readonly IntegratieUserStore store;
        private IProfileRepo ProfileRepo;
        private UnitOfWorkManager UowManager;



        //public AccountManager(IntegratieUserStore store, UnitOfWorkManager uowMgr) : base(store)
        //{
        //Console.WriteLine("Gebruik accountmanager constructor met store and uow");
        //  UowManager = uowMgr;
        //  this.store = store;
        //  ProfileRepo profileRepo = new ProfileRepo(UowManager.UnitOfWork);

        //  CreateRolesandUsers();


        //}
        public AccountManager(IntegratieUserStore store) : base(store)
        {
            //Console.WriteLine("Gebruik accountmanager constructor met store");

            //UowManager = uowMgr;
            this.store = store;
            InitNonExistingRepo(true);
            //ProfileRepo profileRepo = new ProfileRepo(UowManager.UnitOfWork);
            CreateRolesandUsers();
        }

        public AccountManager(IntegratieUserStore store, UnitOfWorkManager uowMgr) : base(store)
        {
            UowManager = uowMgr;
            this.store = store;
            InitNonExistingRepo(true);
            CreateRolesandUsers();
        }



        public static AccountManager Create(IdentityFactoryOptions<AccountManager> options, IOwinContext context)
        {
            //Console.WriteLine("Create accountmanager wordt gedaan");

            var manager = new AccountManager(new IntegratieUserStore(context.Get<IntegratieDbContext>()));
            manager.UserValidator = new UserValidator<BL.Domain.Account.Profile>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            manager.MaxFailedAccessAttemptsBeforeLockout = 10;

            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<PB.BL.Domain.Account.Profile>
            //{
            //  MessageFormat = "Your security code is {0}"
            //});

            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<PB.BL.Domain.Account.Profile>
            //{
            //  Subject = "Security code",
            //  BodyFormat = "Your security Code is {0}"

            //});
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<BL.Domain.Account.Profile>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;

        }

        public List<IdentityRole> GetAllRoles()
        {
            return store.ReadAllRoles();
        }

        private void CreateRolesandUsers()
        {
            IntegratieDbContext context = store.ReadContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("SuperAdmin"))
            {
                //Create SuperAdmin role
                var role = new IdentityRole {Name = "SuperAdmin"};
                roleManager.Create(role);

            }

            //Create Admin role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole {Name = "Admin"};
                roleManager.Create(role);

            }
            //Create User role   
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole {Name = "User"};
                roleManager.Create(role);

            }
        }


        public void InitNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (ProfileRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (UowManager == null)
                    {
                        UowManager = new UnitOfWorkManager();
                        //Console.WriteLine("UOW MADE IN ACCOUNT MANAGER for profile repo");
                    }
                    else
                    {
                        //Console.WriteLine("uo bestaat al");
                    }

                    ProfileRepo = new ProfileRepo(UowManager.UnitOfWork);
                }
                else
                {
                    ProfileRepo = new ProfileRepo();
                    //Console.WriteLine("OLD WAY REPO ACCOUNTMGR");
                }
            }
        }


        #region Profile
        //public Profile AddProfile(string username, string email)
        //{
        //    InitNonExistingRepo();
        //    Profile profile = new Profile()
        //    {
        //        UserName = username,
        //        Email = email,
        //        Alerts = new List<Alert>(),
        //        Dashboards = new List<Dashboard>(),
        //        Settings = new List<UserSetting>(),
        //        Subscriptions = new List<Item>()

        //    };
        //    profile.UserData = new UserData() { Profile = profile };

        //    profile = AddProfile(profile);
        //    UowManager.Save();
        //    return profile;
        //}

        //private Profile AddProfile(Profile profile)
        //{
        //    InitNonExistingRepo();
        //    Profile newProfile = ProfileRepo.CreateProfile(profile);
        //    UowManager.Save();
        //    return profile;
        //}

        public void ChangeProfile(Profile profile)
        {
            InitNonExistingRepo();
            ProfileRepo.UpdateProfile(profile);
            //Store.UpdateAsync(profile);
            UowManager.Save();
        }

        public Profile GetProfile(string userId)
        {
            InitNonExistingRepo();
            return ProfileRepo.ReadProfile(userId);
        }

        public IEnumerable<Profile> GetProfiles()
        {
            InitNonExistingRepo();
            return ProfileRepo.ReadProfiles();
        }

        public void RemoveProfile(string userId)
        {
            InitNonExistingRepo();
            Profile profile = ProfileRepo.ReadProfile(userId);

            //int id = profile.UserData.Id;

            ProfileRepo.DeleteProfile(userId);

            UowManager.Save();
        }

        public int GetUserCount()
        {
            return ProfileRepo.ReadProfiles().Count();
        }
        #endregion

        #region Subscriptions
        public Profile AddSubscription(Profile profile, Item item)
        {
            InitNonExistingRepo();

            profile.Subscriptions.Add(item);
            item.SubscribedProfiles.Add(profile);

            ChangeProfile(profile);
            UowManager.Save();

            return profile;
        }

        public Profile RemoveSubscription(Profile profile, Item item)
        {
            InitNonExistingRepo();

            profile.Subscriptions.Remove(item);
            item.SubscribedProfiles.Remove(profile);

            ChangeProfile(profile);
            UowManager.Save();

            return profile;
        }
        #endregion

        #region ProfileSettings
        public Profile AddUserSetting(string userId, Setting.Account settingName, string value)
        {
            InitNonExistingRepo();
            Profile profile = GetProfile(userId);
            profile.Settings.Add(new UserSetting()
            {
                Profile = profile,
                SettingName = settingName,
                Value = value
            });
            ChangeProfile(profile);
            UowManager.Save();
            return profile;
        }

        public void ChangeUserSetting(string userId, UserSetting userSetting)
        {
            InitNonExistingRepo();
            Profile profile = GetProfile(userId);
            profile.Settings[profile.Settings.FindIndex(s => s.SettingName.Equals(userSetting.SettingName) && s.Profile.Id.Equals(userSetting.Profile.Id))] = userSetting;

            ChangeProfile(profile);
            UowManager.Save();
        }

        public IEnumerable<UserSetting> GetUserSettings(string userId)
        {
            InitNonExistingRepo();
            return GetProfile(userId).Settings;
        }

        public UserSetting GetUserSetting(string userId, Setting.Account accountSetting)
        {
            InitNonExistingRepo();
            return GetProfile(userId).Settings.FirstOrDefault(s => s.SettingName.Equals(accountSetting));
        }
        #endregion

        public void LinkAlertsToProfile(List<Alert> alerts)
        {
            alerts.ForEach(a =>
            {
                a.Profile.Alerts.Add(a);
                ProfileRepo.UpdateProfile(a.Profile);
            });

            UowManager.Save();
        }
    }
}