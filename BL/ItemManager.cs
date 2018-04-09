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
using Domain.JSONConversion;
using PB.BL.Domain.Dashboards;

namespace PB.BL
{
    public class ItemManager : IItemManager
    {
        private IItemRepo ItemRepo;
        private IRecordRepo RecordRepo;

        private UnitOfWorkManager UowManager;

        private Trendspotter trendspotter = new Trendspotter();

        public ItemManager()
        {

        }

        public ItemManager(UnitOfWorkManager uowMgr)
        {
            UowManager = uowMgr;
            ItemRepo = new ItemRepo(uowMgr.UnitOfWork);
            RecordRepo = new RecordRepo(uowMgr.UnitOfWork);
        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (RecordRepo == null || ItemRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (UowManager == null)
                    {
                        UowManager = new UnitOfWorkManager();
                        Console.WriteLine("UOW MADE IN ITEM MANAGER for Record and Item repo");
                    }
                    else
                    {
                        Console.WriteLine("uo bestaat al");
                    }

                    RecordRepo = new RecordRepo(UowManager.UnitOfWork);
                    ItemRepo = new ItemRepo(UowManager.UnitOfWork);
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
            UowManager.Save();

            return organisation;
        }

        public Person AddPerson(string name, DateTime birthDay, string socialMediaLink, string iconURL, Organisation organisation = null, Function function = null)
        {
            initNonExistingRepo();
            Person person = new Person()
            {
                Name = name,
                SocialMediaLink = socialMediaLink,
                IconURL = iconURL,
                Function = function,
                Keywords = new List<Keyword>(),
                SubPlatforms = new List<Subplatform>(),
                Records = new List<Record>(),
                Organisation = organisation
            };
            person = (Person)AddItem(person);
            UowManager.Save();
            return person;
        }

        public Theme AddTheme(string name, string description)
        {
            initNonExistingRepo();
            Theme theme = new Theme()
            {
                Name = name,
                Description = description,
                Keywords = new List<Keyword>(),
                SubPlatforms = new List<Subplatform>(),
                Records = new List<Record>()
            };

            theme = (Theme)AddItem(theme);
            UowManager.Save();
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
            UowManager.Save();
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
            UowManager.Save();
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

        public Record AddRecord(long tweet_Id, RecordProfile recordProfile, List<Word> words, Sentiment sentiment, string source, List<Hashtag> hashtags, List<Mention> mentions, List<Url> uRLs, List<Theme> themes, List<Person> persons, DateTime date, double longitude, double latitude, bool retweet)
        {
            initNonExistingRepo();
            Record record = new Record()
            {
                Tweet_Id = tweet_Id,
                RecordProfile = recordProfile,
                Words = words,
                Sentiment = sentiment,
                Source = source,
                Hashtags = hashtags,
                Mentions = mentions,
                URLs = uRLs,
                Themes = themes,
                Persons = persons,
                Date = date,
                Longitude = longitude,
                Latitude = latitude,
                Retweet = retweet,
                ListUpdatet = DateTime.Now
            };

            record = AddRecord(record);
            UowManager.Save();
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
            UowManager.Save();
        }

        public void RemoveRecord(long id)
        {
            initNonExistingRepo();
            RecordRepo.DeleteRecord(id);
            UowManager.Save();
        }
        #endregion


        public void Seed(bool evenRecords = true)
        {
            initNonExistingRepo();
            List<Record> toegevoegde = JClassToRecord(RecordRepo.Seed(evenRecords));
        }

        public List<Record> JClassToRecord(List<JClass> data)
        {
            initNonExistingRepo();
            List<Mention> allMentions = RecordRepo.ReadMentions().ToList();
            List<Word> allWords = RecordRepo.ReadWords().ToList();
            List<Hashtag> allHashtags = RecordRepo.ReadHashTags().ToList();
            List<Url> allUrls = RecordRepo.ReadUrls().ToList();

            List<Record> oldRecords = RecordRepo.ReadRecords().ToList();
            List<Record> newRecords = new List<Record>();
            List<Person> oldPersons = ItemRepo.ReadPersons().ToList();
            List<Person> newPersons = new List<Person>();

            foreach (var el in data)
            {
                if (oldRecords.FirstOrDefault(r => r.Tweet_Id == el.Id) == null)
                {
                    Record record = new Record()
                    {
                        Tweet_Id = el.Id,
                        RecordProfile = el.Profile,
                        Words = new List<Word>(),
                        Sentiment = new Sentiment(el.Sentiment[0], el.Sentiment[1]),
                        Source = el.Source,
                        Hashtags = new List<Hashtag>(),
                        Mentions = new List<Mention>(),
                        URLs = new List<Url>(),
                        Themes = new List<Theme>(),
                        Persons = new List<Person>(),
                        Date = el.Date,
                        Retweet = el.Retweet,
                        ListUpdatet = DateTime.Now
                    };

                    if (el.Geo != null)
                    {
                        record.Longitude = el.Geo[0];
                        record.Latitude = el.Geo[1];
                    }

                    foreach (var person in el.Persons)
                    {
                        Person personCheck = oldPersons.FirstOrDefault(p => p.Name.ToLower().Equals(person.ToLower()));
                        if (personCheck == null)
                        {
                            personCheck = new Person()
                            {
                                Name = person,
                                IsHot = false,
                                Records = new List<Record>(),
                                Comparisons = new List<Comparison>(),
                                Keywords = new List<Keyword>(),
                                SubPlatforms = new List<Subplatform>(),
                                SubscribedProfiles = new List<Profile>()
                            };
                            oldPersons.Add(personCheck);
                            newPersons.Add(personCheck);
                        }
                        record.Persons.Add(personCheck);
                        personCheck.Records.Add(record);
                    }


                    foreach (var m in el.Mentions)
                    {
                        Mention mentionCheck = allMentions.Find(me => me.Name.Equals(m));
                        if (mentionCheck != null)
                        {
                            record.Mentions.Add(mentionCheck);
                        }
                        else
                        {
                            Mention mention = new Mention(m);
                            record.Mentions.Add(mention);
                            allMentions.Add(mention);
                        }
                    }


                    foreach (var w in el.Words)
                    {
                        Word wordCheck = allWords.Find(wo => wo.Text.Equals(w));
                        if (wordCheck != null)
                        {
                            record.Words.Add(wordCheck);

                        }
                        else
                        {
                            Word word = new Word(w);
                            record.Words.Add(word);
                            allWords.Add(word);
                        }
                    }


                    foreach (var h in el.Hashtags)
                    {
                        Hashtag hashtagCheck = allHashtags.Find(ha => ha.HashTag.Equals(h));
                        if (hashtagCheck != null)
                        {
                            record.Hashtags.Add(hashtagCheck);
                        }
                        else
                        {
                            Hashtag tag = new Hashtag(h);
                            record.Hashtags.Add(tag);
                            allHashtags.Add(tag);
                        }
                    }


                    foreach (var u in el.URLs)
                    {
                        Url urlCheck = allUrls.Find(url => url.Link.Equals(u));
                        if (urlCheck != null)
                        {
                            record.URLs.Add(urlCheck);
                        }
                        else
                        {
                            Url url = new Url(u);
                            record.URLs.Add(url);
                            allUrls.Add(url);
                        }
                    }

                    if (newRecords.FirstOrDefault(r => r.Tweet_Id == record.Tweet_Id) != null)
                    {
                        newRecords[newRecords.FindIndex(r => r.Tweet_Id == record.Tweet_Id)] = record;
                    }
                    else
                    {
                        newRecords.Add(record);
                    }
                }
            }

            RecordRepo.CreateRecords(newRecords);
            UowManager.Save();

            return newRecords;
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
                a.Username = profile.UserName;
            });

            alerts.ForEach(a =>
            {
                if (!profile.Alerts.Contains(a)) profile.Alerts.Add(a);
            });

            UowManager.Save();

            //Return alerts
            return alerts;
        }

