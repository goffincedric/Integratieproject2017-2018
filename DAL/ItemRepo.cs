using PB.BL.Domain.Items;
using PB.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PB.DAL
{
    public class ItemRepo : IItemRepo
    {
        private readonly IntegratieDbContext ctx;

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

        public Organisation CreateOrganisation(Organisation organisation)
        {
            organisation = ctx.Organisations.Add(organisation);
            ctx.SaveChanges();
            return organisation;
        }

        public Theme CreateTheme(Theme theme)
        {
            theme = ctx.Themes.Add(theme);
            ctx.SaveChanges();
            return theme;
        }

        public Person ReadPerson(int itemId)
        {
            return ctx.Persons
                .Include(i => i.Records)
                .Include(i => i.SubPlatforms)
                .Include(i => i.SubscribedProfiles)
                .Include(i => i.Keywords)
                .Include(i => i.Alerts)
                .Include(i => i.Comparisons)
                .FirstOrDefault(p => p.ItemId == itemId);
        }

        public IEnumerable<Person> ReadPersons()
        {
            return ctx.Persons
                .Include(i => i.Records)
                .Include(i => i.SubPlatforms)
                .Include(i => i.SubscribedProfiles)
                .Include(i => i.Keywords)
                .Include(i => i.Alerts)
                .Include(i => i.Comparisons)
                .AsEnumerable();
        }

        public Organisation ReadOrganisation(int itemId)
        {
            return ctx.Organisations
                .Include(o => o.Records)
                .Include(o => o.SubPlatforms)
                .Include(o => o.People)
                .Include(o => o.Keywords)
                .Include(o => o.SubscribedProfiles)
                .Include(o => o.Comparisons)
                .Include(o => o.Alerts)
                .FirstOrDefault(o => o.ItemId == itemId);
        }

        public IEnumerable<Organisation> ReadOrganisations()
        {
            return ctx.Organisations
                .Include(o => o.Records)
                .Include(o => o.SubPlatforms)
                .Include(o => o.People)
                .Include(o => o.Keywords)
                .Include(o => o.SubscribedProfiles)
                .Include(o => o.Comparisons)
                .Include(o => o.Alerts)
                .AsEnumerable();
        }

        public Theme ReadTheme(int itemId)
        {
            return ctx.Themes
                .Include(t => t.Records)
                .Include(t => t.SubPlatforms)
                .Include(t => t.Keywords)
                .Include(t => t.SubscribedProfiles)
                .Include(t => t.Comparisons)
                .Include(t => t.Alerts)
                .FirstOrDefault(t => t.ItemId == itemId);
        }

        public IEnumerable<Theme> ReadThemes()
        {
            return ctx.Themes
                .Include(t => t.Records)
                .Include(t => t.SubPlatforms)
                .Include(t => t.Keywords)
                .Include(t => t.SubscribedProfiles)
                .Include(t => t.Comparisons)
                .Include(t => t.Alerts)
                .AsEnumerable();
        }

        public Item ReadItem(int itemId)
        {
            return ctx.Items
                .Include(i => i.SubPlatforms)
                .Include(i => i.SubscribedProfiles)
                .Include(i => i.Keywords)
                .Include(i => i.Alerts)
                .Include(i => i.Comparisons)
                .FirstOrDefault(p => p.ItemId == itemId);
        }

        public IEnumerable<Item> ReadItems()
        {
            return ctx.Items
                .Include(i => i.SubPlatforms)
                .Include(i => i.SubscribedProfiles)
                .Include(i => i.Keywords)
                .Include(i => i.Alerts)
                .Include(i => i.Comparisons)
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

        public IEnumerable<Item> ReadItemsEmpty()
        {
            return ctx.Items.AsEnumerable();
        }

        public void UpdateItem(Item item)
        {
            ctx.Items.Attach(item);
            ctx.Entry(item).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void UpdateItems(List<Item> items)
        {
            items.ForEach(item =>
            {
                ctx.Items.Attach(item);
            });
            ctx.SaveChanges();
        }

        public Keyword CreateKeyword(Keyword keyword)
        {
            return ctx.Keywords.Add(keyword);
        }

        public IEnumerable<Keyword> ReadKeywords()
        {
            return ctx.Keywords
                .Include(k => k.Items)
                .AsEnumerable();
        }

        public Keyword ReadKeyword(int keywordId)
        {
            return ctx.Keywords
                .FirstOrDefault(k => k.KeywordId == keywordId);
        }

        public void DeleteKeyword(int keywordId)
        {
            Keyword keyword = ReadKeyword(keywordId);
            ctx.Keywords.Remove(keyword);
        }
    }
}