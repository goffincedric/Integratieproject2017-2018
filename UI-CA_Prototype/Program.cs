using Mono.Options;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.BL.Interfaces;
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
        private static readonly IAccountManager AccountMgr = new AccountManager(new IntegratieUserStore(Uow.UnitOfWork), Uow);
        private static readonly IItemManager ItemMgr = new ItemManager(Uow);
        private static readonly ISubplatformManager SubplatformMgr = new SubplatformManager(Uow);

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
                Console.WriteLine("6) Verstuur weekly reviews");
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
                        AccountMgr.RemoveSubscription(SelectedProfile, ExtensionMethods.SelectItem(SelectedProfile.Subscriptions));
                        break;
                    case 6:
                        AccountMgr.SendWeeklyReviews();
                        break;
                    case 7:
                        if (SelectedSubplatform == null) throw new Exception("U heeft nog geen subplatform geselecteerd, gelieve er eerst een te kiezen");
                        int days = int.Parse(SelectedSubplatform.Settings.FirstOrDefault(se => se.SettingName.Equals(Setting.Platform.DAYS_TO_KEEP_RECORDS)).Value);
                        ItemMgr.CleanupOldRecords(SelectedSubplatform);
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
            bool CleanupAll = false;
            bool GenerateAlerts = false;
            bool QuietMode = false;
            //Available CLI options
            CLIOptions = new OptionSet {
                {"c|cleanup-db:", "Cleans the database of old records for the given subplatform and exits program. If no subplatforms are given, the database will clean up old records for all subplatforms. This option can be called multiple times.", cdb =>
                    {
                        if (cdb == null && SubplatformsToClear.Count == 0) {
                            SubplatformsToClear.AddRange(SubplatformMgr.GetSubplatforms());
                            CleanupAll = true;
                        }
                        if (!CleanupAll)
                        {
                            if (Subplatforms == null) Subplatforms = SubplatformMgr.GetSubplatforms().ToList();
                            Subplatform subplatform = Subplatforms.FirstOrDefault(s => s.Name.Replace(" ", "").ToLower().Equals(cdb.Replace(" ", "").ToLower()));
                            if (subplatform == null) Console.WriteLine("'" + cdb + "' is not a known subplatform");
                            else SubplatformsToClear.Add(subplatform);
                        }
                    }
                },
                {"g|generate-alerts", "Will generate alerts for all current profiles with subscriptions and update item trending status.", g => GenerateAlerts = (g != null) },
                {"h|help", "Shows this message and exits.", h =>
                    {
                        ShowHelp();
                        Environment.Exit(0);
                    }
                },
                {"q|quiet", "Will execute parsed commands and close quietly.", q => QuietMode = (q != null) },
                {"s|sync", "Will use data from TextGainAPI to sync the database.", s => WillSeed = (s != null) },
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
            if (WillSeed)
            {
                Console.WriteLine("=======================");
                Console.WriteLine("==== Seed from API ====");
                Console.WriteLine("=======================");
                Seed();
                Console.WriteLine(" ");
            }

            //Clears subplatforms
            if (SubplatformsToClear.Count != 0 || CleanupAll)
            {
                Console.WriteLine("====================");
                Console.WriteLine("==== Cleanup DB ====");
                Console.WriteLine("====================");
                try
                {
                    SubplatformsToClear.ForEach(s =>
                    {
                        int days = int.Parse(s.Settings.FirstOrDefault(se => se.SettingName.Equals(Setting.Platform.DAYS_TO_KEEP_RECORDS)).Value);
                        Console.WriteLine("Clear " + s.Name + " from records older than " + days + " days");
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
                Console.WriteLine(" ");
            }

            //Generates alerts for all profiles & Updates trending status items
            if (GenerateAlerts)
            {
                Console.WriteLine("=========================");
                Console.WriteLine("==== Generate Alerts ====");
                Console.WriteLine("=========================");
                List<Item> itemsToUpdate = new List<Item>();
                AccountMgr.GenerateAllAlerts(ItemMgr.GetItems(), out itemsToUpdate);
                ItemMgr.ChangeItems(itemsToUpdate);
                Console.WriteLine(" ");
            }

            if (QuietMode)
            {
                Console.WriteLine("Done");
                Environment.Exit(0);
                Console.ReadKey();
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
            if (!ItemManager.IsSyncing)
            {
                ItemManager.IsSyncing = true;
                //Makes PB subplatform
                Subplatform pbSubplatform = SubplatformMgr.GetSubplatforms().FirstOrDefault(s => s.Name.ToLower().Equals("Politieke Barometer".ToLower()));
                ItemMgr.SyncDatabase(pbSubplatform);

                ItemManager.IsSyncing = false;
            }
        }
    }
}
