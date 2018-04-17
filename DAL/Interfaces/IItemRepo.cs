using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL
{
    public interface IItemRepo
    {
        IEnumerable<Item> ReadItems();
        Item CreateItem(Item item);
        IEnumerable<Item> CreateItems(List<Item> items);
        Item ReadItem(int itemId);
        void UpdateItem(Item item);
        void DeleteItem(int itemId);


        IEnumerable<Person> ReadPersons();
        Person CreatePerson(Person person);
        Person ReadPerson(int itemId);

        IEnumerable<Organisation> ReadOrganisations();
        IEnumerable<Theme> ReadThemes();
    }
}
