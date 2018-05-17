using Newtonsoft.Json;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            records.OrderBy(r => r.Date).ThenBy(r => r.Persons[0].Name).ThenBy(r => r.Tweet_Id).ToList().ForEach(Console.WriteLine);
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
            profile.Subscriptions.ForEach(Console.WriteLine);
        }

        //Method to test write functionality of JsonConvert (read written json file on desktop for record-object structure)
        public void WriteTestRecords(IEnumerable<Record> records)
        {
            List<Record> recordList = records.Where(r => r.Tweet_Id % 2 == 0).ToList();

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + @"\structure.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, recordList);
            }

            Console.WriteLine("Er werd een testbestand genaamd 'structure.json' weggeschreven naar uw desktop met max 200 test-records");
        }
    }
}
