using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class AccountManager : IAccountManager
  {
    IProfileRepoHC ProfileRepo = new ProfileRepoHC();

    #region Profile
    public Profile AddProfile(string username, string password, string email, Role role = Role.USER)
    {
      Profile profile = new Profile()
      {
        Username = username,
        Password = password,
        Email = email,
        Role = role
      };
      return AddProfile(profile);
    }

    private Profile AddProfile(Profile profile)
    {
      return ProfileRepo.CreateProfile(profile);
    }

    public void ChangeProfile(Profile profile)
    {
      ProfileRepo.UpdateProfile(profile);
    }

    public Profile GetProfile(string username)
    {
      return ProfileRepo.ReadProfile(username);
    }

    public IEnumerable<Profile> GetProfiles()
    {
      return ProfileRepo.ReadProfiles();
    }

    public void RemoveProfile(string username)
    {
      ProfileRepo.DeleteProfile(username);
    }
    #endregion

    public void Seed()
    {
      List<Profile> profiles = new List<Profile>()
      {
        new Profile()
        {
          Email = "thomas.verhoeven@student.kdg.be",
          Username = "verhoeventhomas",
          Password = "schlack1",
          Role = Role.USER,
          Subscriptions = new Dictionary<Item, bool>(),
          Alerts = new List<Alert>()
        },
        new Profile()
        {
          Email = "cedric.goffin@student.kdg.be",
          Username = "goffincedric",
          Password = "schlack2",
          Role = Role.USER,
          Subscriptions = new Dictionary<Item, bool>(),
          Alerts = new List<Alert>()
        },
        new Profile()
        {
          Email = "stef.havermans@student.kdg.be",
          Username = "haversmansstef",
          Password = "schlack3",
          Role = Role.USER,
          Subscriptions = new Dictionary<Item, bool>(),
          Alerts = new List<Alert>()
        },
        new Profile()
        {
          Email = "lotte.marien@student.kdg.be",
          Username = "marienlotte",
          Password = "schlack4",
          Role = Role.USER,
          Subscriptions = new Dictionary<Item, bool>(),
          Alerts = new List<Alert>()
        },
        new Profile()
        {
          Email = "lins.vannijlen@student.kdg.be",
          Username = "vannijlenlins",
          Password = "schlack5",
          Role = Role.USER,
          Subscriptions = new Dictionary<Item, bool>(),
          Alerts = new List<Alert>()
        },
        new Profile()
        {
          Email = "celine.verwilligen@student.kdg.be",
          Username = "verwilligenceline",
          Password = "schlack6",
          Role = Role.USER,
          Subscriptions = new Dictionary<Item, bool>(),
          Alerts = new List<Alert>()
        }
      };

      profiles.ForEach(p => ProfileRepo.CreateProfile(p));
    }

    // Hardcoded subscriptions --> Voeg toe via menu

    // Fout
    /*
    public void SubscribeProfiles(IEnumerable<Item> items)
    {
      List<Profile> profiles = GetProfiles().ToList();
      profiles[0].Subscriptions.Add(items.ToList()[0], true);
      profiles[0].Subscriptions.Add(items.ToList()[1], true);
      profiles[1].Subscriptions.Add(items.ToList()[2], false);
      profiles[1].Subscriptions.Add(items.ToList()[5], true);
    }
    */

    public Dictionary<Profile, Alert> generateAlerts()
    {
      Dictionary<Profile, Alert> newAlerts = new Dictionary<Profile, Alert>();
      List<Profile> profiles = GetProfiles().ToList();
      
      profiles.ForEach(profile =>
      {
        foreach(KeyValuePair<Item, bool> subscription in profile.Subscriptions)
        {
          if (subscription.Value)
          {
            List<Record> sortedByDate = new List<Record>();
            List<Record> huidige = new List<Record>();
            List<Record> vorige = new List<Record>();

            sortedByDate = subscription.Key.Records.OrderByDescending(r => r.Date).ToList();
            
            //Toont alle records van een subscribed item
            //sortedByDate.ForEach(r => Console.WriteLine(r.Date.ToString() + " - " + r.Politician[0] + " " + r.Politician[1] + " (" + r.Id + ")"));

            DateTime huidigeDate = sortedByDate[0].Date.Date;
            DateTime vorigeDate = sortedByDate.First(r => huidigeDate.Date.CompareTo(r.Date) > 0).Date.Date;

            huidige = sortedByDate.FindAll(r => r.Date.CompareTo(huidigeDate) >= 0);
            vorige = sortedByDate.FindAll(r => r.Date.CompareTo(vorigeDate) >= 0 && r.Date.CompareTo(huidigeDate) < 0);

            //Toont de laatste en voorlaatste dag + de hoeveelheid records van een subscribed item waarvan er een record gevonden
            //Console.WriteLine(Aantal records: huidige.Count);
            //Console.WriteLine(Aantal records: vorige.Count);


            /* TODO:
             * Kijken of item persoon / Thema / Organisatie is
             *    ==> andere trend-detectiemethodes + alertteksten, ...
             * 
             **/
          }
        }
      });


      return newAlerts;
    }
  }
}
