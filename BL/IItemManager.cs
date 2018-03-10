using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public interface IItemManager
  {
    IEnumerable<Item> GetItems();
    Item GetItem(int itemId);
    Person GetPerson(int itemId);
    Organisation GetOrganistation(int itemId);
    Theme GetTheme(int itemId);
    Person AddPerson(string FirstName, string LastName, DateTime BirthDay, string SocialMediaLink, string IconURL, Organisation organisation, Function function);
    Organisation AddOrganisation(string name, string SocialMediaLink = null, string IconURL = null);
    Theme AddTheme(string themeName, string description);
    void ChangeItem(Item item);
    void RemoveItem(int itemId);


    IEnumerable<Record> GetRecords();
    Record GetRecord(long id);
    Record AddRecord(string source, long id, string user_Id, List<string> mentions, DateTime date, string geo, List<string> politician, bool retweet, List<string> words, List<double> sentiment, List<string> hashtags, List<string> uRLs);
    void ChangeRecord(Record record);
    void RemoveRecord(long id);
  }
}
