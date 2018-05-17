using System.Collections.Generic;
using PB.BL.Domain.Items;

namespace PB.DAL
{
    public interface IItemRepo
    {
        IEnumerable<Item> ReadItems();
        IEnumerable<Item> ReadItemsEmpty();
        Item CreateItem(Item item);
        IEnumerable<Item> CreateItems(List<Item> items);
        Item ReadItem(int itemId);
        void UpdateItem(Item item);
        void UpdateItems(List<Item> items);
        void DeleteItem(int itemId);
        void UpdatePerson(Person person);
        void UpdateKeyword(Keyword keyword);
        void UpdateTheme(Theme theme);
        void UpdateOrganisation(Organisation organisation);


        Person ReadPerson(int itemId);
        IEnumerable<Person> ReadPersons();
        Person CreatePerson(Person person);

        Organisation ReadOrganisation(int itemId);
        IEnumerable<Organisation> ReadOrganisations();
        Organisation CreateOrganisation(Organisation organisation);

        Theme ReadTheme(int itemId);
        IEnumerable<Theme> ReadThemes();
        Theme CreateTheme(Theme theme);

        Keyword CreateKeyword(Keyword keyword);
        IEnumerable<Keyword> ReadKeywords();
        Keyword ReadKeyword(int keywordId);
        void DeleteKeyword(int keywordId);
    }
}