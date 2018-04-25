using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PB.BL
{
    public class ItemManager : IItemManager
    {
        private IItemRepo ItemRepo;
        private IRecordRepo RecordRepo;

        private UnitOfWorkManager UowManager;

        public ItemManager()
        {

        }

        public ItemManager(UnitOfWorkManager uowMgr)
        {
            UowManager = uowMgr;
            ItemRepo = new ItemRepo(uowMgr.UnitOfWork);
            RecordRepo = new RecordRepo(uowMgr.UnitOfWork);
        }

        public void InitNonExistingRepo(bool createWithUnitOfWork = false)
        {
            if (RecordRepo == null || ItemRepo == null)
            {
                if (createWithUnitOfWork)
                {
                    if (UowManager == null)
                    {
                        UowManager = new UnitOfWorkManager();
                        //Console.WriteLine("UOW MADE IN ITEM MANAGER for Record and Item repo");
                    }
                    else
                    {
                        //Console.WriteLine("uo bestaat al");
                    }

                    RecordRepo = new RecordRepo(UowManager.UnitOfWork);
                    ItemRepo = new ItemRepo(UowManager.UnitOfWork);
                }
                else
                {
                    RecordRepo = new RecordRepo();
                    ItemRepo = new ItemRepo();
                    //Console.WriteLine("OLD WAY REPO ITEMMGR");
                }
            }
        }

        #region Items
        public IEnumerable<Item> AddItems(List<Item> items)
        {
            InitNonExistingRepo();

            IEnumerable<Item> CreatedItems = ItemRepo.CreateItems(items);
            UowManager.Save();

            return items;
        }

        public Organisation AddOrganisation(string name, string description, string abbreviation, string socialMediaLink = null, string iconUrl = null)
        {
            InitNonExistingRepo();

            Organisation organisation = new Organisation()
            {
                Name = name,
                Description = description,
                Abbreviation = abbreviation,
                SocialMediaLink = socialMediaLink,
                IconURL = iconUrl,
                Keywords = new List<Keyword>(),
                SubPlatforms = new List<Subplatform>(),
                Records = new List<Record>(),
                People = new List<Person>()
            };

            organisation = (Organisation)AddItem(organisation);
            UowManager.Save();

            return organisation;
        }

        public Person AddPerson(string name, string socialMediaLink, string iconUrl, Organisation organisation = null, Function function = null)
        {
            InitNonExistingRepo();
            Person person = new Person()
            {
                Name = name,
                SocialMediaLink = socialMediaLink,
                IconURL = iconUrl,
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
            InitNonExistingRepo();
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
            InitNonExistingRepo();
            return ItemRepo.CreateItem(item);
        }

        public void ChangeItem(Item item)
        {
            InitNonExistingRepo();
            ItemRepo.UpdateItem(item);
            UowManager.Save();
        }

        public Item GetItem(int itemId)
        {
            InitNonExistingRepo();
            return ItemRepo.ReadItem(itemId);
        }

        public Item GetItem(string name)
        {
            InitNonExistingRepo();
            return ItemRepo.ReadItems().FirstOrDefault(i => i.Name.Equals(name));
        }

        public IEnumerable<Item> GetItems()
        {
            InitNonExistingRepo();
            return ItemRepo.ReadItems();
        }

        public IEnumerable<Organisation> GetOrganisations()
        {
            InitNonExistingRepo();
            return ItemRepo.ReadOrganisations();
        }

        public IEnumerable<Theme> GetThemes()
        {
            InitNonExistingRepo();
            return ItemRepo.ReadThemes();
        }

        public Organisation GetOrganisation(int itemId)
        {
            InitNonExistingRepo();
            return (Organisation)ItemRepo.ReadItem(itemId);
        }

        public Person GetPerson(int itemId)
        {
            InitNonExistingRepo();
            return (Person)ItemRepo.ReadItem(itemId);
        }

        public Theme GetTheme(int itemId)
        {
            InitNonExistingRepo();
            return (Theme)ItemRepo.ReadItem(itemId);
        }

        public void RemoveItem(int itemId)
        {
            InitNonExistingRepo();
            ItemRepo.DeleteItem(itemId);
            UowManager.Save();
        }

        public IEnumerable<Person> GetPersons()
        {
            InitNonExistingRepo();
            return ItemRepo.ReadPersons();
        }

        public void RemoveKeyword(int keywordId)
        {
            InitNonExistingRepo();

            Keyword keyword = ItemRepo.ReadKeyword(keywordId);
            keyword.Items.ForEach(i => i.Keywords.Remove(keyword));
            ItemRepo.DeleteKeyword(keywordId);

            UowManager.Save();
        }

        public Keyword AddKeyword(string name)
        {
            Keyword keyword = new Keyword()
            {
                Name = name,
                Items = new List<Item>()
            };

            keyword = ItemRepo.CreateKeyword(keyword);
            UowManager.Save();
            return keyword;
        }

        public int GetNumberOfMentions(Record record)
        {
            InitNonExistingRepo();
            return record.Mentions.Count;
        }

        public Dictionary<DateTime, int> GetTweetAmountByDate(Predicate<Record> predicate, DateTime since, DateTime until)
        {
            InitNonExistingRepo();
            Dictionary<DateTime, int> records =
                (from record in RecordRepo.ReadRecords().ToList().FindAll(r => predicate.Invoke(r) && r.Date > since && r.Date < until)
                 group record by record.Date.Date
                 into groupedRecords
                 select groupedRecords)
                    .ToDictionary(gr => gr.Key, gr => gr.ToList().Count);
            return records;
        }

        public int GetTotalTweetsByDate(Predicate<Record> predicate, DateTime since, DateTime until)
        {
            InitNonExistingRepo();
            return GetTweetAmountByDate(predicate, since, until).Values.Sum();
        }

        public Dictionary<DateTime, int> GetTweetAmountByDate(DateTime since, DateTime until)
        {
            InitNonExistingRepo();
            Dictionary<DateTime, int> records =
                (from record in RecordRepo.ReadRecords().ToList().FindAll(r => r.Date > since && r.Date < until)
                 group record by record.Date.Date
                 into groupedRecords
                 select groupedRecords)
                    .ToDictionary(gr => gr.Key, gr => gr.ToList().Count);
            return records;
        }

        public int GetTotalTweetsByDate(DateTime since, DateTime until)
        {
            InitNonExistingRepo();
            return GetTweetAmountByDate(since, until).Values.Sum();
        }

        public Dictionary<Person, int> GetTopPoliticians(int amount)
        {
            InitNonExistingRepo();
            return ItemRepo.ReadPersons().OrderByDescending(p => p.Records.Count).ToDictionary(p => p, p => p.Records.Count);
        }
        #endregion

        #region Records
        public IEnumerable<Record> GetRecords()
        {
            InitNonExistingRepo();
            return RecordRepo.ReadRecords();
        }

        public Record GetRecord(long id)
        {
            InitNonExistingRepo();
            return RecordRepo.ReadRecord(id);
        }

        public Record AddRecord(long tweetId, RecordProfile recordProfile, List<Word> words, Sentiment sentiment, string source, List<Hashtag> hashtags, List<Mention> mentions, List<Url> uRLs, List<Theme> themes, List<Person> persons, DateTime date, double longitude, double latitude, bool retweet)
        {
            InitNonExistingRepo();
            Record record = new Record()
            {
                Tweet_Id = tweetId,
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
                Retweet = retweet
            };

            record = AddRecord(record);
            UowManager.Save();
            return record;
        }

        private Record AddRecord(Record record)
        {
            InitNonExistingRepo();
            return RecordRepo.CreateRecord(record);
        }

        public void ChangeRecord(Record record)
        {
            InitNonExistingRepo();
            RecordRepo.UpdateRecord(record);
            UowManager.Save();
        }

        public void RemoveRecord(long id)
        {
            InitNonExistingRepo();
            RecordRepo.DeleteRecord(id);
            UowManager.Save();
        }
        #endregion

        #region Subscriptions
        public Profile AddSubscription(Profile profile, Item item)
        {
            InitNonExistingRepo();

            profile.Subscriptions.Add(item);
            item.SubscribedProfiles.Add(profile);

            ChangeItem(item);
            UowManager.Save();

            return profile;
        }

        public Profile RemoveSubscription(Profile profile, Item item)
        {
            InitNonExistingRepo();

            profile.Subscriptions.Remove(item);
            item.SubscribedProfiles.Remove(profile);

            ChangeItem(item);
            UowManager.Save();

            return profile;
        }
        #endregion

        public void Seed(bool evenRecords = true)
        {
            InitNonExistingRepo();
            List<Record> toegevoegde = JClassToRecord(RecordRepo.Seed(evenRecords));
        }

        public void CleanupOldRecords(Subplatform subplatform, int days)
        {
            InitNonExistingRepo();
            List<Person> persons = ItemRepo.ReadPersons().Where(i => i.SubPlatforms.Contains(subplatform)).ToList();
            List<Record> oldRecords = new List<Record>();

            persons.ForEach(p =>
            {
                oldRecords.AddRange(p.Records.Where(r => r.Date.Date < DateTime.Now.Date.AddDays(-days)));
            });

            RecordRepo.DeleteRecords(oldRecords);
            UowManager.Save();
        }

        public List<Record> JClassToRecord(List<JClass> data)
        {
            InitNonExistingRepo();
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
                        Sentiment = (el.Sentiment.Count != 0) ? new Sentiment(el.Sentiment[0], el.Sentiment[1]) : new Sentiment(0, 0),
                        Source = el.Source,
                        Hashtags = new List<Hashtag>(),
                        Mentions = new List<Mention>(),
                        URLs = new List<Url>(),
                        Themes = new List<Theme>(),
                        Persons = new List<Person>(),
                        Date = el.Date,
                        Retweet = el.Retweet
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
                                SubPlatforms = el.Subplatforms,
                                SubscribedProfiles = new List<Profile>()
                            };
                            oldPersons.Add(personCheck);
                            newPersons.Add(personCheck);
                        }
                        else
                        {
                            el.Subplatforms.ForEach(sp =>
                            {
                                if (!personCheck.SubPlatforms.Contains(sp)) personCheck.SubPlatforms.Add(sp);
                            });
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

        public void CheckTrend()
        {
            //InitNonExistingRepo();
            //trendspotter.CheckTrendAverageRecords(GetRecords());
        }

        #region Keywords
        public IEnumerable<Keyword> GetKeywords()
        {
            return ItemRepo.ReadKeywords();
        }
        #endregion

        public int GetKeywordsCount()
        {
            return ItemRepo.ReadKeywords().Count();
        }

        public int GetThemesCount()
        {
            return ItemRepo.ReadThemes().Count();
        }

        public int GetPersonsCount()
        {
            return ItemRepo.ReadPersons().Count();
        }

        public int GetOrganisationsCount()
        {
            return ItemRepo.ReadOrganisations().Count();
        }

        public int GetItemsCount()
        {
            return ItemRepo.ReadItems().Count();
        }
    }
}