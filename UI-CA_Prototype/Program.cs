using PB.BL;
using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_CA_Prototype
{
  class Program
  {
    private static readonly ItemManager itemMgr = new ItemManager();
    private static readonly AccountManager accountMgr = new AccountManager();

    private static bool stop = false;

    static void Main(string[] args)
    {
      while (!stop)
      {
        Console.WriteLine("=======================================");
        Console.WriteLine("=== Prototype - Politieke barometer ===");
        Console.WriteLine("=======================================");
        Console.WriteLine("1) Seed (!! best eerst uitvoeren alvorens acties te testen !!)");
        Console.WriteLine("2) Generate alerts");
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
      }
      //Schrijft 2 test-records weg naar desktop
      //Write();

      //Vult repo's met testdata (true = test data met output; false = enkel seed laden, geen testoutput)
      Seed(true);
    }

    private static void DetectMenuAction()
    {
      bool inValidAction = false;
      do
      {
        Console.Write("Keuze: ");
        int keuze = int.Parse(Console.ReadLine());

        switch (keuze)
        {
          case 1:
            Seed(true);
            break;
          case 2:
            accountMgr.generateAlerts();
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

    private static void Seed(bool testSeedOutput)
    {
      itemMgr.Seed();
      accountMgr.Seed();
      accountMgr.SubscribeProfiles(itemMgr.GetItems());

      if (testSeedOutput)
      {
        //Seed test
        Console.WriteLine("Record met id 908683232232841218 (eerste record): ");
        Console.WriteLine(itemMgr.GetRecord(908683232232841218).ToString());

        Console.WriteLine("\nAlle Items:");
        itemMgr.GetItems().ToList().ForEach(i =>
        {
          if (i is Person)
          {
            Person p = (Person)i;
            Console.WriteLine("(" + p.ItemId + ") " + p.FirstName + " " + p.LastName + " - " + p.Records.Count + " records");
          }
        });
      }
    }


    private static void Write()
    {
      ItemManager mgr = new ItemManager();
      List<Record> records = new List<Record>();
      records.Add(new Record()
      {
        Hashtags = new List<string>(),
        Words = new List<string>() {
          "annouri",
          "kasper goethals",
          "arabië",
          "imade",
          "iran"
        },
        Date = DateTime.Parse("2017-09-11 04:53:38"),
        Politician = new List<string>() {
          "Imade",
          "Annouri"
        },
        Geo = "N/A",
        Id = 907104827896987600,
        User_Id = "N/A",
        Sentiment = new List<double>() {
          0,
          0
        },
        Retweet = true,
        Source = "twitter",
        URLs = new List<string>(){
          "http://pltwps.it/_JY894kJ"
        },
        Mentions = new List<string>()
      }
      );


      records.Add(new Record()
      {
        Hashtags = new List<string>()
        {
          "Firsts,"
        },
        Words = new List<string>()
        {
          "annouri",
          "imade",
          "reeks",
          "time"
        },
        Date = DateTime.Parse("2017-09-07 22:52:35"),
        Politician = new List<string>()
        {
          "Imade",
          "Annouri"
        },
        Geo = "N/A",
        Id = 905926801804980200,
        User_Id = "N/A",
        Sentiment = new List<double>()
        {
          0.7,
          1
        },
        Retweet = false,
        Source = "twitter",
        URLs = new List<string>()
        {
          "https://twitter.com/TIME/status/905785286092877824",
          "http://pltwps.it/_xV6mWwE"
        },
        Mentions = new List<string>()
      }
      );

      mgr.Write(records);
      Console.WriteLine("Er werd een testbestand genaamd 'structure.json' weggeschreven naazr uw desktop met 2 test-records");
      Console.ReadLine();
    }
  }
}
