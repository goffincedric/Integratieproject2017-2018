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

        public Subplatform CreateSubPlatform(Subplatform subPlatform)
        {
            ctx.SubPlatforms.Add(subPlatform);

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
            return subPlatform;
        }

        public void DeleteSubPlatform(int subPlatformId)
        {
            Subplatform subplatform = ReadSubPlatform(subPlatformId);
            if (subplatform != null)
            {
                ctx.SubPlatforms.Remove(subplatform);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Subplatform> ReadSubPlatforms()
        {
            return ctx.SubPlatforms.AsEnumerable();
        }

        public Subplatform ReadSubPlatform(int subPlatformId)
        {
            return ctx.SubPlatforms.FirstOrDefault(s => s.SubplatformId == subPlatformId);
        }

        public void UpdateSubPlatform(Subplatform subPlatform)
        {
            ctx.SubPlatforms.Attach(subPlatform);
            ctx.Entry(subPlatform).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
