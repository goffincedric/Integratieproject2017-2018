using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL
{
//heey
  public class ItemManager : IItemManager
  {
    public Organisation AddOrganisation(string name, string SocialMediaLink = null, string IconURL = null)
    {
      throw new NotImplementedException();
    }

    public Person AddPerson(string FirstName, string LastName, DateTime BirthDay, string SocialMediaLink, string IconURL)
    {
      throw new NotImplementedException();
    }

    public Theme AddTheme(string description)
    {
      throw new NotImplementedException();
    }

    public void ChangeItem(Item item)
    {
      throw new NotImplementedException();
    }

    public Item GetItem(int itemId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Item> GetItems()
    {
      throw new NotImplementedException();
    }

    public Organisation GetOrganistation(int itemId)
    {
      throw new NotImplementedException();
    }

    public Person GetPerson(int itemId)
    {
      throw new NotImplementedException();
    }

    public Theme GetTheme(int itemId)
    {
      throw new NotImplementedException();
    }

    public void RemoveItem(int itemId)
    {
      throw new NotImplementedException();
    }
  }
}
