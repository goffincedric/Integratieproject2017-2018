using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Accounts;

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

        public bool IsValidUserId(string userId)
        {
            return ctx.Users.FirstOrDefault(p => p.UserName.Equals(userId)) is null;
        }
    }
}