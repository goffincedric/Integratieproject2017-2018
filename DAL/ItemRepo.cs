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
      Console.WriteLine("UOW MADE ITEMREPO");
    }

    public Item CreateItem(Item item)
    {
      ctx.Items.AddOrUpdate(item);
      ctx.SaveChanges();
      return item;
    }

    public void DeleteItem(int itemId)
    {
      Item item = ReadItem(itemId);
      if (item != null) ctx.Items.Remove(item);
      ctx.SaveChanges();
    }

    public IEnumerable<Person> ReadPersons()
    {
      return ctx.Persons.AsEnumerable();
    }

    public Person CreatePerson(Person person)
    {
      ctx.Persons.AddOrUpdate(person);
      ctx.SaveChanges();
      return person;
    }

    public Person ReadPerson(int itemId)
    {
      return ctx.Persons.FirstOrDefault(p => p.ItemId == itemId);
    }

    public Item ReadItem(int itemId)
    {
      return ctx.Items
                //.Include("Records")
                //.Include("Keywords")
                //.Include("SubscribedProfiles")
                .FirstOrDefault(p => p.ItemId == itemId);
    }

    public IEnumerable<Item> ReadItems()
    {
      return ctx.Items
                //.Include("Records")
                //.Include("Keywords")
                //.Include("SubscribedProfiles")
                .AsEnumerable();
    }

    public void UpdateItem(Item item)
    {
      ctx.Items.Attach(item);
      ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
      ctx.SaveChanges();
    }
  }
}
