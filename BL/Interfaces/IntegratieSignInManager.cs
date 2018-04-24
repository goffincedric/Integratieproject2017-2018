using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PB.BL.Domain.Accounts;

namespace PB.BL.Interfaces
{
    //This class is used for the authentication of the user, it uses
    //the SupportCenterUserManager which it gets trough its constructor
    public class IntegratieSignInManager: SignInManager<Profile, string>
  {
    public IntegratieSignInManager(AccountManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
    {

    }

    public static IntegratieSignInManager Create(IdentityFactoryOptions<IntegratieSignInManager> options, IOwinContext context)
    {
      return new IntegratieSignInManager(context.GetUserManager<AccountManager>(), context.Authentication);
    }
  }
  
}
