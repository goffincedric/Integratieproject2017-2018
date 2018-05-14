using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PB.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<IntegratieDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IntegratieDbContext ctx)
        {
            // Seed basic data

            #region Subplatforms
            //Makes PB subplatform
            Subplatform pbSubplatform = ctx.Subplatforms.FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Politieke Barometer".ToLower())).Result;
            if (pbSubplatform == null)
            {
                pbSubplatform = new Subplatform()
                {
                    Name = "Politieke Barometer",
                    URL = "politieke-barometer",
                    DateOnline = DateTime.Now,
                    Settings = new List<SubplatformSetting>()
                    {
                        new SubplatformSetting(){
                            SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                            Value = "31",
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SOURCE_API_URL,
                            Value = "https://kdg.textgain.com/query",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                         new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DEFAULT_THEME,
                            Value = "Light",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                            Value = @"~/Content/Images/Users/user.png",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                            Value = @"~/Content/Images/Users/user.png",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SOCIAL_SOURCE,
                            Value = "Twitter",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                         new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                            Value = "https://twitter.com",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                         new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SITE_NAME,
                            Value = "Barometer",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                          new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SITE_ICON_URL,
                            Value = @"~/Content/Images/Site/logo_new.png",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        }
                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>()
                };
            }

            //Makes Test subplatform
            Subplatform testSubplatform = ctx.Subplatforms.FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Test".ToLower())).Result;
            if (testSubplatform == null)
            {
                testSubplatform = new Subplatform()
                {
                    Name = "Test",
                    URL = "testing-testing",
                    DateOnline = DateTime.Now,
                    Settings = new List<SubplatformSetting>()
                    {
                        new SubplatformSetting(){
                            SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                            Value = "31",
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.SOURCE_API_URL,
                            Value = "https://kdg.textgain.com/query",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        }
                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>()
                };
            }
            #endregion

            #region Organisation
            //Makes all organisations
            List<Organisation> OrganisationsToAdd = new List<Organisation>()
            {
                new Organisation()
                {
                    Name = "PVDA",
                    FullName = "Partij van de Arbeid",
                    IconURL=@"~/Content/Images/Organisations/pvda.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform,
                        testSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                },
                new Organisation()
                {
                    Name = "CD&V",
                    FullName = "Christen-Democratisch en Vlaams",
                    IconURL=@"~/Content/Images/Organisations/cdv.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform,
                        testSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                },
                new Organisation()
                {
                    Name =  "SP.A",
                    FullName ="Socialistische Partij Anders",
                    IconURL=@"~/Content/Images/Organisations/spa.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform,
                        testSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                },
                new Organisation()
                {
                    Name = "Open Vld",
                    FullName = "Open Vlaamse Liberalen en Democraten",
                    IconURL=@"~/Content/Images/Organisations/openvld.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                },
                new Organisation()
                {
                    Name = "Groen",
                    FullName = "Groen",
                    IconURL=@"~/Content/Images/Organisations/groen.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                },
                new Organisation()
                {
                    Name = "N-VA",
                    FullName = "Nieuw-Vlaamse Alliantie",
                    IconURL=@"~/Content/Images/Organisations/nva.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                },
                new Organisation()
                {
                    Name = "VB",
                    FullName ="Vlaams Belang" ,
                    IconURL=@"~/Content/Images/Organisations/vb.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Themes = new List<Theme>()
                }
            };
            ctx.Organisations.ForEachAsync(o =>
                    {
                        Organisation organisation = OrganisationsToAdd.FirstOrDefault(org => org.Equals(o));
                        if (organisation != null) OrganisationsToAdd.Remove(o);
                    }).Wait();
            if (OrganisationsToAdd.Count != 0) ctx.Organisations.AddRange(OrganisationsToAdd);
            #endregion

            #region Keywords
            List<Keyword> KeywordsToAdd = new List<Keyword>()
            {
                new Keyword()
                {
                    Name="Auto",
                    Items = new List<Item>(),

                },
                new Keyword()
                {
                    Name="Openbaar Vervoer",
                    Items = new List<Item>()
                },
                new Keyword()
                {
                    Name="File",
                    Items = new List<Item>()
                },
                new Keyword()
                {
                    Name="Milieu",
                    Items = new List<Item>()
                },
                new Keyword()
                {
                    Name="Groene Energie",
                    Items = new List<Item>()
                },
                new Keyword()
                {
                    Name="Kernuitstap",
                    Items = new List<Item>()
                },
                new Keyword()
                {
                    Name="Vlaams",
                    Items = new List<Item>()
                },

                new Keyword()
                {
                    Name="Links",
                    Items = new List<Item>()
                },

                new Keyword()
                {
                    Name="Rechts",
                    Items = new List<Item>()
                }
            };

            ctx.Keywords.ForEachAsync(t =>
                {
                    Keyword keyword = KeywordsToAdd.FirstOrDefault(keyw => keyw.Name.ToLower().Equals(t.Name.ToLower()));
                    if (keyword != null) KeywordsToAdd.Remove(t);
                }).Wait();
            if (KeywordsToAdd.Count != 0) ctx.Keywords.AddRange(KeywordsToAdd);
            #endregion

            #region Themes
            // Makes all themes
            List<Theme> ThemesToAdd = new List<Theme>()
            {
                new Theme()
                {
                    Name = "Migratie",
                    IconURL=@"~/Content/Images/Themes/migratie.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Onderwijs",
                    IconURL=@"~/Content/Images/Themes/onderwijs.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Veiligheid",
                    IconURL=@"~/Content/Images/Themes/veiligheid.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Zorg",
                    IconURL=@"~/Content/Images/Themes/zorg.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Europa",
                    IconURL=@"~/Content/Images/Themes/eu.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Milieu",
                    IconURL=@"~/Content/Images/Themes/eu.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name="Mobiliteit",
                    IconURL=@"~/Content/Images/Themes/mobiliteit.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                }
            };
            ctx.Themes.ForEachAsync(t =>
                {
                    Theme theme = ThemesToAdd.FirstOrDefault(them => them.Name.ToLower().Equals(t.Name.ToLower()));
                    if (theme != null) ThemesToAdd.Remove(t);
                }).Wait();
            if (ThemesToAdd.Count != 0) ctx.Themes.AddRange(ThemesToAdd);
            #endregion

            #region Pages
            List<Page> pagesToAdd = new List<Page>()
            {
                new Page()
                {
                    PageName = "Home",
                    Title = "Home",
                    Tags = new List<Tag>()
                    {
                        new Tag()
                        {
                           Name="BannerTitle",
                           Text="Politieke Barometer"
                        },
                        new Tag()
                        {
                            Name="BannerTextSub1",
                            Text = "Volg uw favoriete politiekers, partijen en thema's en bekijk hoe deze door anderen besproken worden op sociale media."
                        },
                        new Tag()
                        {
                            Name="BannerTextSub2",
                            Text = "Cree�r uw eigen dashboard en bekijk en analyseer live grafieken!"
                        },
                        new Tag()
                        {
                            Name="call-to-action-text",
                            Text="Krijg toegang tot ons duizelingwekkend aanbod aan geanalyseerde en gevisualiseerde data."
                        }

                    },
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Dashboard",
                    Title = "Dashboard",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "WeeklyReview",
                    Title = "Weekly Review",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Account",
                    Title = "Account",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "FAQ",
                    Title = "FAQ",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Contact",
                    Title = "Contact",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "ItemDetail",
                    Title = "Item Detail",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Login",
                    Title = "Login",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Register",
                    Title = "Register",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Notification",
                    Title = "Notifications",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "UserSettings",
                    Title = "User Settings",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page()
                {
                    PageName = "Menu",
                    Title = "Menu",
                    Tags = new List<Tag>(){
                          new Tag()
                        {
                            Name="Home",
                            Text="Home"
                        },
                          new Tag()
                        {
                            Name="Dashboard",
                            Text="Dashboard"
                        },
                          new Tag()
                        {
                            Name="Weekly_Review",
                            Text="Weekly Review"
                        },
                          new Tag()
                        {
                            Name="Account",
                            Text="My Account"
                        },
                           new Tag()
                        {
                            Name="More",
                            Text="More"
                        },
                            new Tag()
                        {
                            Name="FAQ",
                            Text="FAQ"
                        },
                             new Tag()
                        {
                            Name="Contact",
                            Text="Contact"
                        },
                            new Tag()
                            {
                                Name="Legal",
                                Text="Terms of use"
                              }

                    }
                    ,
                    Subplatform = pbSubplatform
                }
            };
            ctx.Pages.ForEachAsync(p =>
                {
                    Page page = pagesToAdd.FirstOrDefault(pta => pta.Equals(p));
                    if (page != null) pagesToAdd.Remove(p);
                }).Wait();
            if (pagesToAdd.Count != 0) ctx.Pages.AddRange(pagesToAdd);
            #endregion

            // Save all pending changes
            ctx.SaveChanges();
        }
    }
}
