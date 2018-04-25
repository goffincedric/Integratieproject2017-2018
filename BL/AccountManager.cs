using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using PB.DAL;
using PB.DAL.EF;
using PB.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    //This class talks with the SupportCenterUserStore and tells it which
    //data to store, it also handles some logic and settings
    public class AccountManager : UserManager<Profile>, IAccountManager
    {
        private UnitOfWorkManager UowManager;
        private IntegratieUserStore store;
        private IProfileRepo ProfileRepo;
        private IAlertRepo AlertRepo;

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


        #region Init & create
        public static AccountManager Create(IdentityFactoryOptions<AccountManager> options, IOwinContext context)
        {
            //Console.WriteLine("Create accountmanager wordt gedaan");

            var manager = new AccountManager(new IntegratieUserStore(context.Get<IntegratieDbContext>()));
            manager.UserValidator = new UserValidator<Profile>(manager)
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

            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<PB.BL.Domain.Accounts.Profile>
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
                manager.UserTokenProvider = new DataProtectorTokenProvider<Profile>(dataProtectionProvider.Create("ASP.NET Identity"));
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
                var role = new IdentityRole { Name = "SuperAdmin" };
                roleManager.Create(role);

            }

            //Create Admin role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);

            }
            //Create User role   
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole { Name = "User" };
                roleManager.Create(role);

            }
        }
        #endregion

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
                    AlertRepo = new AlertRepo(UowManager.UnitOfWork);
                }
                else
                {
                    ProfileRepo = new ProfileRepo();
                    AlertRepo = new AlertRepo();
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

        #region alerts
        public List<Alert> GenerateProfileAlerts(Profile profile)
        {
            InitNonExistingRepo();
            //Alle items uit profile subscriptions halen
            if (profile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
            if (profile.Subscriptions == null || profile.Subscriptions.Count == 0) throw new Exception("U heeft nog geen subscriptions toegevoegd aan uw account, gelieve er eerst enkele te kiezen");
            List<Item> subscribedItems = profile.Subscriptions;

            //Items opdelen in Subklasses [Person, Organisation, Theme]
            List<Person> people = subscribedItems.Where(i => i is Person).ToList().Select(i => (Person)i).ToList();
            List<Organisation> organisations = new List<Organisation>(); // Alerts op organisaties;
            List<Theme> themes = new List<Theme>(); // Alerts op thema's

            //Records uit people halen
            List<Record> peopleRecords = new List<Record>();
            people.ForEach(p => p.Records.ForEach(r => peopleRecords.Add(r)));

            //Print all subscribed items
            Console.WriteLine("========= SUBSCRIBED =========");
            subscribedItems.ForEach(Console.WriteLine);

            //Check trends voor people
            List<Alert> alerts = new Trendspotter().GenerateAllAlertTypes(profile, peopleRecords);

            //Replace generated alerts with existing alerts
            AlertRepo.ReadAlerts().ToList().ForEach(a =>
            {
                Alert alert = alerts.FirstOrDefault(al => al.Equals(a));
                if (alert != null) alerts[alerts.IndexOf(alert)] = a;
            });

            //Link alerts aan profile
            List<Alert> alertsToCreate = new List<Alert>();
            List<Alert> alertsToUpdate = new List<Alert>();

            Console.WriteLine("========= NIEUWE ALERTS ========");
            alerts.ForEach(a =>
            {
                ProfileAlert profileAlert = new ProfileAlert()
                {
                    AlertId = a.AlertId,
                    Alert = a,
                    UserId = profile.Id,
                    Profile = profile,
                    IsRead = false,
                    TimeStamp = DateTime.Now
                };

                if (!a.ProfileAlerts.Contains(profileAlert) && !profile.ProfileAlerts.Contains(profileAlert))
                {
                    a.ProfileAlerts.Add(profileAlert);
                    profile.ProfileAlerts.Add(profileAlert);
                    Console.WriteLine(a);

                    if (a.AlertId == 0) alertsToCreate.Add(a);
                    else alertsToUpdate.Add(a);
                }
            });

            //Persist alerts
            AlertRepo.CreatAlerts(alertsToCreate).ToList();
            alertsToUpdate.ForEach(AlertRepo.UpdateAlert);
            UowManager.Save();

            return alerts;
        }

        //public void LinkAlertsToProfile(List<Alert> alerts)
        //{
        //    alerts.ForEach(a =>
        //    {
        //        a.Profile.Alerts.Add(a);
        //        ProfileRepo.UpdateProfile(a.Profile);
        //    });

        //    UowManager.Save();
        //}
        #endregion
    }
}