        public void CheckTrend()
        {
            initNonExistingRepo();
            //initNonExistingRepoRecord();
            trendspotter.CheckTrendAverageRecords(GetRecords());
        }

        //private void RecordsToItems(List<Record> records)
        //{
        //    initNonExistingRepo();
        //    //initNonExistingRepoRecord();
        //    //initNonExistingRepoItem();
        //    //List<Record> records = GetRecords().ToList();
        //    List<Person> persons = GetPersons().ToList();
        //    List<Person> people = new List<Person>();
        //    Item item;

        //    records.ToList().ForEach(r =>
        //    {
        //        item = persons.FirstOrDefault(p => p.FirstName.Equals(r.RecordPerson.FirstName) && p.LastName.Equals(r.RecordPerson.LastName));
        //        if (item == null)
        //        {
        //            Person person = new Person()
        //            {
        //                ItemId = persons.Count,
        //                FirstName = r.RecordPerson.FirstName,
        //                LastName = r.RecordPerson.LastName,
        //                Keywords = r.Words.ConvertAll(w => new Keyword() { Name = w.Text }),
        //                SubPlatforms = new List<Subplatform>(),
        //                Records = new List<Record>() { r }
        //            };
        //            persons.Add(person);
        //            people.Add(person);
        //        }
        //        else
        //        {
        //            item.Records.Add(r);
        //        }
        //    });

        //    people.ForEach(p => ItemRepo.CreatePerson(p));
        //    UowManager.Save();
        //}
    }
}