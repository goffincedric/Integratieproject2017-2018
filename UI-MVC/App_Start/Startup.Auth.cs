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
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.OAuth;
using UI_MVC.Controllers.API;
using UI_MVC.Controllers.API.Helper_Code;

namespace UI_MVC
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(IntegratieDbContext.Create);
            app.CreatePerOwinContext<AccountManager>(AccountManager.Create);
            app.CreatePerOwinContext<IntegratieSignInManager>(IntegratieSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromDays(5),
                Provider = new CookieAuthenticationProvider
                {
                    OnApplyRedirect = ApplyRedirect,
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
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseFacebookAuthentication(
            appId: "161271304692485",
             appSecret: "42ed933110a987c58fb42e7e4164b193");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "140081820355-iqphmt6v1h69pk442f5bc9qqb305our6.apps.googleusercontent.com",
                ClientSecret = "ndCEP2Pz3VlK0WsJKa4KsDmw"
            });


            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/account/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(5),
                Provider = new AuthorizationServerProvider()
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            UrlHelper _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            String actionUri = _url.Action("Login", "Account", new { returnUrl = context.Request.Uri.PathAndQuery });
            context.Response.Redirect(actionUri);
        }
    }
}