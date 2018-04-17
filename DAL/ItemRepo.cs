using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Items;
using PB.DAL.EF;

namespace PB.DAL
{
  public class ItemRepo : IItemRepo
  {
    private IntegratieDbContext ctx;

    public ItemRepo()
    {
      ctx = new IntegratieDbContext();
    }

    public ItemRepo(UnitOfWork uow)
    {
      ctx = uow.Context;
      //Console.WriteLine("UOW MADE ITEMREPO");
    }

    public Item CreateItem(Item item)
    {
      item = ctx.Items.Add(item);
      ctx.SaveChanges();
      return item;
    }

    public IEnumerable<Item> CreateItems(List<Item> items)
    {
      ctx.Items.AddRange(items);
      ctx.SaveChanges();
      return items;
    }

    public void DeleteItem(int itemId)
    {
      Item item = ReadItem(itemId);
      if (item != null) ctx.Items.Remove(item);
      ctx.SaveChanges();
    }

    public Person CreatePerson(Person person)
    {
      person = ctx.Persons.Add(person);
      ctx.SaveChanges();
      return person;
    }

    public Person ReadPerson(int itemId)
    {
      return ctx.Persons
          .Include("Records")
          .Include("SubPlatforms")
          .Include("SubscribedProfiles")
          .FirstOrDefault(p => p.ItemId == itemId);
    }

    public IEnumerable<Person> ReadPersons()
    {
      return ctx.Persons
          .Include("Records")
          .Include("SubPlatforms")
          .Include("SubscribedProfiles")
          .AsEnumerable();
    }

    public Item ReadItem(int itemId)
    {
      return ctx.Items
          .Include("SubPlatforms")
          .Include("SubscribedProfiles")
          .FirstOrDefault(p => p.ItemId == itemId);
    }

    public IEnumerable<Item> ReadItems()
    {
      return ctx.Items
          .Include("SubPlatforms")
          .Include("SubscribedProfiles")
          .OfType<Person>()
          .Concat<Item>(
              ctx.Items
              .OfType<Theme>()
          )
          .Concat<Item>(
              ctx.Items
              .OfType<Organisation>()
          )
          .AsEnumerable();
    }

    public void UpdateItem(Item item)
    {
      ctx.Items.Attach(item);
      ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
      ctx.SaveChanges();
    }


    public IEnumerable<Organisation> ReadOrganisations()
    {
      return ctx.Organisations
         .Include("Records")
         .Include("SubPlatforms")
         .Include("People")
         .Include("Keywords")
        .AsEnumerable();
    }

    public IEnumerable<Theme> ReadThemes()
    {
      return ctx.Themes
          .Include("Records")
          .Include("SubPlatforms")
          .Include("Keywords")
          .AsEnumerable();
    }
  }
}
