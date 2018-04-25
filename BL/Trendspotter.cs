using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    public class Trendspotter
    {
        public List<Alert> GenerateAllAlertTypes(Profile profile, IEnumerable<Record> records)
        {
            List<Alert> AllAlerts = new List<Alert>();

            // Call methodes to generate all types of alerts
            AllAlerts.AddRange(GenerateAverageTweetsAlert(profile, records));
            AllAlerts.AddRange(GenerateSentimentAlerts(profile, records));

            // return list of all alerts
            return AllAlerts;
        }

        public List<Alert> GenerateAverageTweetsAlert(Profile profile, IEnumerable<Record> records)
        {
            double period = 14; //Aantal dagen vergelijken

            // Records ouder dan huidige dag
            DateTime lastDate = DateTime.Now;

            List<Record> oldRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date <= lastDate.Date.AddDays(-1)).ToList();
            List<Record> newRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date <= lastDate.Date).ToList();

            //Alle recordpersonen die records hebben van de afgelopen 14 dagen toevoegen aan lijst
            List<Person> RecordPersons = GetPersonsWithRecord(profile.Subscriptions, newRecords);

            //Alle records van 1 persoon in een Dictionary met RecordPersoon als Key en de List van records als value
            Dictionary<Person, List<Record>> groupedOld = GetGroupRecordsPerPerson(RecordPersons, oldRecords);
            Dictionary<Person, List<Record>> groupedNew = GetGroupRecordsPerPerson(RecordPersons, newRecords);

            //De List van records opdelen in Dictionary van List<Record> per DateTime van de Record
            Console.WriteLine("=============OLD=============");
            Dictionary<Person, double> oldGemiddelde = GetAverageTweets(groupedOld, period - 1);
            
            Console.WriteLine("=============NEW=============");
            Dictionary<Person, double> newGemiddelde = GetAverageTweets(groupedNew, period);

            //De verschillen tonen in console
            //Alerts maken
            Console.WriteLine("===========VERSCHIL===========");
            List<Alert> alerts = new List<Alert>();
            oldGemiddelde.Keys.ToList().ForEach(k =>
            {
                newGemiddelde.TryGetValue(k, out double nieuw);
                oldGemiddelde.TryGetValue(k, out double oud);

                double verschil = nieuw - oud;
                Console.WriteLine(k + " = " + verschil);

                if (verschil == 0) return;

                if (verschil <= -0.25)
                {
                    alerts.Add(new Alert()
                    {
                        Description = "Daling populariteit " + k.Name,
                        Text = k.Name + " is minder populair vergeleken met de laatste 2 weken",
                        Event = "daalt in",
                        Subject = "populariteit",
                        Item = k,
                        ProfileAlerts = new List<ProfileAlert>()
                    });
                }
                else if (verschil >= 0.25)
                {
                    alerts.Add(new Alert()
                    {
                        Description = "Stijging populariteit " + k.Name,
                        Text = k.Name + " heeft meer populariteit gekregen vergeleken met de laatste 2 weken",
                        Event = "stijgt in",
                        Subject = "populariteit",
                        Item = k,
                        ProfileAlerts = new List<ProfileAlert>()
                    });
                }
            });

            return alerts;
        }

        public List<Alert> GenerateSentimentAlerts(Profile profile, IEnumerable<Record> records)
        {
            double period = 5; //Aantal dagen vergelijken

            // Records ouder dan huidige dag
            DateTime LastDate = DateTime.Now;

            List<Record> PeriodRecords = records.Where(r => r.Date.Date >= LastDate.AddDays(-period-1).Date && r.Date.Date <= LastDate.Date).ToList();

            // alle personen met records
            List<Person> PersonsWithRecords = GetPersonsWithRecord(profile.Subscriptions, PeriodRecords);

            // alle records per persoon
            Dictionary<Person, List<Record>> PersonRecords = GetGroupRecordsPerPerson(PersonsWithRecords, PeriodRecords);

            // gemiddeld sentiment per persoon per datum
            Console.WriteLine("=============AVERAGE=============");
            Dictionary<Person, double> AveragePolarity = GetAverageTweetPolarity(PersonRecords, period-1);

            // verschil
            // alerts generaten
            Console.WriteLine("=========== NEW ALERTS ==========");
            List<Alert> alerts = new List<Alert>();
            AveragePolarity.Keys.ToList().ForEach(k =>
            {
                AveragePolarity.TryGetValue(k, out double average);
                
                Console.WriteLine(k + " = " + average);

                if (average == 0) return;

                if (average <= -0.35)
                {
                    alerts.Add(new Alert()
                    {
                        Description = "Negatieve reacties op " + k.Name,
                        Text = k.Name + " heeft gemiddeld meer negatieve reacties gekregen de laatste " + period + " dagen",
                        Event = "kreeg negatievere",
                        Subject = "reacties",
                        Item = k,
                        ProfileAlerts = new List<ProfileAlert>()
                    });
                }
                else if (average >= 0.35)
                {
                    alerts.Add(new Alert()
                    {
                        Description = "Positieve reacties op " + k.Name,
                        Text = k.Name + " heeft gemiddeld meer positieve reacties gekregen de laatste " + period + " dagen",
                        Event = "kreeg positievere",
                        Subject = "reacties",
                        Item = k,
                        ProfileAlerts = new List<ProfileAlert>()
                    });
                }
            });

            return alerts;
        }

        private List<Person> GetPersonsWithRecord(List<Item> subscriptions, List<Record> records)
        {
            List<Person> RecordPersons = new List<Person>();
            records.ForEach(r =>
            {
                foreach (Person person in r.Persons)
                {
                    if (!RecordPersons.Contains(person) && subscriptions.Contains(person))
                    {
                        RecordPersons.Add(person);
                    }
                }
            });
            return RecordPersons;
        }

        private Dictionary<Person, List<Record>> GetGroupRecordsPerPerson(List<Person> persons, List<Record> periodRecords)
        {
            Dictionary<Person, List<Record>> groupedOld = new Dictionary<Person, List<Record>>();

            persons.ForEach(rp =>
            {
                if (!groupedOld.Keys.Contains(rp))
                {
                    List<Record> records = periodRecords.Where(r => r.Persons.Contains(rp)).ToList();
                    groupedOld.Add(rp, (records.Count != 0) ? records : new List<Record>());
                }
            });

            return groupedOld;
        }

        private Dictionary<Person, Dictionary<DateTime, List<Record>>> GetGroupedByDate(Dictionary<Person, List<Record>> groupedPerson)
        {
            Dictionary<Person, Dictionary<DateTime, List<Record>>> GroupedDate = new Dictionary<Person, Dictionary<DateTime, List<Record>>>();

            groupedPerson.Keys.ToList().ForEach(rp =>
            {
                if (!GroupedDate.Keys.Contains(rp))
                {
                    Dictionary<DateTime, List<Record>> valueDict = new Dictionary<DateTime, List<Record>>();

                    groupedPerson.TryGetValue(rp, out var rpRecords);

                    if (rpRecords.Count != 0)
                    {
                        rpRecords.Select(r => r.Date.Date).Distinct().ToList().ForEach(d => valueDict.Add(d, rpRecords.Where(r => r.Date.Date.Equals(d)).ToList()));
                    }

                    GroupedDate.Add(rp, valueDict);
                }
            });

            return GroupedDate;
        }

        private Dictionary<Person, double> GetAverageTweets(Dictionary<Person, List<Record>> groupedPerson, double period)
        {
            Dictionary<Person, double> AverageTweets = new Dictionary<Person, double>();

            groupedPerson.Keys.ToList().ForEach(rp =>
            {
                Dictionary<DateTime, List<Record>> valueDict = new Dictionary<DateTime, List<Record>>();

                groupedPerson.TryGetValue(rp, out var rpRecords);

                if (rpRecords.Count != 0)
                {
                    rpRecords.Select(r => r.Date.Date).Distinct().ToList().ForEach(d => valueDict.Add(d, rpRecords.Where(r => r.Date.Date.Equals(d)).ToList()));
                }

                AverageTweets.Add(rp, CalcAverageTweets(valueDict, period));
                Console.WriteLine(rp.ToString() + " - " + CalcAverageTweets(valueDict, period)); // period -1 omdat periode is uitgezonderd vandaag
            });

            return AverageTweets;
        }

        private double CalcAverageTweets(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
        {
            if (recordsPerDate.Values.Count == 0) return 0;

            List<double> aantal = new List<double>();
            recordsPerDate.Keys.ToList().ForEach(k =>
            {
                recordsPerDate.TryGetValue(k, out var records);
                aantal.Add(records.Count);
            });
            return aantal.Average() / period * aantal.Count;
        }

        private Dictionary<Person, double> GetAverageTweetPolarity(Dictionary<Person, List<Record>> groupedPerson, double period)
        {
            Dictionary<Person, double> AverageTweets = new Dictionary<Person, double>();

            groupedPerson.Keys.ToList().ForEach(rp =>
            {
                Dictionary<DateTime, List<Record>> valueDict = new Dictionary<DateTime, List<Record>>();

                groupedPerson.TryGetValue(rp, out var rpRecords);

                if (rpRecords.Count != 0)
                {
                    rpRecords.Select(r => r.Date.Date).Distinct().ToList().ForEach(d => valueDict.Add(d, rpRecords.Where(r => r.Date.Date.Equals(d)).ToList()));
                }

                AverageTweets.Add(rp, CalcAverageTweetPolarity(valueDict, period));
                Console.WriteLine(rp.ToString() + " - " + CalcAverageTweetPolarity(valueDict, period)); // period -1 omdat periode is uitgezonderd vandaag
            });

            return AverageTweets;
        }

        private double CalcAverageTweetPolarity(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
        {
            if (recordsPerDate.Values.Count == 0) return 0;

            List<double> aantal = new List<double>();
            recordsPerDate.Keys.ToList().ForEach(k =>
            {
                recordsPerDate.TryGetValue(k, out var records);
                aantal.Add(records.Average(r => r.Sentiment.Objectivity * r.Sentiment.Polarity));
            });
            Console.WriteLine(aantal.Average());
            return aantal.Average();
        }
    }
}
