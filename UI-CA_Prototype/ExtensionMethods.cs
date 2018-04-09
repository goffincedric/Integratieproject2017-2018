using Newtonsoft.Json;
using PB.BL.Domain.Account;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;
using PB.BL.Domain.Platform;

namespace UI_CA_Prototype
{
    public class ExtensionMethods
    {
        //Lets the user select an available profile
        public Profile SelectProfile(IEnumerable<Profile> profiles)
        {
            int keuze = 0;
            do
            {
                ShowProfiles(profiles);
                Console.Write("Keuze: ");
                int.TryParse(Console.ReadLine(), out keuze);
            } while (keuze < 1 || keuze > profiles.ToList().Count);

            return profiles.ElementAt(keuze - 1);
        }

        //Shows a list of all available profiles
        public void ShowProfiles(IEnumerable<Profile> profiles)
        {
            if (profiles.ToList().Count == 0) throw new Exception("Er zijn nog geen accounts, maak er eerst een aan");
            Console.WriteLine("\nAccounts: ");
            int counter = 1;
            profiles.ToList().ForEach(p =>
            {
                Console.WriteLine(counter + ") " + p);
                counter++;
            });
        }

        //Lets the user select an available subplatform
        public Subplatform SelectSubplatform(IEnumerable<Subplatform> subplatforms)
        {
            int keuze = 0;
            do
            {
                ShowSubplatforms(subplatforms);
                Console.Write("Keuze: ");
                int.TryParse(Console.ReadLine(), out keuze);
            } while (keuze < 1 || keuze > subplatforms.ToList().Count);

            return subplatforms.ElementAt(keuze - 1);
        }

        //Shows a list of all available profiles
        public void ShowSubplatforms(IEnumerable<Subplatform> subplatforms)
        {
            if (subplatforms.ToList().Count == 0) throw new Exception("Er zijn nog geen subplatforms, maak er eerst een aan");
            Console.WriteLine("\nSubplatforms: ");
            int counter = 1;
            subplatforms.ToList().ForEach(p =>
            {
                Console.WriteLine(counter + ") " + p);
                counter++;
            });
        }


        //Show all records sorted by Name, then by Date descending
        public void ShowRecords(IEnumerable<Record> records)
        {
            records.OrderBy(r => r.Tweet_Id).ThenByDescending(r => r.Date).ToList().ForEach(r =>
            {
                Console.WriteLine(r);
            });
        }

        //Lets the user select an available item
        public Item SelectItem(IEnumerable<Item> persons)
        {
            int keuze = 0;

            do
            {
                ShowPersons(persons);
                Console.Write("Keuze: ");
                int.TryParse(Console.ReadLine(), out keuze);
            } while (keuze < 1 || keuze > persons.ToList().Count);

            return persons.ElementAt(keuze - 1);
        }

        //Shows all items
        public void ShowPersons(IEnumerable<Item> persons)
        {
            int counter = 1;
            Console.WriteLine("\nAlle Persons:");
            persons.ToList().ForEach(p =>
            {
                Console.WriteLine(counter + ") " + p);
                counter++;
            });
        }

        //Shows all subscribed items from profile
        public void ShowSubScribedItems(Profile profile)
        {
            if (profile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
            Console.WriteLine("Subscribed items:");
            profile.Subscriptions.ForEach(subs => Console.WriteLine(subs));
        }


        public Profile CreateAccount()
        {
            string AccountName = "";
            string email = "";
            string password = "";
            Console.Write("Accountname: ");
            AccountName = Console.ReadLine();
            Console.Write("Email: ");
            email = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();

            Profile newProfile = new Profile()
            {
                Username = AccountName,
                Email = email,
                Password = password
            };

            return newProfile;
        }

        //Method to test write functionality of JsonConvert (read written json file on desktop for record-object structure)
        public void WriteTestRecords()
        {
            List<Record> records = new List<Record>();
            records.Add(new Record()
            {
                Hashtags = new List<Hashtag>(),
                Words = new List<Word>() {
                    new Word("annouri"),
                    new Word("kasper goethals"),
                    new Word("arabië"),
                    new Word("imade"),
                    new Word("iran")
                },
                Date = DateTime.Parse("2017-09-11 04:53:38"),
                Persons = new List<Person>() {
                    new Person() { Name = "Imade Annouri" },
                    new Person() { Name = "Annick De Ridder"},
                },
                Longitude = 4.399708,
                Latitude = 51.22111,
                Tweet_Id = 907104827896987600,
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
                Words = new List<Word>()
                {
                    new Word("annouri"),
                    new Word("imade"),
                    new Word("reeks"),
                    new Word("time")
                },
                Date = DateTime.Parse("2017-09-07 22:52:35"),
                Persons = new List<Person>() {
                    new Person() { Name = "Bart De Wever" }
                },
                Longitude = 6.46744,
                Latitude = 52.81776,
                Tweet_Id = 905926801804980200,
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
