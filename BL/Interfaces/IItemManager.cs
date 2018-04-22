using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;

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
        Person AddPerson(string name, string socialMediaLink, string iconUrl, Organisation organisation = null, Function function = null);
        Organisation AddOrganisation(string name, string description, string socialMediaLink = null, string iconUrl = null);
        Theme AddTheme(string themeName, string description);
        void ChangeItem(Item item);
        void RemoveItem(int itemId);


        IEnumerable<Person> GetPersons();
        IEnumerable<Organisation> GetOrganisations();
        IEnumerable<Theme> GetThemes();
        IEnumerable<Keyword> GetKeywords();

        Keyword AddKeyword(string name);
        void RemoveKeyword(int keywordId);

        IEnumerable<Record> GetRecords();
        Record GetRecord(long id);
        Record AddRecord(long tweetId, RecordProfile recordProfile, List<Word> words, Sentiment sentiment, string source, List<Hashtag> hashtags, List<Mention> mentions, List<Url> uRLs, List<Theme> themes, List<Person> persons, DateTime date, double longitude, double latitude, bool retweet);
        void ChangeRecord(Record record);
        void RemoveRecord(long id);

        List<Record> JClassToRecord(List<JClass> data);
        void CleanupOldRecords(Subplatform subplatform, int days);

        int GetKeywordsCount();
        int GetThemesCount();
        int GetPersonsCount();
        int GetOrganisationsCount();
        int GetItemsCount();
    }
}
