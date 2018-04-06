using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.BL.Domain.Platform;
using PB.DAL.EF;

namespace PB.DAL
{
    public class SubplatformRepo : ISubplatformRepo
    {
        private IntegratieDbContext ctx;

        public SubplatformRepo()
        {
            ctx = new IntegratieDbContext();
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
                .Include("Settings")
                .AsEnumerable();
        }

        public Subplatform ReadSubplatform(int subplatformId)
        {
            return ctx.Subplatforms
                .Include("Settings")
                .FirstOrDefault(s => s.SubplatformId == subplatformId);
        }

        public void UpdateSubplatform(Subplatform subplatform)
        {
            ctx.Subplatforms.Attach(subplatform);
            ctx.Entry(subplatform).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
