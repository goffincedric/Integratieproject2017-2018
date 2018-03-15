using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;

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

    public ItemManager(UnitOfWorkManager uofMgr)
    {
      uowManager = uofMgr;
      ItemRepo = new ItemRepo(uofMgr.UnitOfWork);
      RecordRepo = new RecordRepo(uofMgr.UnitOfWork);
      

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
        SubPlatforms = new List<SubPlatform>(),
        Records = new List<Record>(),
        People = new List<Person>()
      };

      organisation = (Organisation) AddItem(organisation);
      uowManager.Save();
      
      return organisation;
    }

    public Person AddPerson(string firstName, string lastName, DateTime birthDay, string socialMediaLink, string iconURL, Organisation organisation = null, Function function = null)
    {
      //initNonExistingRepoItem();
     initNonExistingRepo();
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
      person = (Person) AddItem(person); 
      uowManager.Save();
      return person;
    }

    public Theme AddTheme(string themeName, string description)
    {
      //initNonExistingRepoItem();
      initNonExistingRepo();
      Theme theme = new Theme()
      {
        ThemeName = themeName,
        Description = description,
        Keywords = new List<Keyword>(),
        SubPlatforms = new List<SubPlatform>(),
        Records = new List<Record>()
      };

      theme = (Theme) AddItem(theme);
      uowManager.Save();
      return theme;
    }

    private Item AddItem(Item item)
    {
      initNonExistingRepo();
     // initNonExistingRepoItem();
      return ItemRepo.CreateItem(item);
    }

    public void ChangeItem(Item item)
    {
     initNonExistingRepo();
       //initNonExistingRepoItem();
      ItemRepo.UpdateItem(item);
      uowManager.Save();
    }

    public Item GetItem(int itemId)
    {

     // initNonExistingRepoItem();
      initNonExistingRepo();
      return ItemRepo.ReadItem(itemId);
    }

    public IEnumerable<Item> GetItems()
    {
     // initNonExistingRepoItem();
     initNonExistingRepo();
      return ItemRepo.ReadItems();
    }

    public Organisation GetOrganistation(int itemId)
    {
     // initNonExistingRepoItem();
     initNonExistingRepo();
      return (Organisation)ItemRepo.ReadItem(itemId);
    }

    public Person GetPerson(int itemId)
    {
      initNonExistingRepo();
       //initNonExistingRepoItem();
      return (Person)ItemRepo.ReadItem(itemId);
    }

    public Theme GetTheme(int itemId)
    {
      initNonExistingRepo();
     // initNonExistingRepoItem();
      return (Theme)ItemRepo.ReadItem(itemId);
    }

    public void RemoveItem(int itemId)
    {
      initNonExistingRepo();
      //initNonExistingRepoItem();
      ItemRepo.DeleteItem(itemId);
      uowManager.Save();
    }
    #endregion


    #region Records
    public IEnumerable<Record> GetRecords()
    {
     initNonExistingRepo();
     // initNonExistingRepo();
      return RecordRepo.ReadRecords();
    }

    public Record GetRecord(long id)
    {
      initNonExistingRepo();
      //initNonExistingRepoRecord();
      return RecordRepo.ReadRecord(id);
    }

    public Record AddRecord(string source, long Tweet_Id, string user_Id, List<Mention> mentions, DateTime date, string geo, RecordPerson recordPerson,
      bool retweet, List<Words> words, Sentiment sentiment, List<Hashtag> hashtags, List<Url> uRLs)
    {
      //initNonExistingRepoRecord();
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
      //initNonExistingRepoRecord();
      return RecordRepo.CreateRecord(record);
    }

    public void ChangeRecord(Record record)
    {
      initNonExistingRepo();
      //initNonExistingRepoRecord();
      RecordRepo.UpdateRecord(record);
      uowManager.Save();
    }

    public void RemoveRecord(long id)
    {
      initNonExistingRepo();
      //initNonExistingRepoRecord();
      RecordRepo.DeleteRecord(id);
      uowManager.Save();
    }
    #endregion


    public void Seed()
    {
      initNonExistingRepo();
      //initNonExistingRepoRecord();
      RecordRepo.Seed();
      uowManager.Save();
      RecordsToItems();
      uowManager.Save();
    }

    public void Seed2()
    {
      initNonExistingRepo();
      RecordRepo.Seed2();
      uowManager.Save();
      RecordsToItems();
      uowManager.Save();
    }

    public void CheckTrend()
    {
     initNonExistingRepo();
     // initNonExistingRepoRecord();
      trendspotter.CheckTrend(GetRecords());
    }
    private void RecordsToItems()
    {


      initNonExistingRepo();
      //initNonExistingRepoRecord();
      //initNonExistingRepoItem();
      List<Record> records = GetRecords().ToList();
      List<Person> people = new List<Person>();
      List<Item> persons = GetItems().ToList();
      Item item;

      records.ToList().ForEach(r =>
      {
        item = people.FirstOrDefault(p => p.FirstName.Equals(r.RecordPerson.FirstName) && p.LastName.Equals(r.RecordPerson.LastName));
        if (item == null)
        {
          people.Add(new Person()
          {
            ItemId = people.Count,
            FirstName = r.RecordPerson.FirstName,
            LastName = r.RecordPerson.LastName,
            Keywords = r.Words.ConvertAll(w => new Keyword() { Name = w.Word }),
            SubPlatforms = new List<SubPlatform>(),
            Records = new List<Record>() { r }
          });
        }
        else
        {
          item.Records.Add(r);
        }
      });

      people.ForEach(p => AddItem(p));
      uowManager.Save();
    }

  }
}