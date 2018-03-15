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
    private static readonly  UnitOfWorkManager uow = new UnitOfWorkManager();
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
        Console.WriteLine("\n---------------- Info -----------------");
        Console.WriteLine("7) Toon alle records");
        Console.WriteLine("8) Toon alle items");
        Console.WriteLine("9) Toon subscribed items van geselecteerd profiel");
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
            selectedProfile.Subscriptions.Add(extensionMethods.SelectItem(itemMgr.GetItems()), true);
            accountMgr.ChangeProfile(selectedProfile);
            break;
          case 4:
            selectedProfile.Subscriptions.Remove(extensionMethods.SelectItem(selectedProfile.Subscriptions.Keys));
            accountMgr.ChangeProfile(selectedProfile);
            break;
          case 5:
            itemMgr.CheckTrend();
            break;
          case 6: 
            itemMgr.Seed2();
            break;
           
          case 7:
            extensionMethods.ShowRecords(itemMgr.GetRecords());
            break;
          case 8:
            extensionMethods.ShowItems(itemMgr.GetItems());
            break;
          case 9:
            extensionMethods.ShowSubcsribedItems(selectedProfile);
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

    private static void Seed()
    {
      //Injects seed data
      itemMgr.Seed();
      accountMgr.Seed();
      uow.Save();
      //accountMgr.SubscribeProfiles(itemMgr.GetItems());
    }
  }
}
