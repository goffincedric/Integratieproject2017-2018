using PB.BL.Domain.Platform;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace PB.DAL
{
    public class SubplatformRepo : ISubplatformRepo
    {
        private readonly IntegratieDbContext ctx;

        public SubplatformRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public SubplatformRepo(IntegratieDbContext context)
        {
            ctx = context;
        }

        public SubplatformRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public Subplatform CreateSubplatform(Subplatform subplatform)
        {
            ctx.Subplatforms.Add(subplatform);

            try
            {
                ctx.SaveChanges();
            }
            catch(DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return subplatform;
        }

        public void DeleteSubplatform(int subplatformId)
        {
            Subplatform subplatform = ReadSubplatform(subplatformId);
            if (subplatform != null)
            {
                ctx.Subplatforms.Remove(subplatform);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Subplatform> ReadSubplatforms()
        {
            return ctx.Subplatforms
                .Include(s => s.Items)
                .Include(s => s.Dashboards)
                .Include(s => s.Admins)
                .Include(s => s.Settings)
                .AsEnumerable();
        }

        public Subplatform ReadSubplatform(int subplatformId)
        {
            return ctx.Subplatforms
                .Include(s => s.Items)
                .Include(s => s.Dashboards)
                .Include(s => s.Admins)
                .Include(s => s.Settings)
                .FirstOrDefault(s => s.SubplatformId == subplatformId);
        }

        public Subplatform ReadSubplatform(string subplatformURL)
        {
            return ctx.Subplatforms
                .Include(s => s.Items)
                .Include(s => s.Dashboards)
                .Include(s => s.Admins)
                .Include(s => s.Settings)
                .FirstOrDefault(s => s.URL.ToLower().Equals(subplatformURL.ToLower()));
        }

        public void UpdateSubplatform(Subplatform subplatform)
        {
            ctx.Subplatforms.Attach(subplatform);
            ctx.Entry(subplatform).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public SubplatformSetting CreateSubplatformSetting(SubplatformSetting subplatformSetting)
        {
            subplatformSetting = ctx.SubplatformSettings.Add(subplatformSetting);

            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return subplatformSetting;
        }

        public void UpdateSubplatformSetting(SubplatformSetting subplatformSetting)
        {
            ctx.SubplatformSettings.Attach(subplatformSetting);
            ctx.Entry(subplatformSetting).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<Page> ReadPages()
        {
            return ctx.Pages
                .Include(p => p.Tags)
                .Include(p => p.Subplatform)
                .AsEnumerable();
        }

        public Page ReadPage(int pageId)
        {
            return ctx.Pages
                .Include(p => p.Tags)
                .Include(p => p.Subplatform)
                .FirstOrDefault(p => p.PageId == pageId);
        }

        public Page CreatePage(Page page)
        {
            page = ctx.Pages.Add(page);

            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return page;
        }

        public void UpdatePage(Page page)
        {
            ctx.Pages.Attach(page);
            ctx.Entry(page).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeletePage(int pageId)
        {
            Page page = ReadPage(pageId);
            if (page != null)
            {
                ctx.Pages.Remove(page);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Tag> ReadTags()
        {
            return ctx.Tags
                .Include(t => t.Page)
                .AsEnumerable();
        }

        public Tag ReadTag(int tagId)
        {
            return ctx.Tags
                .Include(t => t.Page)
                .FirstOrDefault(t => t.TagId == tagId);
        }

        public Tag CreateTag(Tag tag)
        {
            tag = ctx.Tags.Add(tag);

            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return tag;
        }

        public void UpdateTag(Tag tag)
        {
            ctx.Tags.Attach(tag);
            ctx.Entry(tag).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteTag(int tagId)
        {
            Tag tag = ReadTag(tagId);
            if (tag != null)
            {
                ctx.Tags.Remove(tag);
                ctx.SaveChanges();
            }
        }
    }
}
