using Domain.Items;
using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class Trendspotter
  {

    //generateAlerts moeten net andersom, De user moet een methode kunnen oproepen die alerts genereert (want is customizable per user) 
    //bv. user1.generateAlerts()
    //dus frequentie bij houden en for each op users en daarop oproepen
    //wat is dictionary (soort van collection)

    /* Oke wat ik in gedachten had
 Dus ik heb een TrendSpotter klasse aangemaakt in de BL, 
 ik zou deze klasse willen oproepen telkens als er records
 worden bijgemaakt(dus rijen bijkomen in de db).
 In de Trendspotter klasse gebeurd alles rond het zoeken 
 van trends voor een bepaalde user, met zelfs de keuze 
 voor het aantal items dat je wilt vergelijken voor een alert
 nu doe jij in je generate alerts
 en dan doe jij een for loop over de users die subscriptions hebben
 maar dat is fout want een user kan subscriptions hebben maar in zijn settings het krijgen van meldingen en emails uitgezetten hebben 
 daarom zou ik liever de trendspotter klasse ook alle users willen laten op halen die meldingen aan hebben en subscriptions hebben, 
 dan gaan we per users per subscription bepalen 
 of er een trend is en hier voor een melding sturen en dan naar de volgende user, dan gebeeurt er bijvoorbeeld user1.getAlerts() return List() en nadien alle alerts pushen
 maar als we dan een trendspotter 
 klasse aanmaken kunnen we mss meegeven Trendspotter spotter = new Spotter(startDate, EndDate) 
 om te bepalen tussen welke data de trends berekent moeten worden*/



    public List<Alert> CheckTrendAverageRecords(IEnumerable<Record> records)
    {
      /*
       *!!
       *lastDate OVERAL AANPASSEN NAAR DATETIME.NOW WANNEER WE DE API KUNNEN AANSPREKEN
       * !!
       */
      double period = 14; //Aantal dagen vergelijken

      // Records ouder dan huidige dag
      DateTime lastDate = records.ToList().OrderByDescending(r => r.Date).ToList()[0].Date;

      List<Record> oldRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date < lastDate.Date.AddDays(-1)).ToList();
      //DateTime minDate = oldRecords.ToList().OrderByDescending(r => r.Date).ToList()[oldRecords.Count - 1].Date; //Datum van oudste record
      List<Record> newRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date <= lastDate.Date).ToList();

      //Console.WriteLine(records.ToList()[0].Date.Date <= lastDate.Date);
      //Console.WriteLine(lastDate + " " + lastDate.Date.AddDays(-1));
      //Console.WriteLine(oldRecords.Count + " " + newRecords.Count);


      //Alle NewRecords afdrukken
      //newRecords.ForEach(r => Console.WriteLine(r));

      //Alle recordpersonen die records hebben van de afgelopen 14 dagen toevoegen aan lijst
      List<RecordPerson> recordPeople = new List<RecordPerson>();
      newRecords.ToList().ForEach(r =>
      {
        if (!recordPeople.Contains(r.RecordPerson))
        {
          recordPeople.Add(r.RecordPerson);
        }
      });

      //Alle oldrecords van 1 persoon in een Dictionary met RecordPersoon als Key en de List van records als value
      Dictionary<RecordPerson, List<Record>> groupedOld = groupRecordsPerPerson(recordPeople, oldRecords);

      Console.WriteLine("=============OLD=============");
      //De List van records opdelen in Dictionary van List<Record> per dag
      Dictionary<RecordPerson, Dictionary<DateTime, List<Record>>> groupedDateOld = new Dictionary<RecordPerson, Dictionary<DateTime, List<Record>>>();
      Dictionary<RecordPerson, double> oldGemiddelde = new Dictionary<RecordPerson, double>();

      groupedOld.Keys.ToList().ForEach(rp =>
      {
        if (!groupedDateOld.Keys.Contains(rp))
        {
          List<Record> rpRecords;
          groupedOld.TryGetValue(rp, out rpRecords);
          Dictionary<DateTime, List<Record>> valueDict = new Dictionary<DateTime, List<Record>>();
          if (rpRecords.Count != 0)
          {
            rpRecords.Select(r => r.Date.Date).Distinct().ToList().ForEach(d => valueDict.Add(d, rpRecords.Where(r => r.Date.Date.Equals(d)).ToList()));
          }

          groupedDateOld.Add(rp, valueDict);

          oldGemiddelde.Add(rp, GetAverage(valueDict, period - 1));
          Console.WriteLine(rp.ToString() + " - " + GetAverage(valueDict, period - 1)); // period -1 omdat periode uitgezonder vandaag
        }
      });



      Console.WriteLine("=============NEW=============");
      //TODO: GEMMIDDELDE BEREKENEN HUIDIGE DAG (NEWRECORDS)
      //Alle newrecords van 1 persoon in een Dictionary met RecordPersoon als Key en de List van records als value
      Dictionary<RecordPerson, List<Record>> groupedNew = groupRecordsPerPerson(recordPeople, newRecords);

      //De List van records opdelen in Dictionary van List<Record> per dag
      Dictionary<RecordPerson, Dictionary<DateTime, List<Record>>> groupedDatenew = new Dictionary<RecordPerson, Dictionary<DateTime, List<Record>>>();
      Dictionary<RecordPerson, double> newGemiddelde = new Dictionary<RecordPerson, double>();

      groupedNew.Keys.ToList().ForEach(rp =>
      {
        if (!groupedDatenew.Keys.Contains(rp))
        {
          List<Record> rpRecords;
          groupedNew.TryGetValue(rp, out rpRecords);
          Dictionary<DateTime, List<Record>> valueDict = new Dictionary<DateTime, List<Record>>();
          rpRecords.Select(r => r.Date.Date).Distinct().ToList().ForEach(d => valueDict.Add(d, rpRecords.Where(r => r.Date.Date.Equals(d)).ToList()));

          groupedDatenew.Add(rp, valueDict);

          newGemiddelde.Add(rp, GetAverage(valueDict, period - 1));
          Console.WriteLine(rp.ToString() + " - " + GetAverage(valueDict, period - 1)); // period -1 omdat periode is uitgezonderd vandaag
        }
      });

      Console.WriteLine("===========VERSCHIL===========");
      oldGemiddelde.Values.ToList().ForEach(v => Console.WriteLine(oldGemiddelde.Keys.ToList()[oldGemiddelde.Values.ToList().IndexOf(v)] + " = " + (newGemiddelde.Values.ToList()[oldGemiddelde.Values.ToList().IndexOf(v)] - v)));

      //WAT RETURNEN?
      Console.WriteLine("\noldRECORDPERSONSSSSSSSSSSSSSSSSSSSSSSS");
      oldGemiddelde.Keys.ToList().ForEach(v => Console.WriteLine(v));

      Console.WriteLine("\nnewRECORDPERSONSSSSSSSSSSSSSSSSSSSSSSS");
      newGemiddelde.Keys.ToList().ForEach(v => Console.WriteLine(v));

      //Alerts maken
      List<Alert> alerts = new List<Alert>();
      oldGemiddelde.Keys.ToList().ForEach(k =>
      {
        double verschil = 0;
        verschil = newGemiddelde.Values.ToList()[oldGemiddelde.Keys.ToList().IndexOf(k)] - oldGemiddelde.Values.ToList()[oldGemiddelde.Keys.ToList().IndexOf(k)];

        if (verschil == 0) return;

        if (verschil <= -0.02)
        {
          alerts.Add(new Alert()
          {
            Description = "Daling populariteit " + k.FirstName + " " + k.LastName,
            Text = k.FirstName + " " + k.LastName + " is minder populair vergeleken met de laatste 2 weken", //Random tekstjes laten kiezen?
            IsRead = false,
            TimeStamp = DateTime.Now
          });
        }
        else if (verschil >= 0.02)
        {
          alerts.Add(new Alert()
          {
            Description = "Stijging populariteit " + k.FirstName + " " + k.LastName,
            Text = k.FirstName + " " + k.LastName + " heeft meer populariteit gekregen vergeleken met de laatste 2 weken", //Random tekstjes laten kiezen?
            IsRead = false,
            TimeStamp = DateTime.Now
          });
        }
      });
      
      //Return alerts
      return alerts;
    }

    public Dictionary<RecordPerson, List<Record>> groupRecordsPerPerson(List<RecordPerson> recordPeople, List<Record> periodRecords)
    {
      Dictionary<RecordPerson, List<Record>> groupedOld = new Dictionary<RecordPerson, List<Record>>();

      recordPeople.ForEach(rp =>
      {
        if (!groupedOld.Keys.Contains(rp))
        {
          List<Record> records = periodRecords.Where(r => r.RecordPerson.Equals(rp)).ToList();
          groupedOld.Add(rp, (records.Count != 0) ? records : new List<Record>());
        }
      });

      return groupedOld;
    }

    private double GetAverage(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
    {
      if (recordsPerDate.Values.Count == 0) return 0;

      List<Double> aantal = new List<double>();
      recordsPerDate.Keys.ToList().ForEach(k =>
      {
        List<Record> records;
        recordsPerDate.TryGetValue(k, out records);
        aantal.Add(records.Count);
      });
      return aantal.Average() / period * aantal.Count;
    }

    public void generateAlert(Profile profile, string Trend)
    {
      Alert a = new Alert()
      {
        AlertId = 1,
        Description = "Something has happened",
        Text = "A change is coming",
        Profile = profile,
        Username = profile.Username,
        IsRead = false,
        TimeStamp = DateTime.Now
      };


    }
  }
}
