using Mono.Options;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI_CA_Prototype
{
    class Program
    {
        private static bool WillSeed = false;
        private static OptionSet CLIOptions;

        private static readonly UnitOfWorkManager Uow = new UnitOfWorkManager();
        private static readonly AccountManager AccountMgr = new AccountManager(new IntegratieUserStore(Uow.UnitOfWork), Uow);
        private static readonly ItemManager ItemMgr = new ItemManager(Uow);
        private static readonly SubplatformManager SubplatformMgr = new SubplatformManager(Uow);

        private static readonly ExtensionMethods ExtensionMethods = new ExtensionMethods();

        private static bool Stop = false;
        private static Profile SelectedProfile;
        private static Subplatform SelectedSubplatform;

        static void Main(string[] args)
        {
            //Handles CLI options/args and acts accordingly
            HandleCLIArgs(args);

            //Menu
            while (!Stop)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("=== Prototype - Politieke barometer ===");
                Console.WriteLine("=======================================");
                Console.WriteLine("Huidig subplatform: " + SelectedSubplatform);
                Console.WriteLine("Huidig account: " + SelectedProfile);
                Console.WriteLine("=======================================");
                Console.WriteLine("1) Schrijf testrecords naar desktop");
                Console.WriteLine("2) Selecteer account");
                Console.WriteLine("3) Selecteer subplatform");
                Console.WriteLine("4) Voeg subscription toe");
                Console.WriteLine("5) Verwijder subscription");
                Console.WriteLine("6) Show gemiddelde tweets/dag per persoon voorbij 14 dagen");
                Console.WriteLine("7) Verwijder oude records uit database");
                Console.WriteLine("8) Voeg API data toe");
                Console.WriteLine("9) Voeg alerts to aan selected profile");
                Console.WriteLine("---------------- Info -----------------");
                Console.WriteLine("10) Toon alle records");
                Console.WriteLine("11) Toon alle persons");
                Console.WriteLine("12) Toon subscribed items van geselecteerd profiel");
                Console.WriteLine("-------------- Commands ---------------");
                Console.WriteLine("99) Toon de CLI help pagina");
                Console.WriteLine("0) Afsluiten");

                //try
                //{
                    DetectMenuAction();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine();
                //    Console.WriteLine(e.Message);
                //    Console.WriteLine();
                //}
                Console.WriteLine("\n");
            }
        }

        private static void DetectMenuAction()
        {
            bool inValidAction = false;
            do
            {
                Console.Write("Keuze: ");
               int.TryParse(Console.ReadLine(), out int keuze);
                Console.WriteLine("\n");

                switch (keuze)
                {
                    case 1:
                        ExtensionMethods.WriteTestRecords(ItemMgr.GetRecords());
                        break;
                    case 2:
                        SelectedProfile = ExtensionMethods.SelectProfile(AccountMgr.GetProfiles());
                        break;
                    case 3:
                        SelectedSubplatform = ExtensionMethods.SelectSubplatform(SubplatformMgr.GetSubplatforms());
                        break;
                    case 4:
                        if (SelectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
                        AccountMgr.AddSubscription(SelectedProfile, ExtensionMethods.SelectItem(ItemMgr.GetPersons()));
                        break;
                    case 5:
                        if (SelectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
                        ItemMgr.RemoveSubscription(SelectedProfile, ExtensionMethods.SelectItem(SelectedProfile.Subscriptions));
                        break;
                    case 6:
                        Console.WriteLine("OUT OF ORDER");
                        //ItemMgr.CheckTrend();
                        break;
                    case 7:
                        if (SelectedSubplatform == null) throw new Exception("U heeft nog geen subplatform geselecteerd, gelieve er eerst een te kiezen");
                        int days = int.Parse(SelectedSubplatform.Settings.FirstOrDefault(se => se.SettingName.Equals(Setting.Platform.DAYS_TO_KEEP_RECORDS)).Value);
                        ItemMgr.CleanupOldRecords(SelectedSubplatform, days);
                        break;
                    case 8:
                        Seed();
                        break;
                    case 9:
                        AccountMgr.GenerateProfileAlerts(SelectedProfile);
                        break;
                    case 10:
                        ExtensionMethods.ShowRecords(ItemMgr.GetRecords());
                        break;
                    case 11:
                        ExtensionMethods.ShowPersons(ItemMgr.GetPersons());
                        break;
                    case 12:
                        ExtensionMethods.ShowSubScribedItems(SelectedProfile);
                        break;
                    case 99:
                        ShowHelp();
                        break;
                    case 0:
                        Stop = true;
                        return;
                    default:
                        Console.WriteLine("Geen geldige keuze!");
                        inValidAction = true;
                        break;
                }
            } while (inValidAction);
        }

        private static void HandleCLIArgs(string[] args)
        {
            List<Subplatform> SubplatformsToClear = new List<Subplatform>();
            List<Subplatform> Subplatforms = null;
            bool cleanupAll = false;
            //Available CLI options
            CLIOptions = new OptionSet {
                {"s|sync", "Will use data from TextGainAPI to sync the database.", ns => WillSeed = (ns != null) },
                {"c|cleanup-db:", "Cleans the database of old records for the given subplatform and exits program. If no subplatforms are given, the database will clean up old records for all subplatforms. This option can be called multiple times.", cdb =>
                    {
                        if (cdb == null && SubplatformsToClear.Count == 0) {
                            SubplatformsToClear.AddRange(SubplatformMgr.GetSubplatforms());
                            cleanupAll = true;
                        }
                        if (!cleanupAll)
                        {
                            if (Subplatforms == null) Subplatforms = SubplatformMgr.GetSubplatforms().ToList();
                            Subplatform subplatform = Subplatforms.FirstOrDefault(s => s.Name.Replace(" ", "").ToLower().Equals(cdb.Replace(" ", "").ToLower()));
                            if (subplatform == null) Console.WriteLine("'" + cdb + "' is not a known subplatform");
                            else SubplatformsToClear.Add(subplatform);
                        }
                    }
                },
                { "h|help", "Shows this message and exits", h =>
                    {
                        ShowHelp();
                        Environment.Exit(0);
                    }
                },
            };


            /* ====== Start arg handeling  ====== */

            List<string> extra;
            try
            {
                //Parse the command line
                extra = CLIOptions.Parse(args);
            }
            catch (OptionException e)
            {
                //Output parsing error message (ex. expects number but gets char)
                Console.Write(AppDomain.CurrentDomain.FriendlyName + ": ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try '" + AppDomain.CurrentDomain.FriendlyName + " --help' for more information.");
                Console.Write("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }


            //Injects seed data
            if (WillSeed) Seed();

            //Clear 
            SubplatformsToClear.ForEach(Console.WriteLine);

            if (SubplatformsToClear.Count != 0)
            {
                try
                {
                    SubplatformsToClear.ForEach(s =>
                    {
                        int days = int.Parse(s.Settings.FirstOrDefault(se => se.SettingName.Equals(Setting.Platform.DAYS_TO_KEEP_RECORDS)).Value);
                        ItemMgr.CleanupOldRecords(s, days);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
                Console.WriteLine("Done");
                Environment.Exit(0);
            }
        }

        private static void ShowHelp()
        {
            //Show app description
            Console.WriteLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " [OPTIONS]\n");

            //Output the CLI options
            Console.WriteLine("Options:");
            CLIOptions.WriteOptionDescriptions(Console.Out);
        }

        private static void Seed()
        {
            //Makes PB subplatform
            Subplatform pbSubplatform = SubplatformMgr.GetSubplatforms().FirstOrDefault(s => s.Name.ToLower().Equals("Politieke Barometer".ToLower()));

            if (pbSubplatform == null)
            {
                pbSubplatform = new Subplatform()
                {
                    Name = "Politieke Barometer",
                    URL = "DUMMYURL",
                    DateOnline = DateTime.Now,
                    Settings = new List<SubplatformSetting>()
                    {
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                            Value = "31"
                        }
                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>()
                };
            }

            List<Item> OrganisationsToAdd = new List<Item>()
            {
                new Organisation()
                {
                    Name = "Partij van de Arbeid",
                    Abbreviation = "PVDA-PTB",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Christen-Democratisch en Vlaams",
                    Abbreviation = "CD&V",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Socialistische Partij Anders",
                    Abbreviation = "SP.A",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Open Vlaamse Liberalen en Democraten",
                    Abbreviation = "Open Vld",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Groen",
                    Abbreviation = "Groen",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Nieuw-Vlaamse Alliantie",
                    Abbreviation = "N-VA",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Vlaams Belang",
                    Abbreviation = "VB",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                }
            };

            //Replace new organisations with existing alerts
            ItemMgr.GetOrganisations().ToList().ForEach(o =>
            {
                Organisation organisation = (Organisation)OrganisationsToAdd.FirstOrDefault(org => org.Equals(o));
                if (organisation != null) OrganisationsToAdd[OrganisationsToAdd.IndexOf(organisation)] = o;
            });

            //OrganisationsToAdd.ForEach(o => ItemMgr.AddOrganisation(o.Name, o.Description, o.Abbreviation, o.SocialMediaLink, o.IconURL));
            ItemMgr.AddItems(OrganisationsToAdd);


            //Injects api seed data
            APICalls restClient = new APICalls()
            {
                API_URL = "http://kdg.textgain.com/query"
            };

            //Individueel api aanspreken
            List<JClass> requestedRecords = new List<JClass>();
            requestedRecords.AddRange(restClient.RequestRecords(since: DateTime.Now.AddDays(-int.Parse(pbSubplatform.Settings.First(s => s.SettingName.Equals(Setting.Platform.DAYS_TO_KEEP_RECORDS)).Value))));
            //requestedRecords.AddRange(restClient.RequestRecords("Annick De Ridder", new DateTime(2017, 1, 1)));

            //Convert JClass to Record and persist to database
            requestedRecords.ForEach(r => r.Subplatforms.Add(pbSubplatform));
            ItemMgr.JClassToRecord(requestedRecords);

            // Api aanspreken via collectie
            //List<APIQuery> apiQueries = new List<APIQuery>()
            //{
            //    new APIQuery("Annick De Ridder"),
            //    new APIQuery("Caroline Bastiaens"),
            //    new APIQuery("Jan Bertels"),
            //    new APIQuery("Vera Celis"),
            //    new APIQuery("Dirk De Kort"),
            //    new APIQuery("Imade Annouri"),
            //    new APIQuery("Caroline Gennez"),
            //    new APIQuery("Kathleen Helsen"),
            //    new APIQuery("Marc Hendrickx"),
            //    new APIQuery("Jan Hofkens"),
            //    new APIQuery("Yasmine Kherbache"),
            //    new APIQuery("Kathleen Krekels"),
            //    new APIQuery("Ingrid Pira")
            //};
            //itemMgr.JClassToRecord(restClient.RequestRecords(apiQueries));


            //Old seed method, deprecated json structure
            //itemMgr.Seed();
            //accountMgr.Seed();
            //accountMgr.SubscribeProfiles(itemMgr.GetItems());


            //private static void newAccount()
            //{
            //  Profile profile = extensionMethods.CreateAccount();
            //  accountMgr.AddProfile(profile.UserName, profile.Email);
            //}
        }
    }
}
