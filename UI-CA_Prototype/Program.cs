using PB.BL;
using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;
using PB.DAL.EF;
using Domain.JSONConversion;
using Mono.Options;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Items;


namespace UI_CA_Prototype
{
    class Program
    {
        private static IntegratieDbContext Context = new IntegratieDbContext();
        private static readonly IntegratieUserStore Store = new IntegratieUserStore(Context);
        private static readonly AccountManager AccountMgr = new AccountManager(Store);

        private static bool WillSeed = false;
        private static OptionSet CLIOptions;

        private static readonly UnitOfWorkManager Uow = new UnitOfWorkManager();
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

            //Injects seed data
            if (WillSeed) Seed();

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

                try
                {
                    DetectMenuAction();
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                }
                Console.WriteLine("\n");
            }
        }

        private static void HandleCLIArgs(string[] args)
        {
            List<Subplatform> subplatformsToClear = new List<Subplatform>();
            //Available CLI options
            CLIOptions = new OptionSet {
                {"s|seed", "Will use seed data from TextGainAPI to seed the database", ns => WillSeed = (ns != null) },
                {"c|cleanup-db=", "Clean the database of old records for the given subplatforms", cdb =>
                    {
                        Subplatform subplatform = SubplatformMgr.GetSubplatforms().First(s => s.Name.Replace(" ", "").ToLower().Equals(cdb.Replace(" ", "").ToLower()));
                        subplatformsToClear.Add(subplatform);
                    }
                },
                { "h|help", "Shows this message and exit", h =>
                    {
                        ShowHelp();
                        Environment.Exit(0);
                    }
                },
            };

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
                Environment.Exit(1);
            }

            if (subplatformsToClear.Count != 0)
            {
                try
                {
                    subplatformsToClear.ForEach(s =>
                    {
                        ItemMgr.CleanupOldRecords(s);
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

        private static void DetectMenuAction()
        {
            bool inValidAction = false;
            int keuze;
            do
            {
                Console.Write("Keuze: ");
                int.TryParse(Console.ReadLine(), out keuze);
                Console.WriteLine("\n");

                switch (keuze)
                {
                    case 1:
                        ExtensionMethods.WriteTestRecords();
                        break;
                    case 2:
                        SelectedProfile = ExtensionMethods.SelectProfile(AccountMgr.GetProfiles());
                        break;
                    case 3:
                        SelectedSubplatform = ExtensionMethods.SelectSubplatform(SubplatformMgr.GetSubplatforms());
                        break;
                    case 4:
                        if (SelectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
                        SelectedProfile.Subscriptions.Add(ExtensionMethods.SelectItem(ItemMgr.GetItems()));
                        AccountMgr.ChangeProfile(SelectedProfile);
                        break;
                    case 5:
                        if (SelectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
                        SelectedProfile.Subscriptions.Remove(ExtensionMethods.SelectItem(SelectedProfile.Subscriptions));
                        AccountMgr.ChangeProfile(SelectedProfile);
                        break;
                    case 6:
                        ItemMgr.CheckTrend();
                        break;
                    case 7:
                        if (SelectedSubplatform == null) throw new Exception("U heeft nog geen subplatform geselecteerd, gelieve er eerst een te kiezen");
                        ItemMgr.CleanupOldRecords(SelectedSubplatform);
                        break;
                    case 8:
                        Seed();
                        break;
                    case 9:
                        ItemMgr.GenerateProfileAlerts(SelectedProfile);
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
                    Settings = new List<SubplatformSetting>(),
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>()
                };
            }

            //Injects api seed data
            APICalls restClient = new APICalls()
            {
                API_URL = "http://kdg.textgain.com/query"
            };

            //Individueel api aanspreken
            List<JClass> requestedRecords = new List<JClass>();
            requestedRecords.AddRange(restClient.RequestRecords("Annick De Ridder"));
            requestedRecords.AddRange(restClient.RequestRecords("Caroline Bastiaens"));
            requestedRecords.AddRange(restClient.RequestRecords("Jan Bertels"));
            requestedRecords.AddRange(restClient.RequestRecords("Vera Celis"));
            requestedRecords.AddRange(restClient.RequestRecords("Dirk De Kort"));
            requestedRecords.AddRange(restClient.RequestRecords("Imade Annouri"));
            requestedRecords.AddRange(restClient.RequestRecords("Caroline Gennez"));
            requestedRecords.AddRange(restClient.RequestRecords("Kathleen Helsen"));
            requestedRecords.AddRange(restClient.RequestRecords("Marc Hendrickx"));
            requestedRecords.AddRange(restClient.RequestRecords("Jan Hofkens"));
            requestedRecords.AddRange(restClient.RequestRecords("Yasmine Kherbache"));
            requestedRecords.AddRange(restClient.RequestRecords("Kathleen Krekels"));
            requestedRecords.AddRange(restClient.RequestRecords("Ingrid Pira"));

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
