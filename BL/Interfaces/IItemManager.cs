using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Person AddPerson(string name, string socialMediaLink, string iconUrl, Organisation organisation = null, Function function = null, bool isTrending = false, Subplatform subplatform = null, string Gemeente = null);
        Organisation AddOrganisation(string name, string fullname, string socialMediaLink = null, string iconUrl = null, bool isTrending = false, Subplatform subplatform = null);
        Theme AddTheme(string themeName, string description, string iconUrl, bool isTrending = false, Subplatform subplatform = null);
        void ChangeItem(Item item);
        void ChangeItems(List<Item> items);
        void RemoveItem(int itemId);
        void RemoveItem(int itemId, Subplatform subplatform);

        IEnumerable<Person> GetPersons();
        IEnumerable<Organisation> GetOrganisations();
        IEnumerable<Theme> GetThemes();
        IEnumerable<Keyword> GetKeywords();

        Keyword AddKeyword(string name, List<Item> items = null);
        void RemoveKeyword(int keywordId);

        IEnumerable<Record> GetRecords();
        Record GetRecord(long id);
        Record AddRecord(long tweetId, RecordProfile recordProfile, List<Word> words, Sentiment sentiment, string source, List<Hashtag> hashtags, List<Mention> mentions, List<Url> uRLs, List<Theme> themes, List<Person> persons, DateTime date, double longitude, double latitude, bool retweet);
        void ChangeRecord(Record record);
        void RemoveRecord(long id);
        IEnumerable<Record> GetRecordsFromItem(int itemId);

        List<Record> JClassToRecord(List<JClass> data);
        void SyncDatabase(Subplatform subplatform);
        Task<int> SyncDatabaseAsync(Subplatform subplatform);
        void CleanupOldRecords(Subplatform subplatform);

        int GetKeywordsCount();
        int GetThemesCount();
        int GetPersonsCount();
        int GetOrganisationsCount();
        int GetItemsCount();
    }
}
