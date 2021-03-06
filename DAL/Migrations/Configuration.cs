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

            // Makes default pages
            #region Pages
            List<Page> defaultPages = new List<Page>
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
                            Text = "Cree�r uw eigen dashboard en bekijk en analyseer live grafieken!"
                        },
                        new Tag
                        {
                            Name = "call-to-action-text",
                            Text = "Krijg toegang tot ons duizelingwekkend aanbod aan geanalyseerde en gevisualiseerde data."
                        }
                    }
                },
                new Page
                {
                    PageName = "Dashboard",
                    Title = "Dashboard",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "WeeklyReview",
                    Title = "Weekly Review",
                    Tags = new List<Tag>()
                },
                 new Page
                {
                    PageName = "Items",
                    Title = "Items",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Persons",
                    Title = "Politici",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Organisations",
                    Title = "Partijen",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Themes",
                    Title = "Thema's",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Account",
                    Title = "Account",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "FAQ",
                    Title = "FAQ",
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
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "ItemDetail",
                    Title = "Item Detail",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Login",
                    Title = "Login",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Register",
                    Title = "Register",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "Notification",
                    Title = "Notifications",
                    Tags = new List<Tag>()
                },
                new Page
                {
                    PageName = "UserSettings",
                    Title = "User Settings",
                    Tags = new List<Tag>()
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
                    }
                }
            };

            #endregion

            //Makes PB subplatform
            Subplatform pbSubplatform = ctx.Subplatforms
                .Include(s => s.Settings)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Politieke Barometer".ToLower())).Result;
            if (pbSubplatform == null)
            {
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
                            Value = "21",
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
                            Value = @"~/Content/Images/Site/Politieke-barometer-logo.png",
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
                        },
                         new SubplatformSetting
                        {
                            SettingName = Setting.Platform.BANNER,
                            Value = @"~/Content/Images/Index/banner.jpg",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                         new SubplatformSetting
                        {
                            SettingName = Setting.Platform.PRIMARY_COLOR,
                            Value = "#333333",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        },
                         new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SECONDARY_COLOR,
                            Value = "#888888",
                            IsEnabled = true,
                            Subplatform = pbSubplatform
                        }

                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>(defaultPages.Select(p => (Page)p.Clone()))
                };
                pbSubplatform.Pages.ForEach(p =>
                {
                    p.Tags.ForEach(t =>
                    {
                        t.Page = p;
                    });
                    p.Subplatform = pbSubplatform;
                });
            }
            //Makes Test subplatform
            Subplatform testSubplatform = ctx.Subplatforms.FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Test".ToLower())).Result;
            if (testSubplatform == null)
            {
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
                            Value = "21",
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SOURCE_API_URL,
                            Value = "",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_THEME,
                            Value = "Light",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_USER_ICON,
                            Value = @"~/Content/Images/Users/user.png",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.DEFAULT_NEW_ITEM_ICON,
                            Value = @"~/Content/Images/Users/user.png",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SOCIAL_SOURCE,
                            Value = "",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SOCIAL_SOURCE_URL,
                            Value = "",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SITE_NAME,
                            Value = "Testing",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SITE_ICON_URL,
                            Value = @"~/Content/Images/Site/testing.png",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SEED_INTERVAL_HOURS,
                            Value = "24",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.ALERT_GENERATION_INTERVAL_HOURS,
                            Value = "24",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                        new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SEND_WEEKLY_REVIEWS_INTERVAL_DAYS,
                            Value = "7",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                            new SubplatformSetting
                        {
                            SettingName = Setting.Platform.BANNER,
                            Value = @"~/Content/Images/Index/testing-banner.PNG",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                         new SubplatformSetting
                        {
                            SettingName = Setting.Platform.PRIMARY_COLOR,
                            Value = "#333333",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        },
                         new SubplatformSetting
                        {
                            SettingName = Setting.Platform.SECONDARY_COLOR,
                            Value = "#888888",
                            IsEnabled = true,
                            Subplatform = testSubplatform
                        }
                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>(defaultPages.Select(p => (Page)p.Clone()))
                };
                testSubplatform.Pages.ForEach(p =>
                {
                    p.Tags.ForEach(t =>
                    {
                        t.Page = p;
                    });
                    p.Subplatform = testSubplatform;
                });
            }
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
            ctx.Organisations
                    .Include(s => s.People)
                    .Include(s => s.SubPlatforms)
                    .ToList().ForEach(o =>
            {
                Organisation organisation = OrganisationsToAdd.FirstOrDefault(org => org.Equals(o));
                if (organisation != null)
                {
                    OrganisationsToAdd[OrganisationsToAdd.IndexOf(organisation)] = o;
                }
                else
                {
                    OrganisationsToAdd.Add(o);
                }
            });
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
                    if (string.IsNullOrWhiteSpace(el.Twitter))
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
                            SocialMediaLink = (el.Facebook.Contains(" ")) ? "" : el.Facebook,
                            Site = (el.Site.Contains(" ")) ? "" : el.Site,
                            TwitterName = (el.Twitter.Contains(" ")) ? "" : el.Twitter,
                            Position = el.Position,
                            District = el.District,
                            Gemeente = ToPascalCase(el.Town),
                            Postalcode = el.Postalcode,
                            Gender = el.Gender,
                            DateOfBirth = el.DateOfBirth,
                            Records = new List<Record>(),
                            Themes = new List<Theme>()
                        };
                    }
                    else
                    {
                        personCheck = new Person
                        {
                            ItemId = el.Id,
                            Name = el.Full_name,
                            IsTrending = false,
                            IconURL = (el.Twitter.Contains(" ")) ?
                                pbSubplatform.Settings.FirstOrDefault(ss => ss.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).Value
                                : "https://twitter.com/" + el.Twitter + "/profile_image?size=original",
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
                            SocialMediaLink = (el.Facebook.Contains(" ")) ? "" : el.Facebook,
                            Site = (el.Site.Contains(" ")) ? "" : el.Site,
                            TwitterName = (el.Twitter.Contains(" ")) ? "" : el.Twitter,
                            Position = el.Position,
                            District = el.District,
                            Gemeente = ToPascalCase(el.Town),
                            Postalcode = el.Postalcode,
                            Gender = el.Gender,
                            DateOfBirth = el.DateOfBirth,
                            Records = new List<Record>(),
                            Themes = new List<Theme>()
                        };
                    }

                    pbSubplatform.Items.Add(personCheck);
                }

                // Organisation
                Organisation organisationCheck = OrganisationsToAdd
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

                    OrganisationsToAdd.Add(organisationCheck);
                }
                else
                {
                    organisationCheck.People.Add(personCheck);

                    if (!organisationCheck.SubPlatforms.Contains(pbSubplatform))
                        organisationCheck.SubPlatforms.Add(pbSubplatform);
                }

                personCheck.Organisation = organisationCheck;
                pbSubplatform.Items.Add(organisationCheck);

                personsToAdd.Add(personCheck);
            }
            ctx.Persons.ForEachAsync(p =>
            {
                Person person = personsToAdd.FirstOrDefault(per => per.Equals(p));
                if (person != null)
                {
                    bool removed = personsToAdd.Remove(person);
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
                    Name = "Syri�",
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
                    IconURL = @"~/Content/Images/Themes/milieu.png",
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

            // Save all pending changes
            ctx.SaveChanges();
        }

        public static string ToPascalCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
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