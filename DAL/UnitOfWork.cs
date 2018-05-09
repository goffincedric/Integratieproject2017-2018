using PB.DAL.EF;
using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace PB.DAL
{
    public class UnitOfWork
  {

    private IntegratieDbContext ctx;

    internal IntegratieDbContext Context
    {
      get { return ctx ?? (ctx = new IntegratieDbContext(true)); }
    }

        /// <summary>
        /// Deze methode zorgt ervoor dat alle tot hier toe aangepaste domein objecten
        /// worden gepersisteert naar de databank
        /// </summary>
        public int CommitChanges()
        {
            try
            {
                return ctx.CommitChanges();
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
            return -1;
        }

        public async Task<int> CommitChangesAsync()
        {
            return await ctx.CommitChangesAsync();
        }
    }
}
