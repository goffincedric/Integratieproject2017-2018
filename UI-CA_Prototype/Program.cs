using Newtonsoft.Json;
using PB.BL;
using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;

namespace UI_CA_Prototype
{
    class Program
    {
        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private static readonly ItemManager itemMgr = new ItemManager(uow);
        private static readonly AccountManager accountMgr = new AccountManager(uow);

        private static readonly ExtensionMethods extensionMethods = new ExtensionMethods();

        private static bool stop = false;
        private static Profile selectedProfile;

        static void Main(string[] args)
        {
            //Injects seed data
            Seed();
            //Menu
            while (!stop)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("=== Prototype - Politieke barometer ===");
                Console.WriteLine("=======================================");
                Console.WriteLine("Huidige account: " + selectedProfile);
                Console.WriteLine("=======================================");
                Console.WriteLine("1) Schrijf testrecords naar desktop");
                Console.WriteLine("2) Selecteer account");
                Console.WriteLine("3) Voeg subscription toe");
                Console.WriteLine("4) Verwijder subscription");
                Console.WriteLine("5) Show Gemiddelde tweets/Dag per persoon voorbij 14 dagen");
                Console.WriteLine("6) Voeg 2de deel record data toe");
                Console.WriteLine("7) Voeg alerts to aan selected profile");
                Console.WriteLine("---------------- Info -----------------");
                Console.WriteLine("8) Toon alle records");
                Console.WriteLine("9) Toon alle items");
                Console.WriteLine("10) Toon subscribed items van geselecteerd profiel");
                Console.WriteLine("11) Maak nieuw account");
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

        private static void testSearch()
        {
            //List<Profile> test =  accountMgr.searchUsers();

            // test.ForEach(t => Console.WriteLine(t.Username)); 
            JsonConvert.DeserializeObject<List<Record>>(File.ReadAllText(@"TestData\textgaindump.json")).ForEach(r => Console.WriteLine(r.ToString()));
        }

        private static void DetectMenuAction()
        {
            bool inValidAction = false;
            do
            {
                Console.Write("Keuze: ");
                int keuze = int.Parse(Console.ReadLine());
                Console.WriteLine("\n");

                switch (keuze)
                {
                    case 1:
                        extensionMethods.WriteTestRecords();
                        break;
                    case 2:
                        selectedProfile = extensionMethods.SelectProfile(accountMgr.GetProfiles()); ;
                        break;
                    case 3:
                        selectedProfile.Subscriptions.Add(extensionMethods.SelectItem(itemMgr.GetItems()));
                        accountMgr.ChangeProfile(selectedProfile);
                        break;
                    case 4:
                        selectedProfile.Subscriptions.Remove(extensionMethods.SelectItem(selectedProfile.Subscriptions));
                        accountMgr.ChangeProfile(selectedProfile);
                        break;
                    case 5:
                        itemMgr.CheckTrend();
                        break;
                    case 6:
                        itemMgr.Seed(false);
                        itemMgr.GenerateProfileAlerts(selectedProfile);
                        break;
                    case 7:
                        itemMgr.GenerateProfileAlerts(selectedProfile);
                        break;
                    case 8:
                        extensionMethods.ShowRecords(itemMgr.GetRecords());
                        break;
                    case 9:
                        extensionMethods.ShowItems(itemMgr.GetItems());
                        break;
                    case 10:
                        extensionMethods.ShowSubScribedItems(selectedProfile);
                        break;
                    case 11:
                        newAccount();
                        break;

                    case 0:
                        stop = true;
                        return;
                    default:
                        Console.WriteLine("Geen geldige keuze!");
                        inValidAction = true;
                        break;
                }
            } while (inValidAction);
        }


<<<<<<< HEAD
        private static void newAccount()
        {
            Profile profile = extensionMethods.CreateAccount();
            accountMgr.AddProfile(profile);
        }
        private static void Seed()
        {
            //Injects seed data
            itemMgr.Seed();
            accountMgr.Seed();
            //accountMgr.SubscribeProfiles(itemMgr.GetItems());
        }
=======
    private static void newAccount()
    {
      Profile profile = extensionMethods.CreateAccount();
      accountMgr.AddProfile(profile);
    }
    private static void Seed()
    {
      //Injects seed data
      itemMgr.Seed();
      //accountMgr.Seed();
      //accountMgr.SubscribeProfiles(itemMgr.GetItems());
>>>>>>> parent of 3239529... Revert "Merge branch 'master' into Lins"
    }
}
