using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Account;
using System.Security.Cryptography;

namespace PB.BL
{
    public class AccountManager : IAccountManager
    {
        private IProfileRepo ProfileRepo;
        private UnitOfWorkManager uowManager;

        public AccountManager()
        {

        }

        public AccountManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            ProfileRepo = new ProfileRepo(uowMgr.UnitOfWork);
        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (ProfileRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                        Console.WriteLine("UOW MADE IN ACCOUNT MANAGER for profile repo");
                    }
                    else
                    {
                        Console.WriteLine("uo bestaat al");
                    }

                    ProfileRepo = new ProfileRepo(uowManager.UnitOfWork);
                }
                else
                {
                    ProfileRepo = new ProfileRepo();
                    Console.WriteLine("OLD WAY REPO ITEMMGR");
                }
            }
        }


        #region Profile


        public Profile AddProfile(string username, string password, string hash, byte[] Salt, string email, Role role = Role.USER)
        {
            initNonExistingRepo();
            Profile profile = new Profile()
            {
                Username = username,
                Email = email,
                Role = role,
                Password = password,
                Hash = hash,
                Salt = Salt
            };
            profile.UserData = new UserData() { Profile = profile, Username = username };

            profile = AddProfile(profile);
            uowManager.Save();
            return profile;
        }

        public Profile AddProfile(Profile profile)
        {
            initNonExistingRepo();
            Profile newProfile = ProfileRepo.CreateProfile(profile);
            uowManager.Save();
            return profile;
        }

        public void ChangeProfile(Profile profile)
        {
            initNonExistingRepo();
            ProfileRepo.UpdateProfile(profile);
            uowManager.Save();
        }

        public Profile GetProfile(string username)
        {
            initNonExistingRepo();
            return ProfileRepo.ReadProfile(username);
        }

        public IEnumerable<Profile> GetProfiles()
        {
            initNonExistingRepo();
            return ProfileRepo.ReadProfiles();
        }

        public void RemoveProfile(string username)
        {
            initNonExistingRepo();
            ProfileRepo.DeleteProfile(username);
            uowManager.Save();
        }
        #endregion

        //public void Seed()
        //{
        //    initNonExistingRepo();
        //    List<Profile> profiles = new List<Profile>()
        //    {
        //        new Profile()
        //        {
        //          Email = "thomas.verhoeven@student.kdg.be",
        //          Username = "verhoeventhomas",
        //          Password = "schlack1",
        //          Role = Role.USER,
        //          Subscriptions = new List<Item>() { },
        //          Alerts = new List<Alert>(),
        //          Settings = new List<UserSetting>(),
        //          Dashboards = new Dictionary<SubPlatform, Dashboard>(),
        //          adminPlatforms = new List<SubPlatform>()
        //        },
        //        new Profile()
        //        {
        //          Email = "cedric.goffin@student.kdg.be",
        //          Username = "goffincedric",
        //          Password = "schlack2",
        //          Role = Role.USER,
        //          Subscriptions = new List<Item>(),
        //          Alerts = new List<Alert>(),
        //          Settings = new List<UserSetting>(),
        //          Dashboards = new Dictionary<SubPlatform, Dashboard>(),
        //          adminPlatforms = new List<SubPlatform>()
        //        },
        //        new Profile()
        //        {
        //          Email = "stef.havermans@student.kdg.be",
        //          Username = "haversmansstef",
        //          Password = "schlack3",
        //          Role = Role.USER,
        //          Subscriptions = new List<Item>(),
        //          Alerts = new List<Alert>(),
        //          Settings = new List<UserSetting>(),
        //          Dashboards = new Dictionary<SubPlatform, Dashboard>(),
        //          adminPlatforms = new List<SubPlatform>()
        //        },
        //        new Profile()
        //        {
        //          Email = "lotte.marien@student.kdg.be",
        //          Username = "marienlotte",
        //          Password = "schlack4",
        //          Role = Role.USER,
        //          Subscriptions = new List<Item>(),
        //          Alerts = new List<Alert>(),
        //          Settings = new List<UserSetting>(),
        //          Dashboards = new Dictionary<SubPlatform, Dashboard>(),
        //          adminPlatforms = new List<SubPlatform>()
        //        },
        //        new Profile()
        //        {
        //          Email = "lins.vannijlen@student.kdg.be",
        //          Username = "vannijlenlins",
        //          Password = "schlack5",
        //          Role = Role.USER,
        //          Subscriptions = new List<Item>(),
        //          Alerts = new List<Alert>(),
        //          Settings = new List<UserSetting>(),
        //          Dashboards = new Dictionary<SubPlatform, Dashboard>(),
        //          adminPlatforms = new List<SubPlatform>()
        //        },
        //        new Profile()
        //        {
        //          Email = "celine.verwilligen@student.kdg.be",
        //          Username = "verwilligenceline",
        //          Password = "schlack6",
        //          Role = Role.USER,
        //          Subscriptions = new List<Item>(),
        //          Alerts = new List<Alert>(),
        //          Settings = new List<UserSetting>(),
        //          Dashboards = new Dictionary<SubPlatform, Dashboard>(),
        //          adminPlatforms = new List<SubPlatform>()
        //        }
        //    };
        //    profiles.ForEach(p => p.UserData = new UserData() { Profile = p, Username = p.Username });
            
        //    profiles.ForEach(p =>  AddProfile(p));
        //    uowManager.Save();
        //}


        public void LinkAlertsToProfile(List<Alert> alerts)
        {
            alerts.ForEach(a =>
            {
                a.Profile.Alerts.Add(a);
                ProfileRepo.UpdateProfile(a.Profile);
            });

            uowManager.Save();
        }

        public Profile AddProfile(string username, string password, string email, Role role = Role.USER)
        {
            throw new NotImplementedException();
        }
    }
}
