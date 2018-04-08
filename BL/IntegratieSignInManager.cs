using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PB.BL
{
  public class IntegratieSignInManager: SignInManager<BL.Domain.Account.Profile, string>
  {
    public IntegratieSignInManager(AccountManager accountManager, IAuthenticationManager authenticationManager) : base(accountManager, authenticationManager)
    {

    }

    public static IntegratieSignInManager Create(IdentityFactoryOptions<IntegratieSignInManager> options, IOwinContext context)
    {
      return new IntegratieSignInManager(context.GetUserManager<AccountManager>(), context.Authentication);
    }
  }
  
}
