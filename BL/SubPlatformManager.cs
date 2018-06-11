using System;
using System.Collections.Generic;
using System.Linq;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
using PB.DAL;
using PB.DAL.EF;

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
                if (createWithUnitOfWork)
                {
                    if (uowManager == null) uowManager = new UnitOfWorkManager();

                    SubplatformRepo = new SubplatformRepo(uowManager.UnitOfWork);
                }
                else
                {
                    SubplatformRepo = new SubplatformRepo();
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
            Subplatform subplatform = new Subplatform
            {
                Name = name,
                URL = url ?? name.Trim().ToLower().Replace(" ", "-"),
                DateOnline = DateTime.Now,
                Style = new Style(),
                Admins = new List<Profile>(),
                Items = new List<Item>(),
                Settings = new List<SubplatformSetting>(),
                Pages = new List<Page>(),
                Dashboards = new List<Dashboard>()
            };
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SOURCE_API_URL,
                IsEnabled = true,
                Value = sourceApi ?? "https://kdg.textgain.com/query",
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SITE_ICON_URL,
                IsEnabled = true,
                Value = siteIconUrl ?? @"~/Content/Images/default-subplatform.png",
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                Value = "31",
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.DEFAULT_THEME,
                Value = "Light",
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                Value = @"~/Content/Images/Users/user.png",
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                Value = @"~/Content/Images/Users/user.png",
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SOCIAL_SOURCE,
                Value = null,
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SITE_NAME,
                Value = name,
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                Value = null,
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SEED_INTERVAL_HOURS,
                Value = "24",
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS,
                Value = "24",
                IsEnabled = true,
                Subplatform = subplatform
            });
            subplatform.Settings.Add(new SubplatformSetting
            {
                SettingName = Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS,
                Value = "24",
                IsEnabled = true,
                Subplatform = subplatform
            });

            List<Page> pagesToAdd = new List<Page>
            {
                new Page
                {
                    PageName = "Home",
                    Title = "Home",
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "BannerTitle",
                            Text = "Politieke Barometer"
                        },
                        new Tag
                        {
                            Name = "BannerTextSub1",
                            Text = "Volg uw favoriete politiekers, partijen en thema's en bekijk hoe deze door anderen besproken worden op sociale media."
                        },
                        new Tag
                        {
                            Name = "BannerTextSub2",
                            Text = "Creeër uw eigen dashboard en bekijk en analyseer live grafieken!"
                        },
                        new Tag
                        {
                            Name = "call-to-action-text",
                            Text = "Krijg toegang tot ons duizelingwekkend aanbod aan geanalyseerde en gevisualiseerde data."
                        }
                    },
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Dashboard",
                    Title = "Dashboard",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "WeeklyReview",
                    Title = "Weekly Review",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                 new Page
                {
                    PageName = "Items",
                    Title = "Items",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Persons",
                    Title = "Politici",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Organisations",
                    Title = "Partijen",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Themes",
                    Title = "Thema's",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Account",
                    Title = "Account",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "FAQ",
                    Title = "FAQ",
                    Subplatform = subplatform,
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "Question1",
                            Text = "Waarom zou ik me registreren op de Politieke Barometer?",
                            Text2 = "De politieke barometer kan u bijstaan om politieke trends te monitoren en te analyseren. Hiermee kan u uw visie op politieke events en trends aanscherpen, waardoor u ook bewuster wordt van wat er rondom je gebeurt. Dit zal er voor zorgen dat u met zekerheid naar de stemhokjes kan."
                        },
                        new Tag
                        {
                            Name="Question2",
                            Text = "Hoe maak ik een account aan?",
                            Text2 = "Klik Rechtsboven op het login / register. Wij verwerken uw gegevens en u kan direct aan de slag!"
                        },
                        new Tag
                        {   Name ="Question3",
                            Text= "Hoe subscribe ik op een item?",
                            Text2 = "Rechts boven vindt u een zoekbalk waarin in de naam van het gewenste thema, persoon of organisatie kan ingevult worden. Dit zal u leiden tot aan de desbetreffende detailpagina waar een subscribe knop in het menu staat."
                        },
                        new Tag
                        {
                            Name = "Question4",
                            Text = "Kan ik mijn wachtwoord of gebruikersnaam nog aanpassen?",
                            Text2=  "Ja, dit gaat! Als u naar je persoonlijke instellingen gaat dan kan je al deze gegevens up-to-date houden."
                        },
                        new Tag
                        {
                            Name = "Question5",
                            Text = "Kan ik mijn account ook verwijderen?",
                            Text2 = "Ja, dit gaat ook in je persoonlijke instellingen. We vinden het echter wel spijtig om je te zien vertrekken. Het zou ons veel plezier doen moest je het contact formulier invullen en je ervaringen met het platform delen."
                        },
                        new Tag
                        {
                            Name = "Question6",
                            Text = "Hoe werkt politieke barometer?",
                            Text2 =  "Wij werken nauw samen met TextGain, een bedrijf dat Web services aanbied voor voorspellende text analyses. Zij analyseren Twitter-berichten voor ons en wij bieden u met veel plezier deze data aan in een overzichtelijk formaat."
                        },
                        new Tag
                        {
                            Name = "Question7",
                            Text = "Ik vind jullie kleurenschema maar niets. Kan ik dit aanpassen?",
                            Text2 ="Ja, dit gaat. U kan rechtsboven uw thema aanpassen naar een van onze 3 thema's. Indien u geen enkel van onze thema's leuk vindt, kan u altijd een verzoek sturen via onze contact pagina."
                        }
                    }
                },
                new Page
                {
                    PageName = "Contact",
                    Title = "Contact",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "ItemDetail",
                    Title = "Item Detail",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Login",
                    Title = "Login",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Register",
                    Title = "Register",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Notification",
                    Title = "Notifications",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "UserSettings",
                    Title = "User Settings",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Menu",
                    Title = "Menu",
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "Home",
                            Text = "Home"
                        },
                        new Tag
                        {
                            Name = "Dashboard",
                            Text = "Dashboard"
                        },
                        new Tag
                        {
                            Name = "Weekly_Review",
                            Text = "Weekly Review"
                        },
                        new Tag
                        {
                            Name = "Account",
                            Text = "My Account"
                        },
                        new Tag
                        {
                            Name = "More",
                            Text = "More"
                        },
                        new Tag
                        {
                            Name = "FAQ",
                            Text = "FAQ"
                        },
                        new Tag
                        {
                            Name = "Contact",
                            Text = "Contact"
                        },
                        new Tag
                        {
                            Name = "Items",
                            Text = "Items"
                        },
                        new Tag
                        {
                            Name = "Persons",
                            Text = "Politici"
                        },
                        new Tag
                        {
                            Name = "Organisations",
                            Text = "Partijen"
                        },
                        new Tag
                        {
                            Name = "Themes",
                            Text = "Thema's"
                        },
                        new Tag
                        {
                            Name = "Legal",
                            Text = "Terms of use"
                        }
                    },
                    Subplatform = subplatform
                }
            };
            subplatform.Pages.AddRange(pagesToAdd);

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

        public void ChangeSubplatforms(List<Subplatform> subplatforms)
        {
            InitNonExistingRepo();
            SubplatformRepo.UpdateSubplatforms(subplatforms);
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
            if (subplatform == null)
                throw new Exception("Subplatform with id (" + subplatformId +
                                    ") doesnt exist"); //Subplatform bestaat niet

            subplatform.Admins.Add(admin);
            admin.AdminPlatforms.Add(subplatform);

            SubplatformRepo.UpdateSubplatform(subplatform);
            uowManager.Save();
        }

        public void RemoveAdmin(int subplatformId, Profile admin)
        {
            InitNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);

            if (subplatform == null)
                throw new Exception("Subplatform with id (" + subplatformId +
                                    ") doesn't exist"); //Subplatform bestaat niet
            if (!subplatform.Admins.Remove(admin))
                throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");
            if (!admin.AdminPlatforms.Remove(subplatform))
                throw new Exception("Couldn't remove admin, maybe the admin doesn't exist?");

            uowManager.Save();
        }

        #endregion

        #region Subplatformsettings

        public SubplatformSetting AddSubplatformSetting(Setting.Platform settingName, int subplatformId, string value,
            bool isEnabled = false)
        {
            InitNonExistingRepo();
            Subplatform subplatform = SubplatformRepo.ReadSubplatform(subplatformId);
            if (subplatform == null)
                throw new Exception("Subplatform with id (" + subplatformId +
                                    ") doesn't exist"); //Subplatform bestaat niet
            SubplatformSetting subplatformSetting = new SubplatformSetting
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

        public void ChangeSubplatformSetting(Subplatform subplatform, SubplatformSetting setting)
        {
            InitNonExistingRepo();
            if (subplatform == null) throw new Exception("No subplatform has been provided");
            if (setting.Subplatform.SubplatformId != subplatform.SubplatformId)
                throw new Exception(
                    "Setting doesn't have same subplatform anymore. Settings cannot be removed from subplatforms, only disabled.");
            if (!subplatform.Settings.Contains(setting))
                if (subplatform.Settings.FirstOrDefault(sstc => sstc.SettingName.Equals(setting.SettingName)) is null)
                    subplatform.Settings.Add(setting);
                else
                    subplatform.Settings[
                        subplatform.Settings.FindIndex(sstc => sstc.SettingName.Equals(setting.SettingName))] = setting;
            SubplatformRepo.UpdateSubplatform(subplatform);
            uowManager.Save();
        }

        public void ChangeSubplatformSettings(Subplatform subplatform, List<SubplatformSetting> settings)
        {
            InitNonExistingRepo();
            if (subplatform == null) throw new Exception("No subplatform has been provided");

            settings.ForEach(ss =>
            {
                if (ss.Subplatform.SubplatformId != subplatform.SubplatformId)
                    throw new Exception(
                        "Setting doesn't have same subplatform anymore. Settings cannot be removed from subplatforms, only disabled.");
                if (!subplatform.Settings.Contains(ss))
                    if (subplatform.Settings.FirstOrDefault(sstc => sstc.SettingName.Equals(ss.SettingName)) is null)
                        subplatform.Settings.Add(ss);
                    else
                        subplatform.Settings[
                            subplatform.Settings.FindIndex(sstc => sstc.SettingName.Equals(ss.SettingName))] = ss;
            });

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
            Page page = new Page
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

        public Tag AddTag(int pageId, string name, string text, string text2 = null)
        {
            InitNonExistingRepo();
            Page page = SubplatformRepo.ReadPage(pageId);
            if (page == null) throw new Exception("Page with pageId (" + pageId + ") doesn't exist");
            Tag tag = new Tag
            {
                Page = page,
                Name = name,
                Text = text,
                Text2 = text2
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

        public Tag GetTag(string name, string subplatformUrl)
        {
            InitNonExistingRepo();
            return SubplatformRepo
                .ReadTags()
                .SingleOrDefault(t => t.Name.Equals(name) && t.Page.Subplatform.URL.Equals(subplatformUrl));
        }

        public void ChangeTag(Tag tag)
        {
            InitNonExistingRepo();
            SubplatformRepo.UpdateTag(tag);
            uowManager.Save();
        }

        public void ChangeTags(List<Tag> tags)
        {
            InitNonExistingRepo();
            SubplatformRepo.UpdateTags(tags);
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