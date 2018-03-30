using PB.BL.Domain.Account;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<SubPlatform> GetSubPlatforms()
        {
            initNonExistingRepo();
            return SubPlatformRepo.ReadSubPlatforms();
        }

        public SubPlatform AddSubPlatform(string name, string url, string sourceAPI = null, string siteIconUrl = null)
        {
            initNonExistingRepo();
            SubPlatform subplatform = new SubPlatform()
            {
                Name = name,
                URL = url,
                SourceAPI = sourceAPI,
                SiteIconURL = siteIconUrl,
                DateOnline = DateTime.Now,
                Style = new Style(),
                Admins = new List<Profile>(),
                Pages = new List<Page>()
            };

            subplatform = AddSubPlatform(subplatform);
            uowManager.Save();
            return subplatform;
        }

        private SubPlatform AddSubPlatform(SubPlatform subPlatform)
        {
            return SubPlatformRepo.CreateSubPlatform(subPlatform);
        }

        public SubPlatform GetSubPlatform(int subplatformId)
        {
            initNonExistingRepo();
            return SubPlatformRepo.ReadSubPlatform(subplatformId);
        }

        public void ChangeSubPlatform(SubPlatform profile)
        {
            initNonExistingRepo();
            SubPlatformRepo.UpdateSubPlatform(profile);
            uowManager.Save();
        }

        public void RemoveSubPlatform(int subplatformId)
        {
            initNonExistingRepo();
            SubPlatformRepo.DeleteSubPlatform(subplatformId);
            uowManager.Save();
        }

        public void AddAdmin(int subplatformId, Profile admin)
        {
            initNonExistingRepo();
            SubPlatform subplatform = SubPlatformRepo.ReadSubPlatform(subplatformId);
            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet

            subplatform.Admins.Add(admin);
            admin.adminPlatforms.Add(subplatform);
        }

        public void RemoveAdmin(int subplatformId, Profile admin)
        {
            initNonExistingRepo();
            SubPlatform subplatform = SubPlatformRepo.ReadSubPlatform(subplatformId);

            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet
            if (!subplatform.Admins.Remove(admin)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");
        }


        #endregion
    }
}
