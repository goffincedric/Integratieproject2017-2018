using System;
using System.Collections.Generic;
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

    public Item CreateItem(Item item)
    {
      ctx.Items.Add(item);
      return item;
    }

    public void DeleteItem(int itemId)
    {
      Item item = ReadItem(itemId);
      if (item != null) ctx.Items.Remove(item);
    }

    public Item ReadItem(int itemId)
    {
      return ctx.Items.FirstOrDefault(p => p.ItemId == itemId);
    }

    public IEnumerable<Item> ReadItems()
    {
      return ctx.Items.AsEnumerable();
    }

    public void UpdateItem(Item item)
    {
      ctx.Items.Attach(item);
      ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
      ctx.SaveChanges();
    }
  }
}
