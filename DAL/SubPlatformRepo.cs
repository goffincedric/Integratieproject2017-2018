using PB.BL.Domain.Platform;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Data.Entity;

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
             //Console.WriteLine("UOW MADE SUBPLATFORMREPO");
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
                .Include(s => s.Settings)
                .Include(s => s.Admins)
                .Include(s => s.Dashboards)
                .AsEnumerable();
        }

        public Subplatform ReadSubplatform(int subplatformId)
        {
            return ctx.Subplatforms
                .Include(s => s.Settings)
                .Include(s => s.Admins)
                .Include(s => s.Dashboards)
                .FirstOrDefault(s => s.SubplatformId == subplatformId);
        }

        public void UpdateSubplatform(Subplatform subplatform)
        {
            ctx.Subplatforms.Attach(subplatform);
            ctx.Entry(subplatform).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
