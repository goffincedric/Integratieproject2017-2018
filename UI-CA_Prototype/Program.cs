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
    private static readonly ItemManager itemMgr = new ItemManager();
    private static readonly AccountManager accountMgr = new AccountManager();

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
        Console.WriteLine("5) Genereer alerts voor geselecteerd account");
        Console.WriteLine("\n---------------- Info -----------------");
        Console.WriteLine("6) Toon alle records");
        Console.WriteLine("7) Toon alle items");
        Console.WriteLine("8) Toon subscribed items van geselecteerd profiel");
        Console.WriteLine("0) Afsluiten");
        Console.WriteLine("10) Test alert");
        Console.WriteLine("19) TEST SEARCH");
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
            selectedProfile.Subscriptions.Add(extensionMethods.SelectItem(itemMgr.GetItems()), true);
            accountMgr.ChangeProfile(selectedProfile);
            break;
          case 4:
            selectedProfile.Subscriptions.Remove(extensionMethods.SelectItem(selectedProfile.Subscriptions.Keys));
            accountMgr.ChangeProfile(selectedProfile);
            break;
          case 5:
            Console.WriteLine("Out of order");
           //accountMgr.generateAlerts(); 
            break;
          case 6:

           extensionMethods.ShowRecords(itemMgr.GetRecords());
            break;
          case 7:
            extensionMethods.ShowItems(itemMgr.GetItems());
            break;
          case 8:
            extensionMethods.ShowSubcsribedItems(selectedProfile);
            break;
          case 19:
            testSearch();
            break;
          case 0:
            stop = true;
            return;
          case 10:
<<<<<<< HEAD
            itemMgr.CheckTrend();
=======
                        itemMgr.CheckTrend();
>>>>>>> b6c174ef258150646d596be61bd3069333d3431c
            break;
          default:
            Console.WriteLine("Geen geldige keuze!");
            inValidAction = true;
            break;
        }
      } while (inValidAction);
    }

    private static void Seed()
    {
      //Injects seed data
      itemMgr.Seed();
      accountMgr.Seed();
      //accountMgr.SubscribeProfiles(itemMgr.GetItems());
    }
  }
}
