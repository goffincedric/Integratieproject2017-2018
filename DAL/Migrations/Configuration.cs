using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using Domain.JSONConversion;
using Newtonsoft.Json;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;

namespace PB.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<IntegratieDbContext>
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

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
            Subplatform pbSubplatform = ctx.Subplatforms
                .Include(s => s.Settings)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Politieke Barometer".ToLower())).Result;
            if (pbSubplatform == null)
                pbSubplatform = new Subplatform
                {
                    Name = "Politieke Barometer",
                    URL = "politieke-barometer",
                    DateOnline = DateTime.Now,
                    Settings = new List<SubplatformSetting>
                    {
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                            Value = "31",
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SOURCE_API_URL,
                            Value = "https://kdg.textgain.com/query",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_THEME,
                            Value = "Light",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                            Value = @"~/Content/Images/Users/user.png",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                            Value = @"~/Content/Images/Users/user.png",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SOCIAL_SOURCE,
                            Value = "Twitter",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                            Value = "https://twitter.com",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SITE_NAME,
                            Value = "Barometer",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SITE_ICON_URL,
                            Value = @"~/Content/Images/Site/logo_new.png",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SEED_INTERVAL_HOURS,
                            Value = "24",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS,
                            Value = "24",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS,
                            Value = "7",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        }
                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>()
                };

            //Makes Test subplatform
            Subplatform testSubplatform =
                ctx.Subplatforms.FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Test".ToLower())).Result;
            if (testSubplatform == null)
                testSubplatform = new Subplatform
                {
                    Name = "Test",
                    URL = "testing-testing",
                    DateOnline = DateTime.Now,
                    Settings = new List<SubplatformSetting>
                    {
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                            Value = "31",
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
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

            #endregion

            #region Organisation
            //Makes all organisations
            List<Organisation> OrganisationsToAdd = new List<Organisation>
            {
                new Organisation
                {
                    Name = "PVDA",
                    FullName = "Partij van de Arbeid",
                    IconURL = @"~/Content/Images/Organisations/pvda.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform,
                        testSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                },
                new Organisation
                {
                    Name = "CD&V",
                    FullName = "Christen-Democratisch en Vlaams",
                    IconURL = @"~/Content/Images/Organisations/cdv.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform,
                        testSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                },
                new Organisation
                {
                    Name = "SP.A",
                    FullName = "Socialistische Partij Anders",
                    IconURL = @"~/Content/Images/Organisations/spa.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform,
                        testSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                },
                new Organisation
                {
                    Name = "Open Vld",
                    FullName = "Open Vlaamse Liberalen en Democraten",
                    IconURL = @"~/Content/Images/Organisations/openvld.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                },
                new Organisation
                {
                    Name = "Groen",
                    FullName = "Groen",
                    IconURL = @"~/Content/Images/Organisations/groen.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                },
                new Organisation
                {
                    Name = "N-VA",
                    FullName = "Nieuw-Vlaamse Alliantie",
                    IconURL = @"~/Content/Images/Organisations/nva.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                },
                new Organisation
                {
                    Name = "VB",
                    FullName = "Vlaams Belang",
                    IconURL = @"~/Content/Images/Organisations/vb.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Themes = new List<Theme>()
                }
            };
            List<Organisation> allOrganisations = OrganisationsToAdd.ToList();
            ctx.Organisations
                    .Include(s => s.People)
                    .Include(s => s.SubPlatforms)
                    .ToList().ForEach(o =>
            {
                Organisation organisation = OrganisationsToAdd.FirstOrDefault(org => org.Equals(o));
                if (organisation != null)
                {
                    OrganisationsToAdd.Remove(organisation);
                    allOrganisations[allOrganisations.IndexOf(organisation)] = o;
                }
                else
                {
                    allOrganisations.Add(o);
                }
            });
            //if (OrganisationsToAdd.Count != 0) OrganisationsToAdd = ctx.Organisations.AddRange(OrganisationsToAdd).ToList();
            if (allOrganisations.Count == 0) allOrganisations.AddRange(OrganisationsToAdd);
            #endregion

            #region Persons
            List<Person> personsToAdd = new List<Person>();
            List<JPerson> data = JsonConvert.DeserializeObject<List<JPerson>>(File.ReadAllText(AssemblyDirectory + @"\politiciJSON\politici.json"));

            foreach (var el in data)
            {
                Person personCheck = ctx.Persons
                    .Include(p => p.SubPlatforms)
                    .FirstOrDefault(p => p.Name.ToLower() == el.Full_name.ToLower());
                if (personCheck == null)
                {
                    personCheck = new Person
                    {
                        ItemId = el.Id,
                        Name = el.Full_name,
                        IsTrending = false,
                        IconURL = pbSubplatform.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).Value,
                        SubPlatforms = new List<Subplatform>
                        {
                            pbSubplatform
                        },
                        Keywords = new List<Keyword>(),
                        Elements = new List<Element>(),
                        SubscribedProfiles = new List<Profile>(),
                        Alerts = new List<Alert>(),
                        TrendingScore = 0,
                        FirstName = el.First_name,
                        LastName = el.Last_name,
                        Level = el.Level,
                        SocialMediaLink = el.Facebook,
                        Site = el.Site,
                        TwitterName = el.Twitter,
                        Position = el.Position,
                        District = el.District,
                        Gemeente = ToPascalCase(el.Town),
                        Postalcode = el.Postalcode,
                        Gender = el.Gender,
                        DateOfBirth = el.DateOfBirth,
                        Records = new List<Record>(),
                        Themes = new List<Theme>()
                    };
                    pbSubplatform.Items.Add(personCheck);
                }

                // Organisation
                Organisation organisationCheck = allOrganisations
                    .FirstOrDefault(o =>
                    o.Name.ToLower().Equals(el.Organistion.ToLower()) ||
                    o.FullName.ToLower().Equals(el.Organistion.ToLower()));
                if (organisationCheck == null)
                {
                    organisationCheck = new Organisation
                    {
                        Name = el.Organistion,
                        IsTrending = false,
                        IconURL = pbSubplatform.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).Value,
                        SubPlatforms = new List<Subplatform>
                        {
                            pbSubplatform
                        },
                        Keywords = new List<Keyword>(),
                        Elements = new List<Element>(),
                        SubscribedProfiles = new List<Profile>(),
                        Alerts = new List<Alert>(),
                        FullName = el.Organistion,
                        People = new List<Person>
                        {
                            personCheck
                        },
                        Themes = new List<Theme>()
                    };
                    personCheck.Organisation = organisationCheck;
                    pbSubplatform.Items.Add(organisationCheck);

                    allOrganisations.Add(organisationCheck);
                }
                else
                {
                    personCheck.Organisation = organisationCheck;
                    organisationCheck.People.Add(personCheck);

                    if (!organisationCheck.SubPlatforms.Contains(pbSubplatform))
                        organisationCheck.SubPlatforms.Add(pbSubplatform);
                    pbSubplatform.Items.Add(organisationCheck);
                }

                personsToAdd.Add(personCheck);
            }
            ctx.Persons.ForEachAsync(p =>
            {
                Person person = personsToAdd.FirstOrDefault(per => per.Equals(p));
                if (person != null)
                {
                    bool removed = personsToAdd.Remove(person);
                    ;
                }
            }).Wait();
            if (personsToAdd.Count != 0) ctx.Persons.AddRange(personsToAdd);
            #endregion

            #region Keywords
            List<Keyword> KeywordsToAdd = new List<Keyword>
            {
                new Keyword
                {
                    Name = "Auto",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Openbaar Vervoer",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "File",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Milieu",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Groene Energie",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Kernuitstap",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Vlaams",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Links",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Rechts",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Illegaal",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Moslim",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Syrië",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Oorlog",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Christenen",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Religie",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Economie",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Politiek",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Corrupt",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Amerika",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Privacy",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "GDPR",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Geld",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Hervorming",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Wet",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Jeugd",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Internet",
                    Items = new List<Item>()
                },
                new Keyword
                {
                    Name = "Technologie",
                    Items = new List<Item>()
                }
            };
            #endregion

            #region Themes

            // Makes all themes
            List<Theme> ThemesToAdd = new List<Theme>
            {
                new Theme
                {
                    Name = "Migratie",
                    IconURL = @"~/Content/Images/Themes/migratie.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[9],
                        KeywordsToAdd[10],
                        KeywordsToAdd[13],
                        KeywordsToAdd[14],
                        KeywordsToAdd[11],
                        KeywordsToAdd[12],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[17],
                        KeywordsToAdd[21],
                        KeywordsToAdd[22],
                        KeywordsToAdd[23],
                        KeywordsToAdd[24],
                        KeywordsToAdd[25]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Onderwijs",
                    IconURL = @"~/Content/Images/Themes/onderwijs.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[6],
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[9],
                        KeywordsToAdd[10],
                        KeywordsToAdd[13],
                        KeywordsToAdd[14],
                        KeywordsToAdd[11],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[18],
                        KeywordsToAdd[19],
                        KeywordsToAdd[23],
                        KeywordsToAdd[24],
                        KeywordsToAdd[25],
                        KeywordsToAdd[26]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Veiligheid",
                    IconURL = @"~/Content/Images/Themes/veiligheid.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[5],
                        KeywordsToAdd[6],
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[9],
                        KeywordsToAdd[10],
                        KeywordsToAdd[13],
                        KeywordsToAdd[14],
                        KeywordsToAdd[12],
                        KeywordsToAdd[16],
                        KeywordsToAdd[17],
                        KeywordsToAdd[18],
                        KeywordsToAdd[19],
                        KeywordsToAdd[23],
                        KeywordsToAdd[24],
                        KeywordsToAdd[25],
                        KeywordsToAdd[26]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Zorg",
                    IconURL = @"~/Content/Images/Themes/zorg.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[6],
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[14],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[18],
                        KeywordsToAdd[21],
                        KeywordsToAdd[22],
                        KeywordsToAdd[24]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Europa",
                    IconURL = @"~/Content/Images/Themes/eu.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[3],
                        KeywordsToAdd[4],
                        KeywordsToAdd[5],
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[9],
                        KeywordsToAdd[10],
                        KeywordsToAdd[13],
                        KeywordsToAdd[14],
                        KeywordsToAdd[11],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[17],
                        KeywordsToAdd[18],
                        KeywordsToAdd[19],
                        KeywordsToAdd[20],
                        KeywordsToAdd[21],
                        KeywordsToAdd[22],
                        KeywordsToAdd[23],
                        KeywordsToAdd[24],
                        KeywordsToAdd[25],
                        KeywordsToAdd[26]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Milieu",
                    IconURL = @"~/Content/Images/Themes/eu.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[0],
                        KeywordsToAdd[1],
                        KeywordsToAdd[2],
                        KeywordsToAdd[3],
                        KeywordsToAdd[4],
                        KeywordsToAdd[5],
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[18],
                        KeywordsToAdd[21],
                        KeywordsToAdd[23],
                        KeywordsToAdd[24]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Mobiliteit",
                    IconURL = @"~/Content/Images/Themes/mobiliteit.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[0],
                        KeywordsToAdd[1],
                        KeywordsToAdd[2],
                        KeywordsToAdd[3],
                        KeywordsToAdd[4],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[21],
                        KeywordsToAdd[22],
                        KeywordsToAdd[23],
                        KeywordsToAdd[24]
                    },
                    SubPlatforms = new List<Subplatform>
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme
                {
                    Name = "Energie",
                    IconURL = @"~/Content/Images/Themes/energie.png",
                    Alerts = new List<Alert>(),
                    Elements = new List<Element>(),
                    Keywords = new List<Keyword>
                    {
                        KeywordsToAdd[0],
                        KeywordsToAdd[3],
                        KeywordsToAdd[4],
                        KeywordsToAdd[5],
                        KeywordsToAdd[6],
                        KeywordsToAdd[7],
                        KeywordsToAdd[8],
                        KeywordsToAdd[15],
                        KeywordsToAdd[16],
                        KeywordsToAdd[18],
                        KeywordsToAdd[21],
                        KeywordsToAdd[22],
                        KeywordsToAdd[23],
                        KeywordsToAdd[26]
                    }
                }
            };
            ctx.Themes.ForEachAsync(t =>
            {
                Theme theme = ThemesToAdd.FirstOrDefault(them => them.Name.ToLower().Equals(t.Name.ToLower()));
                if (theme != null) ThemesToAdd.Remove(t);
                else
                    t.Keywords.ForEach(k => k.Items.Add(t));
            }).Wait();

            if (ThemesToAdd.Count != 0) ctx.Themes.AddRange(ThemesToAdd);

            #endregion

            #region Pages
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
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "Dashboard",
                    Title = "Dashboard",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "WeeklyReview",
                    Title = "Weekly Review",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "Account",
                    Title = "Account",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "FAQ",
                    Title = "FAQ",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "Contact",
                    Title = "Contact",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "ItemDetail",
                    Title = "Item Detail",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "Login",
                    Title = "Login",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "Register",
                    Title = "Register",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "Notification",
                    Title = "Notifications",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
                },
                new Page
                {
                    PageName = "UserSettings",
                    Title = "User Settings",
                    Tags = new List<Tag>(),
                    Subplatform = pbSubplatform
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
                            Name = "Legal",
                            Text = "Terms of use"
                        }
                    },
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

        public string ToPascalCase(string value)
        {
            if (value.Equals(string.Empty)) return value;
            value = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
            string[] words = null;
            if (value.Contains("-"))
            {
                words = value.Split('-');
                foreach (string word in words)
                {
                    words[Array.IndexOf(words, word)] = word.Substring(0, 1).ToUpper() + word.Substring(1);
                }
                value = string.Join("-", words);
            }

            if (value.Contains("_"))
            {
                words = value.Split('_');
                foreach (string word in words)
                {
                    words[Array.IndexOf(words, word)] = word.Substring(0, 1).ToUpper() + word.Substring(1);
                }
                value = string.Join("_", words);
            }

            if (value.Contains(" "))
            {
                words = value.Split(' ');
                foreach (string word in words)
                {
                    words[Array.IndexOf(words, word)] = word.Substring(0, 1).ToUpper() + word.Substring(1);
                }
                value = string.Join(" ", words);
            }

            return value;
        }
    }
}