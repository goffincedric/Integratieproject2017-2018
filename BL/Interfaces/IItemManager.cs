using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.JSONConversion;
using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.BL.Domain.Platform;

namespace PB.BL.Interfaces
{
    public interface IItemManager
    {
        IEnumerable<Item> GetItems();
        Item GetItem(int itemId);
        Item GetItem(string name);
        Person GetPerson(int itemId);
        Organisation GetOrganisation(int itemId);
        Theme GetTheme(int itemId);
        IEnumerable<Item> AddItems(List<Item> items);

        Person AddPerson(string name, string socialMediaLink, string iconUrl, bool isTrending = false,
            string firstName = null, string lastName = null, string level = null, string site = null,
            string twitterName = null, string position = null, string district = null, string gemeente = null,
            string postalCode = null, Gender? gender = null, Organisation organisation = null,
            Subplatform subplatform = null, DateTime? dateOfBirth = null);

        Organisation AddOrganisation(string name, string fullname, string socialMediaLink = null,
            List<Theme> themes = null, string iconUrl = null, bool isTrending = false, Subplatform subplatform = null);

        Theme AddTheme(string themeName, string description, string iconUrl, List<Keyword> keywords = null,
            bool isTrending = false, Subplatform subplatform = null);

        void ChangeItem(Item item);
        void ChangeItems(List<Item> items);
        void ChangePerson(Person person);
        void ChangeTheme(Theme theme);
        void ChangeOrganisation(Organisation organisation);
        void RemoveItem(int itemId);
        void RemoveItem(int itemId, Subplatform subplatform);
        IEnumerable<Person> GetPersons();
        IEnumerable<Organisation> GetOrganisations();
        IEnumerable<Theme> GetThemes();
        int GetItemsCount();
        int GetPersonsCount();
        int GetOrganisationsCount();
        int GetThemesCount();


        Keyword AddKeyword(string name, List<Item> items = null);
        IEnumerable<Keyword> GetKeywords();
        IEnumerable<Keyword> GetKeywords(int itemId);
        Keyword GetKeyword(int keywordId);
        void ChangeKeyword(Keyword keyword);
        void RemoveKeyword(int keywordId);
        int GetKeywordsCount();

        IEnumerable<Record> GetRecords();
        Record GetRecord(long id);

        Record AddRecord(long tweetId, RecordProfile recordProfile, List<Word> words, Sentiment sentiment,
            string source, List<Hashtag> hashtags, List<Mention> mentions, List<Url> uRLs, List<Theme> themes,
            List<Person> persons, DateTime date, double longitude, double latitude, bool retweet);

        void ChangeRecord(Record record);
        void RemoveRecord(long id);
        IEnumerable<Record> GetRecordsFromItem(int itemId);

        List<Record> JClassToRecord(List<JClass> data);
        List<Item> JPersonToRecord(List<JPerson> data, Subplatform subplatform);
        void SyncDatabase(Subplatform subplatform);
        Task<int> SyncDatabaseAsync(Subplatform subplatform);
        void CleanupOldRecords(Subplatform subplatform);
    }
}