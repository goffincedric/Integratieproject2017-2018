using Domain.Items;
using PB.BL.Domain.Account;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class Trendspotter
  {

    Dictionary<RecordPerson, double> OldCount = new Dictionary<RecordPerson, double>();
    Dictionary<RecordPerson, int> NewCount = new Dictionary<RecordPerson, int>();
    //generateAlerts moeten net andersom, De user moet een methode kunnen oproepen die alerts genereert (want is customizable per user) 
    //bv. user1.generateAlerts()
    //dus frequentie bij houden en for each op users en daarop oproepen
    //wat is dictionary (soort van collection)

    public void CheckTrend(DateTime date, IEnumerable<Record> records)
    {
      DateTime lastDate = records.ToList().OrderByDescending(r => r.Date).ToList()[0].Date;

      List<Record> OldRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-14).Date && r.Date.Date < lastDate.Date).ToList();
      List<Record> NewRecords = records.Where(r => r.Date.Date.Equals(lastDate.Date)).ToList();

      while (OldRecords.Count > 0)
      {
        Record TestRecord;
        TestRecord = OldRecords.First();

        double average;

        OldRecords.RemoveAt(0);

        //dumb but working average (needs better workout)
        average = (OldRecords.Count(r => r.RecordPerson.LastName == TestRecord.RecordPerson.LastName) + 1) / 13;

        OldRecords.RemoveAll(r => r.RecordPerson.LastName == TestRecord.RecordPerson.LastName);

        OldCount.Add(TestRecord.RecordPerson, average);
      }

      //Finished and working!!! (do not touch)
      while (NewRecords.Count > 0)
      {
        Record TestRecord;
        TestRecord = NewRecords.First();

        int count;

        NewRecords.RemoveAt(0);

        count = NewRecords.Count(r => r.RecordPerson.LastName == TestRecord.RecordPerson.LastName) + 1;

        NewRecords.RemoveAll(r => r.RecordPerson.LastName == TestRecord.RecordPerson.LastName);

        NewCount.Add(TestRecord.RecordPerson, count);
      }
      Console.WriteLine(NewCount.Count());
      Console.WriteLine(OldCount.Count());
    }

    public void generateAlert(Profile profile, string Trend)
    {

    }



  }
}
