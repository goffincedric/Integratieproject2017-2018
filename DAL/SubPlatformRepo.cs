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
    public class SubPlatformRepo : ISubPlatformRepo
    {
        private IntegratieDbContext ctx;

        public SubPlatformRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public SubPlatformRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
            //Console.WriteLine("UOW MADE SUBPLATFORMREPO");
        }

        public SubPlatform CreateSubPlatform(SubPlatform subPlatform)
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
            SubPlatform subplatform = ReadSubPlatform(subPlatformId);
            if (subplatform != null)
            {
                ctx.SubPlatforms.Remove(subplatform);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<SubPlatform> ReadSubPlatform()
        {
            return ctx.SubPlatforms.AsEnumerable();
        }

        public SubPlatform ReadSubPlatform(int subPlatformId)
        {
            return ctx.SubPlatforms.FirstOrDefault(s => s.SubplatformId == subPlatformId);
        }

        public void UpdateSubPlatform(SubPlatform subPlatform)
        {
            ctx.SubPlatforms.Attach(subPlatform);
            ctx.Entry(subPlatform).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
