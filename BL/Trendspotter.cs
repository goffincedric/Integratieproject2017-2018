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

  



    public void CheckTrend(IEnumerable<Record> records)
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

          oldGemiddelde.Add(rp, getAverage(valueDict, period - 1));
          Console.WriteLine(rp.ToString() + " - " + getAverage(valueDict, period - 1)); // period -1 omdat periode uitgezonder vandaag
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

          newGemiddelde.Add(rp, getAverage(valueDict, period - 1));
          Console.WriteLine(rp.ToString() + " - " + getAverage(valueDict, period - 1)); // period -1 omdat periode uitgezonder vandaag
        }
      });


      Console.WriteLine("===========VERSCHIL===========");
      oldGemiddelde.Values.ToList().ForEach(v => Console.WriteLine(oldGemiddelde.Keys.ToList()[oldGemiddelde.Values.ToList().IndexOf(v)] + " = " + (newGemiddelde.Values.ToList()[oldGemiddelde.Values.ToList().IndexOf(v)] - v)));

      //WAT RETURNEN?

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

    private double getAverage(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
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
