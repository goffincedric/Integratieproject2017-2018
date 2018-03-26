using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Account;
using PB.BL.Domain.Platform;
using PB.DAL;

namespace PB.BL
{
    public class SubPlatformManager : ISubPlatformManager
    {
        private ISubPlatformRepo SubPlatformRepo;
        private UnitOfWorkManager uowManager;

        public SubPlatformManager()
        {

        }

        public SubPlatformManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            SubPlatformRepo = new SubPlatformRepo(uowMgr.UnitOfWork);
        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (SubPlatformRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                        Console.WriteLine("UOW MADE IN SUBPLATFORM MANAGER");
                    }
                    else
                    {
                        Console.WriteLine("uo bestaat al");
                    }

                    SubPlatformRepo = new SubPlatformRepo(uowManager.UnitOfWork);
                }
                else
                {
                    SubPlatformRepo = new SubPlatformRepo();
                    Console.WriteLine("OLD WAY REPO SUBPLATFORMMGR");
                }
            }
        }

        #region Subplatform
        public Profile AddProfile(string username, string password, string email, Role role = Role.USER)
        {
            //initNonExistingRepo();
            //SubPlatform subPlatform = new SubPlatform()
            //{
            //    Username = username,
            //    Password = password,
            //    Email = email,
            //    Role = role
            //};
            //profile = AddProfile(profile);
            //uowManager.Save();
            //return profile;
            return null;
        }

        public void AddAdmin(Profile admin)
        {
            throw new NotImplementedException();
        }

        public SubPlatform AddProfile(SubPlatform profile)
        {
            throw new NotImplementedException();
        }

        public void ChangeProfile(SubPlatform profile)
        {
            throw new NotImplementedException();
        }

        public SubPlatform GetProfile(int subplatformId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubPlatform> GetSubPlatforms()
        {
            throw new NotImplementedException();
        }

        public void RemoveAdmin(Profile admin)
        {
            throw new NotImplementedException();
        }

        public void RemoveProfile(int subplatformId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
