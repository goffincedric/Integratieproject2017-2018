using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;
using Domain.JSONConversion;
using PB.BL.Domain.Platform;

namespace PB.BL
{
    public interface IItemManager
    {
        IEnumerable<Item> GetItems();
        Item GetItem(int itemId);
        Person GetPerson(int itemId);
        Organisation GetOrganistation(int itemId);
        Theme GetTheme(int itemId);
        Person AddPerson(string name, DateTime birthDay, string socialMediaLink, string iconURL, Organisation organisation = null, Function function = null);
        Organisation AddOrganisation(string name, string SocialMediaLink = null, string IconURL = null);
        Theme AddTheme(string themeName, string description);
        void ChangeItem(Item item);
        void RemoveItem(int itemId);


        IEnumerable<Person> GetPersons();
        IEnumerable<Organisation> GetOrganisations();
        IEnumerable<Theme> GetThemes();
        IEnumerable<Keyword> GetKeywords();

        IEnumerable<Record> GetRecords();
        Record GetRecord(long id);
        Record AddRecord(long tweet_Id, RecordProfile recordProfile, List<Word> words, Sentiment sentiment, string source, List<Hashtag> hashtags, List<Mention> mentions, List<Url> uRLs, List<Theme> themes, List<Person> persons, DateTime date, double longitude, double latitude, bool retweet);
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
