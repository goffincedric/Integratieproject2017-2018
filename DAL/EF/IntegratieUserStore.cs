using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PB.DAL.EF
{
  public class IntegratieUserStore : UserStore<BL.Domain.Account.Profile>
  {
    private IntegratieDbContext context;

    public IntegratieUserStore(IntegratieDbContext context) : base(context)
    {
      this.context = context;
    }

    public List<IdentityRole> ReadAllRoles()
    {
      return context.Roles.Where(x => !x.Name.Contains("Admin")).ToList();

    }

    public IntegratieDbContext ReadContext()
    {
      return context;
    }



  }
}
