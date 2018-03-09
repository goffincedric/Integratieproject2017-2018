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
    Person AddPerson(string FirstName, string LastName, DateTime BirthDay, string SocialMediaLink, string IconURL);
    Organisation AddOrganisation(string name, string SocialMediaLink = null, string IconURL = null);
    Theme AddTheme(string description);
    void ChangeItem(Item item);
    void RemoveItem(int itemId);
  }
}
