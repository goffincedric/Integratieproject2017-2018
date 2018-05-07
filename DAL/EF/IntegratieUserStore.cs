using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace PB.DAL.EF
{
    public class IntegratieUserStore : UserStore<Profile>
    {
        private readonly IntegratieDbContext ctx;
        
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
            return ctx.Roles.ToList();
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
