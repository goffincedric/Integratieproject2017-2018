﻿using PB.BL.Domain.Account;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
    public class SubplatformManager : ISubplatformManager
    {
        private ISubplatformRepo SubplatformRepo;
        private UnitOfWorkManager uowManager;

        public SubplatformManager()
        {

        }

        public SubplatformManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            SubplatformRepo = new SubplatformRepo(uowMgr.UnitOfWork);
        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (SubplatformRepo == null)
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

                    SubplatformRepo = new SubplatformRepo(uowManager.UnitOfWork);
                }
                else
                {
                    SubplatformRepo = new SubplatformRepo();
                    Console.WriteLine("OLD WAY REPO SUBPLATFORMMGR");
                }
            }
        }

        #region Subplatform

        public IEnumerable<Subplatform> GetSubPlatforms()
        {
            initNonExistingRepo();
            return SubplatformRepo.ReadSubPlatforms();
        }

        public Subplatform AddSubplatform(string name, string url, string sourceAPI = null, string siteIconUrl = null)
        {
            initNonExistingRepo();
            Subplatform subplatform = new Subplatform()
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

            subplatform = AddSubplatform(subplatform);
            uowManager.Save();
            return subplatform;
        }

        private Subplatform AddSubplatform(Subplatform subPlatform)
        {
            return SubplatformRepo.CreateSubPlatform(subPlatform);
        }

        public Subplatform GetSubplatform(int subplatformId)
        {
            initNonExistingRepo();
            return SubplatformRepo.ReadSubPlatform(subplatformId);
        }

        public void ChangeSubplatform(Subplatform profile)
        {
            initNonExistingRepo();
            SubplatformRepo.UpdateSubPlatform(profile);
            uowManager.Save();
        }

        public void RemoveSubplatform(int subplatformId)
        {
            initNonExistingRepo();
            SubplatformRepo.DeleteSubPlatform(subplatformId);
            uowManager.Save();
        }

        public void AddAdmin(int subplatformId, Profile admin)
        {
            initNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubPlatform(subplatformId);
            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet

            subplatform.Admins.Add(admin);
            admin.adminPlatforms.Add(subplatform);

            SubplatformRepo.UpdateSubPlatform(subplatform);
            uowManager.Save();
        }

        public void RemoveAdmin(int subplatformId, Profile admin)
        {
            initNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubPlatform(subplatformId);

            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet
            if (!subplatform.Admins.Remove(admin)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");
            if (!admin.adminPlatforms.Remove(subplatform)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");

            uowManager.Save();
        }


        #endregion

        #region Pages
        public Page AddPage(int subplatformId, string title, string faviconURL)
        {
            Page page = new Page()
            {
                Title = title,
                FaviconURL = faviconURL,
                Tags = new List<Tag>()
            };
            return AddPage(subplatformId, page);
        }

        private Page AddPage(int subplatformId, Page page)
        {
            Subplatform subplatform = SubplatformRepo.ReadSubPlatform(subplatformId);
            subplatform.Pages.Add(page);

            SubplatformRepo.UpdateSubPlatform(subplatform);
            uowManager.Save();
            return page;
        }
        #endregion
    }
}
