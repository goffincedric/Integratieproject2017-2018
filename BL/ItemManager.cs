using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
  public class ItemManager : IItemManager
  {
    private ItemRepo ItemRepo = new ItemRepo();
    public RecordRepo RecordRepo = new RecordRepo();

    #region Items
    public Organisation AddOrganisation(string name, string socialMediaLink = null, string iconURL = null)
    {
      Organisation organisation = new Organisation()
      {
        Name = name,
        SocialMediaLink = socialMediaLink,
        IconURL = iconURL,
        Keywords = new List<Keyword>(),
        SubPlatforms = new List<SubPlatform>(),
        Records = new List<Record>(),
        People = new List<Person>()
      };

      return (Organisation)AddItem(organisation);
    }

    public Person AddPerson(string firstName, string lastName, DateTime birthDay, string socialMediaLink, string iconURL, Organisation organisation = null, Function function = null)
    {
      Person person = new Person()
      {
        FirstName = firstName,
        LastName = lastName,
        BirthDay = birthDay,
        SocialMediaLink = socialMediaLink,
        IconURL = iconURL,
        Function = function,
        Keywords = new List<Keyword>(),
        SubPlatforms = new List<SubPlatform>(),
        Records = new List<Record>(),
        Organisation = organisation
      };

      return (Person)AddItem(person);
    }

    public Theme AddTheme(string themeName, string description)
    {
      Item theme = new Theme()
      {
        ThemeName = themeName,
        Description = description,
        Keywords = new List<Keyword>(),
        SubPlatforms = new List<SubPlatform>(),
        Records = new List<Record>()
      };

      return (Theme)AddItem(theme);
    }

    private Item AddItem(Item item)
    {
      return ItemRepo.CreateItem(item);
    }

    public void ChangeItem(Item item)
    {
      ItemRepo.UpdateItem(item);
    }

    public Item GetItem(int itemId)
    {
      return ItemRepo.ReadItem(itemId);
    }

    public IEnumerable<Item> GetItems()
    {
      return ItemRepo.ReadItems();
    }

    public Organisation GetOrganistation(int itemId)
    {
      return (Organisation)ItemRepo.ReadItem(itemId);
    }

    public Person GetPerson(int itemId)
    {
      return (Person)ItemRepo.ReadItem(itemId);
    }

    public Theme GetTheme(int itemId)
    {
      return (Theme)ItemRepo.ReadItem(itemId);
    }

    public void RemoveItem(int itemId)
    {
      ItemRepo.DeleteItem(itemId);
    }
    #endregion


    #region Records
    public IEnumerable<Record> GetRecords()
    {
      return RecordRepo.ReadRecords();
    }

    public Record GetRecord(long id)
    {
      return RecordRepo.ReadRecord(id);
    }

    public Record AddRecord(string source, long id, string user_Id, List<string> mentions, DateTime date, string geo, List<string> politician, bool retweet, List<string> words, List<double> sentiment, List<string> hashtags, List<string> uRLs)
    {
      Record record = new Record()
      {

        Source = source,
        Id = id,
        User_Id = user_Id,
        Mentions = mentions,
        Date = date,
        Geo = geo,
        Politician = politician,
        Retweet = retweet,
        Words = words,
        Sentiment = sentiment,
        Hashtags = hashtags,
        URLs = uRLs
      };

      return AddRecord(record);
    }

    private Record AddRecord(Record record)
    {
      return RecordRepo.CreateRecord(record);
    }

    public void ChangeRecord(Record record)
    {
      RecordRepo.UpdateRecord(record);
    }

    public void RemoveRecord(long id)
    {
      RecordRepo.DeleteRecord(id);
    }
    #endregion


    public void Seed()
    {
      RecordRepo.Seed();

      //RecordsToItems();
    }

    private void RecordsToItems()
    {
      IEnumerable<Record> records = RecordRepo.ReadRecords();
      List<Person> people = new List<Person>();

      Item item;
      records.ToList().ForEach(r =>
      {
        item = people.FirstOrDefault(p => p.FirstName.Equals(r.Politician[0]) && p.LastName.Equals(r.Politician[1]));
        if (item == null)
        {
          people.Add(new Person()
          {
            ItemId = people.Count,
            FirstName = r.Politician.ToList()[0],
            LastName = r.Politician.ToList()[1],
            Keywords = r.Words.ConvertAll(w => new Keyword() { Name = w }),
            SubPlatforms = new List<SubPlatform>(),
            Records = new List<Record>() { r }
          });
        } else
        {
          item.Records.Add(r);
        }
      });

      people.ForEach(p => ItemRepo.CreateItem(p));
    }
  }
}
