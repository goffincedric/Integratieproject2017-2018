using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PB.BL.Domain.Items;
using PB.DAL.EF;

namespace PB.DAL
{
    public class ItemRepo : IItemRepo
    {
        private readonly IntegratieDbContext ctx;

        public ItemRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public ItemRepo(IntegratieDbContext context)
        {
            ctx = context;
        }

        public ItemRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
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
                .Include(p => p.Records)
                .Include(p => p.SubPlatforms)
                .Include(p => p.SubscribedProfiles)
                .Include(p => p.Keywords)
                .Include(p => p.Alerts)
                .Include(p => p.Elements)
                .Include(p => p.Organisation)
                .Include(p => p.Themes)
                .FirstOrDefault(p => p.ItemId == itemId);
        }

        public IEnumerable<Person> ReadPersons()
        {
            return ctx.Persons
                .Include(p => p.Records)
                .Include(p => p.SubPlatforms)
                .Include(p => p.SubscribedProfiles)
                .Include(p => p.Keywords)
                .Include(p => p.Alerts)
                .Include(p => p.Elements)
                .Include(p => p.Organisation)
                .Include(p => p.Themes)
                .AsEnumerable();
        }

        public Organisation ReadOrganisation(int itemId)
        {
            return ctx.Organisations
                .Include(o => o.SubPlatforms)
                .Include(o => o.People)
                .Include(o => o.Keywords)
                .Include(o => o.SubscribedProfiles)
                .Include(o => o.Elements)
                .Include(o => o.Alerts)
                .FirstOrDefault(o => o.ItemId == itemId);
        }

        public IEnumerable<Organisation> ReadOrganisations()
        {
            return ctx.Organisations
                .Include(o => o.SubPlatforms)
                .Include(o => o.People)
                .Include(o => o.Keywords)
                .Include(o => o.SubscribedProfiles)
                .Include(o => o.Elements)
                .Include(o => o.Alerts)
                .AsEnumerable();
        }

        public Theme ReadTheme(int itemId)
        {
            return ctx.Themes
                .Include(t => t.SubPlatforms)
                .Include(t => t.Keywords)
                .Include(t => t.SubscribedProfiles)
                .Include(t => t.Elements)
                .Include(t => t.Alerts)
                .Include(t => t.Records)
                .FirstOrDefault(t => t.ItemId == itemId);
        }

        public IEnumerable<Theme> ReadThemes()
        {
            return ctx.Themes
                .Include(t => t.SubPlatforms)
                .Include(t => t.Keywords)
                .Include(t => t.SubscribedProfiles)
                .Include(t => t.Elements)
                .Include(t => t.Alerts)
                .Include(t => t.Records)
                .AsEnumerable();
        }

        public Item ReadItem(int itemId)
        {
            return ctx.Items
                .Include(i => i.SubPlatforms)
                .Include(i => i.SubscribedProfiles)
                .Include(i => i.Keywords)
                .Include(i => i.Alerts)
                .Include(i => i.Elements)
                .FirstOrDefault(p => p.ItemId == itemId);
        }

        public IEnumerable<Item> ReadItems()
        {
            return ctx.Items
                .Include(i => i.SubPlatforms)
                .Include(i => i.SubscribedProfiles)
                .Include(i => i.Keywords)
                .Include(i => i.Alerts)
                .Include(i => i.Elements)
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
            items.ForEach(item => { ctx.Items.Attach(item); });
            ctx.SaveChanges();
        }

        public void UpdatePerson(Person person)
        {
            ctx.Persons.Attach(person);
            ctx.Entry(person).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void UpdateKeyword(Keyword keyword)
        {
            ctx.Keywords.Attach(keyword);
            ctx.Entry(keyword).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void UpdateTheme(Theme theme)
        {
            ctx.Themes.Attach(theme);
            ctx.Entry(theme).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void UpdateOrganisation(Organisation organisation)
        {
            ctx.Organisations.Attach(organisation);
            ctx.Entry(organisation).State = EntityState.Modified;
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