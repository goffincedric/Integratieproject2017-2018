using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.DAL.EF;
using System;
using PB.BL.Interfaces;


namespace UI_MVC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(IntegratieDbContext.Create);
            app.CreatePerOwinContext<AccountManager>(AccountManager.Create);
            app.CreatePerOwinContext<IntegratieSignInManager>(IntegratieSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromDays(5),
              Provider = new CookieAuthenticationProvider
        {
            // Enables the application to validate the security stamp when the user logs in.
            // This is a security feature which is used when you change a password or add an external login to your account.  
            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<AccountManager, Profile>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        },

                CookieName = "MyAuthCookie",
                CookieHttpOnly = false,
                CookieSecure = CookieSecureOption.Always,
              
                SlidingExpiration = true
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "275c0a2b-44a2-47ab-b336-533c99456f54",
            //    clientSecret: "A73D23F7F39076E9B13D5F4C331DE85714E6E9E2");

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = "isng1J8G5YcEg27yiHk6OyvIR",
                ConsumerSecret = "eWzANxGLJDSxW1E4LTSbkSDu08lkJhEJSAe9Fcfkkp6V4sKXKW",
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[] {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47",
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5",
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133",
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD",
                    "5168FF90AF0207753CCCD9656462A212B859723B",
                    "B13EC36903F8BF4701D498261A0802EF63642BC3"
          })
            });

            app.UseFacebookAuthentication(
               appId: "161271304692485",
                 appSecret: "42ed933110a987c58fb42e7e4164b193");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "140081820355-iqphmt6v1h69pk442f5bc9qqb305our6.apps.googleusercontent.com",
                ClientSecret = "ndCEP2Pz3VlK0WsJKa4KsDmw"
            });


        }
    }
}