using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Account;

namespace PB.DAL.EF
{
  public class IntegratieUserStore : UserStore<Profile>
  {
    private IntegratieDbContext context;

    public IntegratieUserStore(IntegratieDbContext context) : base(context)
    {
      //System.Console.WriteLine("USERSTORE MADE");
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

    public List<Profile> ReadAllProfiles()
    {
      return context.Users.ToList();
    }

    public string ReadUserName(string AccountId)
    {
      Profile user = context.Users.Where(x => x.Id == AccountId).SingleOrDefault();
      return user.UserName;
    }
  }
}
