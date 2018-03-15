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



    public void CheckTrend(IEnumerable<Record> records)
    {
      /*
       *!!
       *lastDate OVERAL AANPASSEN NAAr DATETIME.NOW WANNEER WE DE API KUNNEN AANSPREKEN
       * !!
       */
      double period = 14; //Aantal dagen vergelijken

      // Records ouder dan huidige dag
      DateTime lastDate = records.ToList().OrderByDescending(r => r.Date).ToList()[0].Date;

      List<Record> oldRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date < lastDate.Date).ToList();
      //DateTime minDate = oldRecords.ToList().OrderByDescending(r => r.Date).ToList()[oldRecords.Count - 1].Date; //Datum van oudste record
      List<Record> newRecords = records.Where(r => r.Date.Date.Equals(lastDate.Date)).ToList();

      newRecords.ForEach(r => Console.WriteLine(r));
      List<RecordPerson> recordPeople = new List<RecordPerson>();
      records.ToList().ForEach(r =>
      {
        if (!recordPeople.Contains(r.RecordPerson)) recordPeople.Add(r.RecordPerson);
      });

      //Alle records van 1 persoon in een Dictionary met RecordPersoon als Key en de List van records als value
      Dictionary<RecordPerson, List<Record>> grouped = new Dictionary<RecordPerson, List<Record>>();
      recordPeople.ForEach(rp =>
      {
        if (oldRecords.Where(r => r.RecordPerson.Equals(rp)).ToList().Count != 0)
        {
          if (!grouped.Keys.Contains(rp))
          {
            grouped.Add(rp, oldRecords.Where(r => r.RecordPerson.Equals(rp)).ToList());
          }
        }
      });

      //De List van records opdelen in Dictionary van List<Record> per dag
      Dictionary<RecordPerson, Dictionary<DateTime, List<Record>>> groupedDate = new Dictionary<RecordPerson, Dictionary<DateTime, List<Record>>>();
      grouped.Keys.ToList().ForEach(rp =>
      {
        if (!groupedDate.Keys.Contains(rp))
        {
          List<Record> rpRecords;
          grouped.TryGetValue(rp, out rpRecords);
          Dictionary<DateTime, List<Record>> valueDict = new Dictionary<DateTime, List<Record>>();
          rpRecords.Select(r => r.Date.Date).Distinct().ToList().ForEach(d => valueDict.Add(d, rpRecords.Where(r => r.Date.Date.Equals(d)).ToList()));
          
          groupedDate.Add(rp, valueDict);
          Console.WriteLine(rp.ToString() + " - " + getAverage(valueDict, period-1)); // period -1 omdat periode uitgezonder vandaag
        }
      });


      //TODO: GEMMIDDELDE BEREKENEN HUIDIGE DAG (NEWRECORDS)


      //WAT RETURNEN?
    }

    private double getAverage(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
    {
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
