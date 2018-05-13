using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using PB.DAL;
using PB.DAL.EF;
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

        public SubplatformManager(IntegratieDbContext context)
        {
            SubplatformRepo = new SubplatformRepo(context);
        }

        public SubplatformManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            SubplatformRepo = new SubplatformRepo(uowMgr.UnitOfWork);

        }

        public void InitNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (SubplatformRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                    }

                    SubplatformRepo = new SubplatformRepo(uowManager.UnitOfWork);
                }
                else
                {
                    SubplatformRepo = new SubplatformRepo();
                }
            }
        }

        #region Subplatform
        public IEnumerable<Subplatform> GetSubplatforms()
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadSubplatforms();
        }

        public Subplatform AddSubplatform(string name, string url, string sourceApi = null, string siteIconUrl = null)
        {
            InitNonExistingRepo();
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

            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                Value = "31",
                IsEnabled = true
            });
            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.DEFAULT_THEME,
                Value = "Light",
                IsEnabled = true
            });
            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                Value = @"~/Content/Images/Users/user.png",
                IsEnabled = true
            });
            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                Value = @"~/Content/Images/Users/user.png",
                IsEnabled = true
            });
            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.SOCIAL_SOURCE,
                Value = null,
                IsEnabled = true
            });
            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.SITE_NAME,
                Value = name,
                IsEnabled = true
            });
            subplatform.Settings.Add(new SubplatformSetting()
            {
                SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                Value = null,
                IsEnabled = true
            });


            subplatform = AddSubplatform(subplatform);
            uowManager.Save();
            return subplatform;
        }

        private Subplatform AddSubplatform(Subplatform subplatform)
        {
            InitNonExistingRepo();
            return SubplatformRepo.CreateSubplatform(subplatform);
        }

        public Subplatform GetSubplatform(int subplatformId)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadSubplatform(subplatformId);
        }

        public Subplatform GetSubplatform(string subplatformURL)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadSubplatform(subplatformURL);
        }

        public void ChangeSubplatform(Subplatform profile)
        {
            InitNonExistingRepo();
            SubplatformRepo.UpdateSubplatform(profile);
            uowManager.Save();
        }

        public void RemoveSubplatform(int subplatformId)
        {
            InitNonExistingRepo();
            SubplatformRepo.DeleteSubplatform(subplatformId);
            uowManager.Save();
        }

        public void AddAdmin(int subplatformId, Profile admin)
        {
            InitNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);
            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesnt exist"); //Subplatform bestaat niet

            subplatform.Admins.Add(admin);
            admin.AdminPlatforms.Add(subplatform);

            SubplatformRepo.UpdateSubplatform(subplatform);
            uowManager.Save();
        }

        public void RemoveAdmin(int subplatformId, Profile admin)
        {
            InitNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);

            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesn't exist"); //Subplatform bestaat niet
            if (!subplatform.Admins.Remove(admin)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");
            if (!admin.AdminPlatforms.Remove(subplatform)) throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");

            uowManager.Save();
        }
        #endregion

        #region Subplatformsettings
        public SubplatformSetting AddSubplatformSetting(Setting.Platform settingName, int subplatformId, string value, bool isEnabled = false)
        {
            InitNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);
            if (subplatform == null) throw new Exception("Subplatform with id (" + subplatformId + ") doesn't exist"); //Subplatform bestaat niet
            SubplatformSetting subplatformSetting = new SubplatformSetting()
            {
                SettingName = settingName,
                Subplatform = subplatform,
                Value = value,
                IsEnabled = isEnabled
            };

            subplatformSetting = SubplatformRepo.CreateSubplatformSetting(subplatformSetting);
            uowManager.Save();
            return subplatformSetting;
        }

        public void ChangeSubplatformSetting(string subplatformURL, SubplatformSetting setting)
        {
            InitNonExistingRepo();
            Subplatform subplatform = GetSubplatform(subplatformURL);
            if (subplatform == null) throw new Exception("Subplatform with subplatformurl (" + subplatformURL + ") doesn't exist");
            if (setting.SubplatformId != subplatform.SubplatformId) throw new Exception("Setting doesn't have same subplatform anymore. Settings cannot be removed from subplatforms, only disabled.");
            if (!subplatform.Settings.Contains(setting))
            {
                subplatform.Settings.Add(setting);
            }
            else
            {
                subplatform.Settings[subplatform.Settings.IndexOf(setting)] = setting;
            }
            SubplatformRepo.UpdateSubplatform(subplatform);
            uowManager.Save();
        }

        public SubplatformSetting GetSubplatformSetting(int subplatformId, Setting.Platform settingname)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadSubplatformSetting(settingname, subplatformId);
        }
        #endregion

        #region Pages
        public Page AddPage(int subplatformId, string pageName, string title)
        {
            InitNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);
            if (subplatform == null) throw new Exception("Subplatform with subplatformId (" + subplatformId + ") doesn't exist");
            Page page = new Page()
            {
                Title = title,
                PageName = pageName,
                Tags = new List<Tag>(),
                Subplatform = subplatform
            };
            subplatform.Pages.Add(page);
            page = SubplatformRepo.CreatePage(page);
            uowManager.Save();
            return page;
        }

        public IEnumerable<Page> GetPages()
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadPages();
        }

        public IEnumerable<Page> GetPages(string subplatformUrl)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadPages(subplatformUrl);
        }

        public Page GetPage(int pageId)
        {
            return SubplatformRepo.ReadPage(pageId);
        }

        public void ChangePage(Page page)
        {
            InitNonExistingRepo();
            SubplatformRepo.UpdatePage(page);
            uowManager.Save();
        }

        public void RemovePage(int pageId)
        {
            InitNonExistingRepo();
            SubplatformRepo.DeletePage(pageId);
            uowManager.Save();
        }
        #endregion

        #region Tags
        public IEnumerable<Tag> GetTags()
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadTags();
        }

        public IEnumerable<Tag> GetTags(int pageId)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadTags(pageId);
        }

        public Tag AddTag(int pageId, string cssName, string nameObject, string text)
        {
            InitNonExistingRepo();
            Page page = SubplatformRepo.ReadPage(pageId);
            if (page == null) throw new Exception("Page with pageId (" + pageId + ") doesn't exist");
            Tag tag = new Tag()
            {
                Page = page,
                CssName = cssName,
                NameObject = nameObject,
                Text = text
            };
            page.Tags.Add(tag);

            tag = SubplatformRepo.CreateTag(tag);
            uowManager.Save();
            return tag;
        }

        public Tag GetTag(int tagId)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadTag(tagId);
        }

        public Tag GetTag(string name)
        {
            InitNonExistingRepo();
            return SubplatformRepo.ReadTag(name);
        }
        public void ChangeTag(Tag tag)
        {
            InitNonExistingRepo();
            SubplatformRepo.UpdateTag(tag);
            uowManager.Save();
        }

        public void RemoveTag(int tagId)
        {
            InitNonExistingRepo();
            SubplatformRepo.DeleteTag(tagId);
            uowManager.Save();
        }
        #endregion
    }
}
