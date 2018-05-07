using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using PB.BL.Senders;
using PB.DAL;
using PB.DAL.EF;
using PB.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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
            var userManager = new UserManager<Profile>(new UserStore<Profile>(context));

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

            // Create superadmin
            if (userManager.Users.FirstOrDefault(p => p.UserName.Equals("captain")) is null)
            {
                var user = new Profile { UserName = "captain", Email = "example@example.tld", ProfileIcon = @"~/Content/Images/Users/user.png" ,CreatedOn= DateTime.Now};
                user.UserData = new UserData() { Profile = user };
                user.Settings = new List<UserSetting>
                {
                    new UserSetting()
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName = Setting.Account.THEME,
                        Value = "light"
                    },
                    new UserSetting()
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName =Setting.Account.WANTS_ANDROID_NOTIFICATIONS,
                        Value=true, //moet nog boolean worden
                    },
                    new UserSetting()
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName =Setting.Account.WANTS_SITE_NOTIFICATIONS,
                        Value=true, //moet nog boolean worden
                    },
                    new UserSetting()
                    {
                        Profile = user,
                        IsEnabled = true,
                        SettingName =Setting.Account.WANTS_EMAIL_NOTIFICATIONS,
                        Value=true, //moet nog boolean worden
                    }
                };

                var result = userManager.CreateAsync(user, "FliesAway").Result;
                if (result.Succeeded)
                {
                    //Assign Role to user
                    userManager.AddToRoleAsync(user.Id, "SuperAdmin");
                }
            }

        }
        #endregion

        #region Profile
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

        #region Alerts
        public Dictionary<Profile, List<ProfileAlert>> SendWeeklyReviews()
        {
            // Get all profiles with at least 1 read profilealert from last week
            List<Profile> allProfiles = GetProfiles()
                .Where(p =>
                    p.Subscriptions.Count > 0 &&
                    p.Email != null &&
                    p.ProfileAlerts.FindAll(pa =>
                        pa.TimeStamp.Date >= DateTime.Today.AddDays(-7))
                    .Count > 0)
                .ToList();

            // Pick 1 random alert from each day
            Dictionary<Profile, List<ProfileAlert>> AlertsPerProfile = new Dictionary<Profile, List<ProfileAlert>>();
            Random random = new Random();
            allProfiles.ForEach(p =>
            {
                Dictionary<DateTime, List<ProfileAlert>> profileAlertsByDate = p.ProfileAlerts.GroupBy(pa => pa.TimeStamp.Date).ToDictionary(kv => kv.Key, kv => kv.ToList());
                List<ProfileAlert> profileAlerts = new List<ProfileAlert>();

                profileAlertsByDate.Values.ToList().ForEach(v =>
                {
                    if (v.Count > 1)
                    {
                        profileAlerts.Add(v[random.Next(0, v.Count - 1)]);
                    }
                });

                AlertsPerProfile.Add(p, profileAlerts);
            });

            // Send mail
            GmailSender sender = new GmailSender
            {
                IsHtml = true,
                Subject = "Weekly Review"
            };

            AlertsPerProfile.Keys.ToList().ForEach(p =>
            {
                // Get profileAlerts
                List<ProfileAlert> profileAlerts = new List<ProfileAlert>();
                AlertsPerProfile.TryGetValue(p, out profileAlerts);
                if (profileAlerts.Count == 0) return;

                // Construct body
                StringBuilder sbBody = new StringBuilder(GmailSender.WeeklyReviewBody);
                sbBody.Replace(GmailSender.DefaultUsernameSubstring, p.UserData.FirstName ?? p.UserName);
                StringBuilder sb = new StringBuilder();
                profileAlerts.ForEach(pa =>
                {
                    StringBuilder sbItem = new StringBuilder(GmailSender.WeeklyReviewListItem);
                    sbItem.Replace(GmailSender.WeeklyReviewListItemIconSubstring, "https://integratieproject.azurewebsites.net" + pa.Alert.Item.IconURL.Substring(1) ?? GmailSender.DefaultItemIcon);
                    sbItem.Replace(GmailSender.DefaultItemLinkSubstring, "#");
                    sbItem.Replace(GmailSender.WeeklyReviewListItemNameSubstring, pa.Alert.Item.Name);
                    sbItem.Replace(GmailSender.WeeklyReviewListItemDescriptionSubstring, 
                        pa.Alert.Item.Name + " " + pa.Alert.Event + " " + pa.Alert.Subject + " - " + pa.TimeStamp.Date.ToShortDateString() + "<br>" + pa.Alert.Text
                    );
                    sb.Append(sbItem.ToString());
                });
                sbBody.Replace(GmailSender.WeeklyReviewListSubstring, sb.ToString());

                // Set mail body
                sender.Body = sbBody.ToString();

                // Set mail recipient
                sender.ToEmail = p.Email;

                // Send mail
                sender.Send();
            });

            return AlertsPerProfile;
        }

        public List<Alert> GenerateAllAlerts(IEnumerable<Item> allItems, out List<Item> itemsToUpdate)
        {
            InitNonExistingRepo();

            //Create alerts list
            List<Alert> alerts = new List<Alert>();

            // Update trending items
            itemsToUpdate = Trendspotter.CheckTrendingItems(allItems.ToList(), 10, ref alerts);

            // Get all profiles with subscriptions
            List<Profile> Profiles = ProfileRepo.ReadProfiles().Where(p => p.Subscriptions.Count > 0).ToList();

            // Alle subscriptions uit profiles halen
            List<Item> Subscriptions = Profiles.SelectMany(p => p.Subscriptions).Distinct().ToList();

            // Check trends voor people
            alerts.AddRange(Trendspotter.GenerateAllAlertTypes(Subscriptions));

            //Replace generated alerts with existing alerts
            AlertRepo.ReadAlerts().ToList().ForEach(a =>
            {
                Alert alert = alerts.FirstOrDefault(al => al.Equals(a));
                if (alert != null) alerts[alerts.IndexOf(alert)] = a;
            });

            //Link alerts aan profile
            List<Alert> alertsToCreate = new List<Alert>();
            List<Alert> alertsToUpdate = new List<Alert>();

            alerts.ForEach(alert =>
            {
                bool changed = false;
                Profiles.ForEach(profile =>
                {
                    if (profile.Subscriptions.Contains(alert.Item))
                    {
                        ProfileAlert profileAlert = new ProfileAlert()
                        {
                            AlertId = alert.AlertId,
                            Alert = alert,
                            UserId = profile.Id,
                            Profile = profile,
                            IsRead = false,
                            TimeStamp = DateTime.Now
                        };

                        if (!alert.ProfileAlerts.Contains(profileAlert) && !profile.ProfileAlerts.Contains(profileAlert))
                        {
                            changed = true;
                            alert.ProfileAlerts.Add(profileAlert);
                            profile.ProfileAlerts.Add(profileAlert);
                            Console.WriteLine(alert);
                        }
                    }
                });

                if (alert.AlertId == 0) alertsToCreate.Add(alert);
                else if (changed) alertsToUpdate.Add(alert);
            });

            //Persist alerts
            AlertRepo.CreatAlerts(alertsToCreate).ToList();
            alertsToUpdate.ForEach(AlertRepo.UpdateAlert);
            UowManager.Save();

            return alerts;
        }

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

            /*
             * Person Alerts
             */
            //Print all subscribed items
            Console.WriteLine("========= SUBSCRIBED =========");
            subscribedItems.ForEach(Console.WriteLine);

            //Check trends voor people
            List<Alert> alerts = Trendspotter.GenerateAllAlertTypes(profile.Subscriptions);


            /*
             * Linking Alerts
             */
            //Replace generated alerts with existing alerts
            AlertRepo.ReadAlerts().ToList().ForEach(a =>
            {
                Alert alert = alerts.FirstOrDefault(al => al.Equals(a));
                if (alert != null) alerts.Remove(a);
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

            // Save all pending changes
            UowManager.Save();

            return alerts;
        }

        public List<ProfileAlert> GetProfileAlerts(Subplatform subplatform, Profile profile)
        {
            List<ProfileAlert> profileAlerts = new List<ProfileAlert>();

            profileAlerts.AddRange(profile.ProfileAlerts.FindAll(pa => pa.Alert.Item.SubPlatforms.Contains(subplatform)));

            profileAlerts.Sort(delegate (ProfileAlert x, ProfileAlert y)
            {
                if (x.TimeStamp == null && y.TimeStamp == null) return 0;
                else if (x.TimeStamp == null) return -1;
                else if (y.TimeStamp == null) return 1;
                else return y.TimeStamp.CompareTo(x.TimeStamp);
            });

            return profileAlerts;
        }
        #endregion
    }
}