using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Items;

namespace PB.DAL
{
  public class ItemRepoHC : IItemRepoHC
  {
    private List<Item> repoItems = new List<Item>();

    public Item CreateItem(Item item)
    {
      repoItems.Add(item);
      return item;
    }

    public void DeleteItem(int itemId)
    {
      Item item = ReadItem(itemId);
      if (item != null) repoItems.Remove(item);
    }

    public Item ReadItem(int itemId)
    {
      return repoItems.FirstOrDefault(p => p.ItemId == itemId);
    }

    public IEnumerable<Item> ReadItems()
    {
      return repoItems.AsEnumerable();
    }

    public void UpdateItem(Item item)
    {
      //MemoryRepo, alle objecten worden automatisch geüpdatet in het geheugen
    }
  }
}
