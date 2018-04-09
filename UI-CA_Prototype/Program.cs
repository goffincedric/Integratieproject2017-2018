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
<<<<<<< HEAD
using PB.DAL.EF;
=======
using Domain.JSONConversion;
>>>>>>> master

namespace UI_CA_Prototype
{
  class Program
  {
    private static IntegratieDbContext context = new IntegratieDbContext();
    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
    private static readonly ItemManager itemMgr = new ItemManager(uow);
    private static readonly IntegratieUserStore store = new IntegratieUserStore(context);
    private static readonly AccountManager accountMgr = new AccountManager(store,uow);

    private static readonly ExtensionMethods extensionMethods = new ExtensionMethods();

    private static bool stop = false;
    private static Profile selectedProfile = null;

<<<<<<< HEAD
    static void Main(string[] args)
    {
      //Injects seed data
      //Seed data structure deprecated
      //Seed();
=======
        static void Main(string[] args)
        {
            //Injects seed data
            //Seed data structure deprecated
            Seed();
>>>>>>> master

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
        Console.WriteLine("7) Voeg API data toe");
        Console.WriteLine("8) Voeg alerts to aan selected profile");
        Console.WriteLine("---------------- Info -----------------");
        Console.WriteLine("9) Toon alle records");
        Console.WriteLine("10) Toon alle persons");
        Console.WriteLine("11) Toon subscribed items van geselecteerd profiel");
        Console.WriteLine("12) Maak nieuw account");
        Console.WriteLine("0) Afsluiten");

        try
        {
          DetectMenuAction();
        }
<<<<<<< HEAD
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
=======

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
                        if (selectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
                        selectedProfile.Subscriptions.Add(extensionMethods.SelectItem(itemMgr.GetItems()));
                        accountMgr.ChangeProfile(selectedProfile);
                        break;
                    case 4:
                        if (selectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
                        selectedProfile.Subscriptions.Remove(extensionMethods.SelectItem(selectedProfile.Subscriptions));
                        accountMgr.ChangeProfile(selectedProfile);
                        break;
                    case 5:
                        itemMgr.CheckTrend();
                        break;
                    case 6:
                        itemMgr.Seed(false);
                        Console.WriteLine("Nieuwe seed data toegevoegd");
                        break;
                    case 7:
                        break;
                    case 8:
                        itemMgr.GenerateProfileAlerts(selectedProfile);
                        break;
                    case 9:
                        extensionMethods.ShowRecords(itemMgr.GetRecords());
                        break;
                    case 10:
                        extensionMethods.ShowPersons(itemMgr.GetPersons());
                        break;
                    case 11:
                        extensionMethods.ShowSubScribedItems(selectedProfile);
                        break;
                    case 12:
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
>>>>>>> master

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
<<<<<<< HEAD
          case 1:
            extensionMethods.WriteTestRecords();
            break;
          case 2:
            selectedProfile = extensionMethods.SelectProfile(accountMgr.GetProfiles()); ;
            break;
          case 3:
            if (selectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
            selectedProfile.Subscriptions.Add(extensionMethods.SelectItem(itemMgr.GetItems()));
             accountMgr.ChangeProfile(selectedProfile);
            break;
          case 4:
            if (selectedProfile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
            selectedProfile.Subscriptions.Remove(extensionMethods.SelectItem(selectedProfile.Subscriptions));
             accountMgr.ChangeProfile(selectedProfile);
            break;
          case 5:
            itemMgr.CheckTrend();
            break;
          case 6:
            itemMgr.Seed(false);
            Console.WriteLine("Nieuwe seed data toegevoegd");
            break;
          case 7:
            APICalls restClient = new APICalls()
            {
              API_URL = "http://kdg.textgain.com/query"
            };
            itemMgr.JClassToRecord(restClient.RequestRecords("Annick De Ridder"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Caroline Bastiaens"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Jan Bertels"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Vera Celis"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Dirk De Kort"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Imade Annouri"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Caroline Gennez"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Kathleen Helsen"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Marc Hendrickx"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Jan Hofkens"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Yasmine Kherbache"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Kathleen Krekels"));
            //itemMgr.JClassToRecord(restClient.RequestRecords("Ingrid Pira"));
            break;
          case 8:
            itemMgr.GenerateProfileAlerts(selectedProfile);
            break;
          case 9:
            extensionMethods.ShowRecords(itemMgr.GetRecords());
            break;
          case 10:
            extensionMethods.ShowPersons(itemMgr.GetPersons());
            break;
          case 11:
            extensionMethods.ShowSubScribedItems(selectedProfile);
            break;
          case 12:
            //newAccount();
            break;
          case 0:
            stop = true;
            return;
          default:
            Console.WriteLine("Geen geldige keuze!");
            inValidAction = true;
            break;
=======
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
            itemMgr.JClassToRecord(requestedRecords);

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
>>>>>>> master
        }
      } while (inValidAction);
    }


    //private static void newAccount()
    //{
    //  Profile profile = extensionMethods.CreateAccount();
    //  accountMgr.AddProfile(profile.UserName, profile.Email);
    //}
    private static void Seed()
    {
      //Injects seed data
      itemMgr.Seed();
      //accountMgr.Seed();
      //accountMgr.SubscribeProfiles(itemMgr.GetItems());
    }
  }
}
