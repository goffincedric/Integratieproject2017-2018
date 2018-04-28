using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    public static class Trendspotter
    {
        public static List<Alert> GenerateAllAlertTypes(List<Item> Subscriptions)
        {
            List<Alert> AllAlerts = new List<Alert>();

            // Items opdelen in Subklasses [Person, Organisation, Theme]
            List<Person> Persons = Subscriptions.Where(i => i is Person).Select(i => (Person)i).ToList();
            List<Organisation> Organisations = Subscriptions.Where(i => i is Organisation).Select(i => (Organisation)i).ToList(); // Alerts op organisaties;
            List<Theme> Themes = new List<Theme>(); // Alerts op thema's

            //Records uit people halen
            List<Record> PersonsWithRecords = new List<Record>();
            Persons.ForEach(p => p.Records.ForEach(r => PersonsWithRecords.Add(r)));

            // Call methodes to generate all types of alerts
            AllAlerts.AddRange(GenerateAverageTweetsAlert(Persons, PersonsWithRecords));
            AllAlerts.AddRange(GenerateSentimentAlerts(Persons, PersonsWithRecords));

            // return list of all alerts
            return AllAlerts;
        }

        public static List<Item> CheckTrendingItems(List<Item> items, int topAmount, ref List<Alert> alerts)
        {
            // Calc average subscribed profiles per person
            int averageSubscriptions = (int)Math.Round(items.Where(i => i is Person).Average(i => i.SubscribedProfiles.Count));

            // Get previous trending organisation, later needed
            Organisation oldTrendingOrganisation = (Organisation)items.FirstOrDefault(i => i is Organisation && i.IsTrending);

            // Reset trending scores and status
            items.ForEach(i =>
            {
                i.IsTrending = false;
                if (i is Person person)
                {
                    person.TrendingScore = 0;
                }
            });

            /* People */
            List<Person> hotPeople = new List<Person>();
            items.ForEach(i =>
            {
                if (i is Person person)
                {
                    person.TrendingScore = person.Records.Count * (person.SubscribedProfiles.Count * averageSubscriptions);
                    hotPeople.Add(person);
                }
            });
            hotPeople
                .OrderByDescending(p => p.TrendingScore)
                .Take(10)
                .ToList().ForEach(p =>
                {
                    p.IsTrending = true;
                    items[items.FindIndex(i => i.ItemId == p.ItemId)] = p;
                });

            /* Organisations */
            //Get amount of hot people per organisation
            Dictionary<Organisation, int> organisations = hotPeople
                .GroupBy(p => p.Organisation)
                .ToDictionary(kv => kv.Key, kv => kv.ToList().Count);
            // calculate max amount of hot people per organisation & filter
            int max = organisations.Values.Max();
            organisations = organisations.
                Where(kv => kv.Value == max)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            // Get organisation based on sum of trendingscore of hot people from organisation
            Organisation organisation = organisations.First().Key;
            if (organisations.Count > 1)
            {
                organisations.Keys.ToList().ForEach(o =>
                {
                    if (o.People.Sum(p => p.TrendingScore) >= organisation.People.Where(p => p.IsTrending).Sum(p => p.TrendingScore))
                    {
                        organisation = o;
                    }
                });
            }
            organisation.IsTrending = true;
            items[items.FindIndex(i => i.ItemId == organisation.ItemId)] = organisation;

            if (!organisation.Equals(oldTrendingOrganisation))
            {
                Alert alert = new Alert()
                {
                    Description = organisation + " is de top trending organisatie",
                    Text = organisation.Name + " is de nieuwe top trending organisatie" + ((oldTrendingOrganisation != null) ? ". Vorige top trending organisatie: " + oldTrendingOrganisation.Name : ""),
                    Event = "is top trending",
                    Subject = "organisatie",
                    Item = organisation,
                    ProfileAlerts = new List<ProfileAlert>()
                };
                organisation.Alerts.Add(alert);

                alerts.Add(alert);
            }

            /* Themes */
            // TODO


            // TODO: Zie of trending organisatie veranderd is, ja => alert maken voor alle profiles die daarop subscribed zijn
            return items;
        }

        public static List<Alert> GenerateAverageTweetsAlert(List<Person> persons, IEnumerable<Record> records)
        {
            double period = 14; //Aantal dagen vergelijken

            // Records ouder dan huidige dag
            DateTime lastDate = DateTime.Now;

            List<Record> oldRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date <= lastDate.Date.AddDays(-1)).ToList();
            List<Record> newRecords = records.Where(r => r.Date.Date >= lastDate.AddDays(-period).Date && r.Date.Date <= lastDate.Date).ToList();

            //Alle recordpersonen die records hebben van de afgelopen 14 dagen toevoegen aan lijst
            List<Person> RecordPersons = GetPersonsWithRecord(persons, newRecords);

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

        public static List<Alert> GenerateSentimentAlerts(List<Person> persons, IEnumerable<Record> records)
        {
            double period = 5; //Aantal dagen vergelijken

            // Records ouder dan huidige dag
            DateTime LastDate = DateTime.Now;

            List<Record> PeriodRecords = records.Where(r => r.Date.Date >= LastDate.AddDays(-period - 1).Date && r.Date.Date <= LastDate.Date).ToList();

            // alle personen met records
            List<Person> PersonsWithRecords = GetPersonsWithRecord(persons, PeriodRecords);

            // alle records per persoon
            Dictionary<Person, List<Record>> PersonRecords = GetGroupRecordsPerPerson(PersonsWithRecords, PeriodRecords);

            // gemiddeld sentiment per persoon per datum
            Console.WriteLine("=============AVERAGE=============");
            Dictionary<Person, double> AveragePolarity = GetAverageTweetPolarity(PersonRecords, period - 1);

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

        private static List<Person> GetPersonsWithRecord(List<Person> subscriptions, List<Record> records)
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

        private static Dictionary<Person, List<Record>> GetGroupRecordsPerPerson(List<Person> persons, List<Record> periodRecords)
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

        private static Dictionary<Person, Dictionary<DateTime, List<Record>>> GetGroupedByDate(Dictionary<Person, List<Record>> groupedPerson)
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

        private static Dictionary<Person, double> GetAverageTweets(Dictionary<Person, List<Record>> groupedPerson, double period)
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

        private static double CalcAverageTweets(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
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

        private static Dictionary<Person, double> GetAverageTweetPolarity(Dictionary<Person, List<Record>> groupedPerson, double period)
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

        private static double CalcAverageTweetPolarity(Dictionary<DateTime, List<Record>> recordsPerDate, double period)
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
