using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Account;
using Domain.Items;
using PB.BL.Domain.Account;

namespace PB.BL
{
    public class ItemManager : IItemManager
    {
        private IItemRepo ItemRepo;
        private IRecordRepo RecordRepo;

        private UnitOfWorkManager uowManager;

        private Trendspotter trendspotter = new Trendspotter();

        public ItemManager()
        {

        }

        public ItemManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            ItemRepo = new ItemRepo(uowMgr.UnitOfWork);
            RecordRepo = new RecordRepo(uowMgr.UnitOfWork);
        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (RecordRepo == null || ItemRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                        Console.WriteLine("UOW MADE IN ITEM MANAGER for Record and Item repo");
                    }
                    else
                    {
                        Console.WriteLine("uo bestaat al");
                    }

                    RecordRepo = new RecordRepo(uowManager.UnitOfWork);
                    ItemRepo = new ItemRepo(uowManager.UnitOfWork);
                }
                else
                {
                    RecordRepo = new RecordRepo();
                    ItemRepo = new ItemRepo();
                    Console.WriteLine("OLD WAY REPO ITEMMGR");
                }
            }
        }



        #region Items
        public Organisation AddOrganisation(string name, string socialMediaLink = null, string iconURL = null)
        {
            initNonExistingRepo();
            // initNonExistingRepoItem();

            Organisation organisation = new Organisation()
            {
                Name = name,
                SocialMediaLink = socialMediaLink,
                IconURL = iconURL,
                Keywords = new List<Keyword>(),
                SubPlatforms = new List<Subplatform>(),
                Records = new List<Record>(),
                People = new List<Person>()
            };

            organisation = (Organisation)AddItem(organisation);
            uowManager.Save();

            return organisation;
        }

        public Person AddPerson(string firstName, string lastName, DateTime birthDay, string socialMediaLink, string iconURL, Organisation organisation = null, Function function = null)
        {
            initNonExistingRepo();
            Person person = new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                SocialMediaLink = socialMediaLink,
                IconURL = iconURL,
                Function = function,
                Keywords = new List<Keyword>(),
                SubPlatforms = new List<Subplatform>(),
                Records = new List<Record>(),
                Organisation = organisation
            };
            person = (Person)AddItem(person);
            uowManager.Save();
            return person;
        }

        public Theme AddTheme(string themeName, string description)
        {
            initNonExistingRepo();
            Theme theme = new Theme()
            {
                ThemeName = themeName,
                Description = description,
                Keywords = new List<Keyword>(),
                SubPlatforms = new List<Subplatform>(),
                Records = new List<Record>()
            };

            theme = (Theme)AddItem(theme);
            uowManager.Save();
            return theme;
        }

        private Item AddItem(Item item)
        {
            initNonExistingRepo();
            return ItemRepo.CreateItem(item);
        }

        public void ChangeItem(Item item)
        {
            initNonExistingRepo();
            ItemRepo.UpdateItem(item);
            uowManager.Save();
        }

        public Item GetItem(int itemId)
        {
            initNonExistingRepo();
            return ItemRepo.ReadItem(itemId);
        }

        public IEnumerable<Item> GetItems()
        {
            initNonExistingRepo();
            return ItemRepo.ReadItems();
        }

        public Organisation GetOrganistation(int itemId)
        {
            initNonExistingRepo();
            return (Organisation)ItemRepo.ReadItem(itemId);
        }

        public Person GetPerson(int itemId)
        {
            initNonExistingRepo();
            return (Person)ItemRepo.ReadItem(itemId);
        }

        public Theme GetTheme(int itemId)
        {
            initNonExistingRepo();
            return (Theme)ItemRepo.ReadItem(itemId);
        }

        public void RemoveItem(int itemId)
        {
            initNonExistingRepo();
            ItemRepo.DeleteItem(itemId);
            uowManager.Save();
        }


        public IEnumerable<Person> GetPersons()
        {
            initNonExistingRepo();
            return ItemRepo.ReadPersons();
        }

        #endregion


        #region Records
        public IEnumerable<Record> GetRecords()
        {
            initNonExistingRepo();
            return RecordRepo.ReadRecords();
        }

        public Record GetRecord(long id)
        {
            initNonExistingRepo();
            return RecordRepo.ReadRecord(id);
        }

        public Record AddRecord(string source, long Tweet_Id, string user_Id, List<Mention> mentions, DateTime date, string geo, RecordPerson recordPerson,
          bool retweet, List<Word> words, Sentiment sentiment, List<Hashtag> hashtags, List<Url> uRLs)
        {
            initNonExistingRepo();
            Record record = new Record()
            {
                Source = source,
                Tweet_Id = Tweet_Id,
                User_Id = user_Id,
                Mentions = mentions,
                Date = date,
                Geo = geo,
                RecordPerson = recordPerson,
                Retweet = retweet,
                Words = words,
                Sentiment = sentiment,
                Hashtags = hashtags,
                URLs = uRLs
            };
            record = AddRecord(record);
            uowManager.Save();
            return record;
        }

        private Record AddRecord(Record record)
        {
            initNonExistingRepo();
            return RecordRepo.CreateRecord(record);
        }

        public void ChangeRecord(Record record)
        {
            initNonExistingRepo();
            RecordRepo.UpdateRecord(record);
            uowManager.Save();
        }

        public void RemoveRecord(long id)
        {
            initNonExistingRepo();
            RecordRepo.DeleteRecord(id);
            uowManager.Save();
        }
        #endregion


        public void Seed(bool even = true)
        {
            initNonExistingRepo();
            List<Record> toegevoegde = RecordRepo.Seed(even);
            uowManager.Save();
            RecordsToItems(toegevoegde);
            uowManager.Save();
        }

        public List<Alert> GenerateProfileAlerts(Profile profile)
        {
            //Alle items uit profile subscriptions halen
            if (profile == null) throw new Exception("U heeft nog geen account geselecteerd, gelieve er eerst een te kiezen");
            List<Item> subscribedItems = profile.Subscriptions;

            //Items opdelen in Subklasses [Person, Organisation, Theme]
            List<Person> people = subscribedItems.Where(i => i is Person).ToList().Select(i => (Person)i).ToList();
            List<Organisation> organisations = new List<Organisation>(); // Alerts op organisaties;
            List<Theme> themes = new List<Theme>(); // Alerts op thema's

            //Records uit people halen
            List<Record> peopleRecords = new List<Record>();
            people.ForEach(p => p.Records.ForEach(r => peopleRecords.Add(r)));

            //Print all subscribed items
            Console.WriteLine("========= SUBSCRIBED =========");
            subscribedItems.ForEach(i => Console.WriteLine(i));

            //Check trends voor people
            List<Alert> alerts = trendspotter.CheckTrendAverageRecords(peopleRecords);
            Console.WriteLine("============ ALERTS ===========");
            //Link profile to alerts
            alerts.ForEach(a =>
            {
                Console.WriteLine(a);
                a.Profile = profile;
                a.Username = profile.Username;
            });

            alerts.ForEach(a =>
            {
                if (!profile.Alerts.Contains(a)) profile.Alerts.Add(a);
            });

            uowManager.Save();

            //Return alerts
            return alerts;
        }

        public void CheckTrend()
        {
            initNonExistingRepo();
            //initNonExistingRepoRecord();
            trendspotter.CheckTrendAverageRecords(GetRecords());
        }

        private void RecordsToItems(List<Record> records)
        {
            initNonExistingRepo();
            //initNonExistingRepoRecord();
            //initNonExistingRepoItem();
            //List<Record> records = GetRecords().ToList();
            List<Person> persons = GetPersons().ToList();
            List<Person> people = new List<Person>();
            Item item;

            records.ToList().ForEach(r =>
            {
                item = persons.FirstOrDefault(p => p.FirstName.Equals(r.RecordPerson.FirstName) && p.LastName.Equals(r.RecordPerson.LastName));
                if (item == null)
                {
                    Person person = new Person()
                    {
                        ItemId = persons.Count,
                        FirstName = r.RecordPerson.FirstName,
                        LastName = r.RecordPerson.LastName,
                        Keywords = r.Words.ConvertAll(w => new Keyword() { Name = w.Text }),
                        SubPlatforms = new List<Subplatform>(),
                        Records = new List<Record>() { r }
                    };
                    persons.Add(person);
                    people.Add(person);
                }
                else
                {
                    item.Records.Add(r);
                }
            });

            people.ForEach(p => ItemRepo.CreatePerson(p));
            uowManager.Save();
        }
    }
}