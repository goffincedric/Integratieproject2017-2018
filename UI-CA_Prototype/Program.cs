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
    static void Main(string[] args)
    {
      //Schrijft 2 test-records weg naar desktop
      //Write();

      //Vult repo's met testdata (true = test data met output; false = enkel seed laden, geen testoutput)
      Seed(true);
    }

    private static void Seed(bool testSeedOutput)
    {
      ItemManager itemMgr = new ItemManager();
      itemMgr.Seed();

      if (testSeedOutput)
      {
        //Seed test
        Record record = itemMgr.GetRecord(908683232232841218);
        Console.WriteLine("Record met id 908683232232841218 (eerste record): ");
        Console.WriteLine(record.ToString());
      }

      Console.ReadLine();
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
