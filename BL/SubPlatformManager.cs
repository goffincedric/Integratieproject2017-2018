using Domain.Settings;
using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using PB.DAL;
using System;
using System.Collections.Generic;

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
                        //Console.WriteLine("UOW MADE IN SUBPLATFORM MANAGER");
                    }
                    else
                    {
                        //Console.WriteLine("uo bestaat al");
                    }

                    SubplatformRepo = new SubplatformRepo(uowManager.UnitOfWork);
                }
                else
                {
                    SubplatformRepo = new SubplatformRepo();
                    //Console.WriteLine("OLD WAY REPO SUBPLATFORMMGR");
                }
            }
        }

        #region Subplatform

        public IEnumerable<Subplatform> GetSubplatforms()
        {
            initNonExistingRepo();
            return SubplatformRepo.ReadSubplatforms();
        }

        public Subplatform AddSubplatform(string name, string url, string sourceApi = null, string siteIconUrl = null)
        {
            initNonExistingRepo();
            Subplatform subplatform = new Subplatform()
            {
                Name = name,
                URL = url,
                DateOnline = DateTime.Now,
                Style = new Style(),
                Admins = new List<Profile>(),
                Items = new List<Item>(),
                Pages = new List<Page>(),
                Settings = new List<SubplatformSetting>(),
                Dashboards = new List<Dashboard>()
            };

            if (sourceApi != null) subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.SOURCE_API_URL,
                IsEnabled = true,
                Value = sourceApi
            });

            if (siteIconUrl != null) subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.SITE_ICON_URL,
                IsEnabled = true,
                Value = siteIconUrl
            });

            subplatform = AddSubplatform(subplatform);
            uowManager.Save();
            return subplatform;
        }

        private Subplatform AddSubplatform(Subplatform subplatform)
        {
            return SubplatformRepo.CreateSubplatform(subplatform);
        }

        public Subplatform GetSubplatform(int subplatformId)
        {
            initNonExistingRepo();
            return SubplatformRepo.ReadSubplatform(subplatformId);
        }

        public void ChangeSubplatform(Subplatform profile)
        {
            initNonExistingRepo();
            SubplatformRepo.UpdateSubplatform(profile);
            uowManager.Save();
        }

        public void RemoveSubplatform(int subplatformId)
        {
            initNonExistingRepo();
            SubplatformRepo.DeleteSubplatform(subplatformId);
            uowManager.Save();
        }

        public void AddAdmin(int subplatformId, Profile admin)
        {
            initNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);
            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet

            subplatform.Admins.Add(admin);
            admin.AdminPlatforms.Add(subplatform);

            SubplatformRepo.UpdateSubplatform(subplatform);
            uowManager.Save();
        }

        public void RemoveAdmin(int subplatformId, Profile admin)
        {
            initNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);

            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet
            if (!subplatform.Admins.Remove(admin)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");
            if (!admin.AdminPlatforms.Remove(subplatform)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");

            uowManager.Save();
        }
        #endregion

        #region Pages
        public Page AddPage(int subplatformId, string title, string faviconUrl)
        {
            Page page = new Page()
            {
                Title = title,
                FaviconURL = faviconUrl,
                Tags = new List<Tag>()
            };
            return AddPage(subplatformId, page);
        }

        private Page AddPage(int subplatformId, Page page)
        {
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);
            subplatform.Pages.Add(page);

            SubplatformRepo.UpdateSubplatform(subplatform);
            uowManager.Save();
            return page;
        }
        #endregion
    }
}
