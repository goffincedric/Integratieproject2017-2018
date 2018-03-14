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
<<<<<<< HEAD
  public class Trendspotter
  {

        Dictionary<RecordPerson, double> OldCount = new Dictionary<RecordPerson, double>();
        Dictionary<RecordPerson, int> NewCount = new Dictionary<RecordPerson, int>();
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
                average = (OldRecords.Count(r => r.RecordPerson.LastName == TestRecord.RecordPerson.LastName)+ 1) / 13;

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
=======
    public class Trendspotter
    {

        public void VoorspelAantal(IEnumerable<Record> records, DateTime start, DateTime end)
        {

        }
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


>>>>>>> b6c174ef258150646d596be61bd3069333d3431c

        public void CheckTrend(DateTime date, IEnumerable<Record> records)
        {
            List<Record> OldRecords = new List<Record>();
            List<Record> NewRecords = new List<Record>();

            NewRecords = records.Where(r => r.ListUpdatet.Date.Equals(DateTime.Now.Date)).ToList();
            // Records ouder dan huidige dag
            OldRecords = records.Where(r => r.ListUpdatet < DateTime.Now).ToList();

            foreach (Record r in NewRecords)
            {
                Console.WriteLine(r.ToString());
            }

            Dictionary<RecordPerson, double> OldCount = new Dictionary<RecordPerson, double>();
            Dictionary<RecordPerson, int> NewCount = new Dictionary<RecordPerson, int>();


        }

        public void generateAlert(Profile profile, string Trend)
        { Alert a = new Alert(1, "Something has happened", "A change is coming", profile.Username); }
    }
}
