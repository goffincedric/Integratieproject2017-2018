﻿using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    public class Trendspotter
    {

        public List<Alert> CheckTrendAverageRecords(Profile profile, IEnumerable<Record> records)
        {
            /*
            *!!
            *lastDate OVERAL AANPASSEN NAAR DATETIME.NOW WANNEER WE DE API KUNNEN AANSPREKEN
            * !!
            */
            double period = 14; //Aantal dagen vergelijken

            // Records ouder dan huidige dag
            DateTime lastDate = DateTime.Now;

            List<Record> oldRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date < lastDate.Date.AddDays(-1)).ToList();
            List<Record> newRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date <= lastDate.Date).ToList();

            //Alle recordpersonen die records hebben van de afgelopen 14 dagen toevoegen aan lijst
            List<Person> RecordPersons = GetPersons(profile.Subscriptions, newRecords);

            //Alle oldrecords van 1 persoon in een Dictionary met RecordPersoon als Key en de List van records als value
            Dictionary<Person, List<Record>> groupedOld = GroupRecordsPerPerson(RecordPersons, oldRecords);

            //Alle newrecords van 1 persoon in een Dictionary met RecordPersoon als Key en de List van records als value
            Dictionary<Person, List<Record>> groupedNew = GroupRecordsPerPerson(RecordPersons, newRecords);

            //De List van records opdelen in Dictionary van List<Record> per DateTime van de Record
            Console.WriteLine("=============OLD=============");
            Dictionary<Person, Dictionary<DateTime, List<Record>>> groupedDateOld = GetGroupedByDate(groupedOld);
            Dictionary<Person, double> oldGemiddelde = GetAverageTweets(groupedOld, period - 1);


            Console.WriteLine("=============NEW=============");
            Dictionary<Person, Dictionary<DateTime, List<Record>>> groupedDatenew = GetGroupedByDate(groupedNew);
            Dictionary<Person, double> newGemiddelde = GetAverageTweets(groupedNew, period);

            //De verschillen tonen in console
            Console.WriteLine("===========VERSCHIL===========");
            oldGemiddelde.Keys.ToList().ForEach(k =>
            {
                newGemiddelde.TryGetValue(k, out double nieuw);
                oldGemiddelde.TryGetValue(k, out double oud);
                Console.WriteLine(k + " = " + (nieuw - oud));
            });

            //Alerts maken
            List<Alert> alerts = new List<Alert>();
            oldGemiddelde.Keys.ToList().ForEach(k =>
            {
                newGemiddelde.TryGetValue(k, out double nieuw);
                oldGemiddelde.TryGetValue(k, out double oud);

                double verschil = nieuw - oud;

                if (verschil == 0) return;

                if (verschil <= -0.25)
                {
                    alerts.Add(new Alert()
                    {
                        Description = "Daling populariteit " + k.Name,
                        Text = k.Name + " is minder populair vergeleken met de laatste 2 weken",
                        IsRead = false,
                        TimeStamp = DateTime.Now,
                        Item = k,
                        Profile = profile
                    });
                }
                else if (verschil >= 0.25)
                {
                    alerts.Add(new Alert()
                    {
                        Description = "Stijging populariteit " + k.Name,
                        Text = k.Name + " heeft meer populariteit gekregen vergeleken met de laatste 2 weken",
                        IsRead = false,
                        TimeStamp = DateTime.Now,
                        Item = k,
                        Profile = profile
                    });
                }
            });


            Console.WriteLine("========= NIEUWE ALERTS ========");
            List<Alert> newAlerts = new List<Alert>();
            alerts.ForEach(a =>
            {
                if (profile.Alerts.FirstOrDefault(pa => pa.TimeStamp.Date.Equals(a.TimeStamp.Date) && pa.Text.Equals(a.Text)) == null)
                {
                    Console.WriteLine(a);
                    profile.Alerts.Add(a);
                    newAlerts.Add(a);
                }
            });

            return newAlerts;
        }

        private List<Person> GetPersons(List<Item> subscriptions, List<Record> records)
        {
            List<Person> RecordPersons = new List<Person>();
            records.ToList().ForEach(r =>
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

        private Dictionary<Person, List<Record>> GroupRecordsPerPerson(List<Person> persons, List<Record> periodRecords)
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

                AverageTweets.Add(rp, GetAverage(valueDict, period));
                Console.WriteLine(rp.ToString() + " - " + GetAverage(valueDict, period)); // period -1 omdat periode is uitgezonderd vandaag
            });

            return AverageTweets;
        }

        private double GetAverage(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
        {
            if (recordsPerDate.Values.Count == 0) return 0;

            List<Double> aantal = new List<double>();
            recordsPerDate.Keys.ToList().ForEach(k =>
            {
                recordsPerDate.TryGetValue(k, out var records);
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
                UserId = profile.Id,

                IsRead = false,
                TimeStamp = DateTime.Now
            };
        }
    }
}
