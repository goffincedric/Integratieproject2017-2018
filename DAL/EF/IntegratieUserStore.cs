using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Account;

namespace PB.DAL.EF
{
    public class IntegratieUserStore : UserStore<Profile>
    {
        private IntegratieDbContext ctx;
        
        public IntegratieUserStore()
        {

        }

        public IntegratieUserStore(IntegratieDbContext context) : base(context)
        {
            //System.Console.WriteLine("USERSTORE MADE");
            ctx = context;
        }

        public IntegratieUserStore(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public List<IdentityRole> ReadAllRoles()
        {
            return ctx.Roles.Where(x => !x.Name.Contains("Admin")).ToList();
        }

        public IntegratieDbContext ReadContext()
        {
            return ctx;
        }

        public List<Profile> ReadAllProfiles()
        {
            return ctx.Users.ToList();
        }
    }
}
