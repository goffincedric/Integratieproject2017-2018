﻿using Newtonsoft.Json;
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
  public class ExtensionMethods
  {
    //Lets the user select an available profile
    public Profile SelectProfile(IEnumerable<Profile> profiles)
    {
      ShowProfiles(profiles);

      int keuze = 0;
      Console.Write("Keuze: ");
      int.TryParse(Console.ReadLine(), out keuze);

      return profiles.ElementAt(keuze-1);
    }
    
    //Shows a list of all available profiles
    public void ShowProfiles(IEnumerable<Profile> profiles)
    {
      int counter = 1;
      Console.WriteLine("Selecteer een account: ");
      profiles.ToList().ForEach(p =>
      {
        Console.WriteLine(counter + ") " + p);
        counter++;
      });
    }

    //Show all records sorted by Name, then by Date descending
    public void ShowRecords(IEnumerable<Record> records)
    {
      records.OrderBy(r => r.RecordPerson.FirstName).ThenByDescending(r => r.Date).ToList().ForEach(r => Console.WriteLine(r.Date.ToString() + " - " + r.RecordPerson.FirstName + " " + r.RecordPerson.LastName + " (" + r.Tweet_Id + ")"));
    }

    //Lets the user select an available item
    public Item SelectItem(IEnumerable<Item> items)
    {
      ShowItems(items);

      int keuze = 0;
      Console.Write("Keuze: ");
      int.TryParse(Console.ReadLine(), out keuze);

      return items.ElementAt(keuze-1);
    }

    //Shows all items
    public void ShowItems(IEnumerable<Item> items)
    {
      int counter = 1;
      Console.WriteLine("\n\nAlle Items:");
      items.ToList().ForEach(i =>
      {
        if (i is Person)
        {
          Person p = (Person)i;
          Console.WriteLine(counter + ") " + p);
          counter++;
        }
      });
    }

    //Shows all subscribed items from profile
    public void ShowSubcsribedItems(Profile profile)
    {
      Console.WriteLine("Subscribed items:");
      profile.Subscriptions.Where(pair => pair.Value).ToList().ForEach(pair => Console.WriteLine(pair.Key));
    }

    //Method to test write functionality of JsonConvert (read written json file on desktop for record-object structure)
    public void WriteTestRecords()
    {
      List<Record> records = new List<Record>();
      records.Add(new Record()
      {
        Hashtags = new List<Hashtag>(),
        Words = new List<Words>() {
          new Words("annouri"),
          new Words("kasper goethals"),
          new Words("arabië"),
          new Words("imade"),
          new Words("iran")
        },
        Date = DateTime.Parse("2017-09-11 04:53:38"),
        RecordPerson = new RecordPerson() { FirstName = "Imade", LastName = "Annouri" },
        Geo = "N/A",
        Tweet_Id = 907104827896987600,
        User_Id = "N/A",
        Sentiment = new Sentiment(0, 0),
        Retweet = true,
        Source = "twitter",
        URLs = new List<Url>(){
          new Url("http://pltwps.it/_JY894kJ")
        },
        Mentions = new List<Mention>()
      }
      );

      records.Add(new Record()
      {
        Hashtags = new List<Hashtag>()
        {
          new Hashtag("Firsts,")
        },
        Words = new List<Words>()
        {
          new Words("annouri"),
          new Words("imade"),
          new Words("reeks"),
          new Words("time")
        },
        Date = DateTime.Parse("2017-09-07 22:52:35"),
        RecordPerson = new RecordPerson() { FirstName = "Imade", LastName = "Annouri" },
        Geo = "N/A",
        Tweet_Id = 905926801804980200,
        User_Id = "N/A",
        Sentiment = new Sentiment(0.7, 1),
        Retweet = false,
        Source = "twitter",
        URLs = new List<Url>()
        {
          new Url("https://twitter.com/TIME/status/905785286092877824"),
          new Url("http://pltwps.it/_xV6mWwE")
        },
        Mentions = new List<Mention>()
      }
      );

      JsonSerializer serializer = new JsonSerializer();
      using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + @"\structure.json"))
      using (JsonWriter writer = new JsonTextWriter(sw))
      {
        serializer.Serialize(writer, records);
      }

      Console.WriteLine("Er werd een testbestand genaamd 'structure.json' weggeschreven naazr uw desktop met 2 test-records");
    }
  }
}
