using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using PB.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI_MVC.Controllers.API.Resource.Constants;
using System.Web;
using PB.BL.Interfaces;
using PB.BL.Domain.Accounts;

namespace UI_MVC.Controllers.API.Helper_Code.Common
{
    /// <summary>   
    /// Authorization for web API class
    /// </summary>   
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        private UnitOfWorkManager UowMgr;
        private AccountManager _accountMgr;
        private IntegratieSignInManager _signInManager;


        public AuthorizationHeaderHandler()
        {
            UowMgr = new UnitOfWorkManager();
            AccountMgr = new AccountManager(new PB.DAL.EF.IntegratieUserStore(UowMgr.UnitOfWork));
        }

        public IntegratieSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<IntegratieSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AccountManager AccountMgr
        {
            get
            {
                return _accountMgr ?? HttpContext.Current.GetOwinContext().GetUserManager<AccountManager>();
            }
            private set
            {
                _accountMgr = value;
            }
        }

        #region Send method
        /// <summary>   
        /// Send method
        /// </summary>   
        /// <param name="request">Request parameter</param>   
        /// <param name="cancellationToken">Cancellation token parameter</param>   
        /// <returns>Return HTTP response.</returns>   
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Initialization
            IEnumerable<string> apiKeyHeaderValues = null;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            string userName = null;
            string password = null;
            HttpResponseMessage responseMessage;
            // Verification
            if (request.Headers.TryGetValues(ApiInfo.API_KEY_HEADER, out apiKeyHeaderValues) && !string.IsNullOrEmpty(authorization.Parameter))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();
                // Get the auth token   
                string authToken = authorization.Parameter;
                // Decode the token from BASE64   
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                // Extract username and password from decoded token   
                userName = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);
                // Verification
                var result = await SignInManager.PasswordSignInAsync(userName, password, false, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:
                        // Set current principal to user
                        if (apiKeyHeaderValue.Equals(ApiInfo.API_KEY_VALUE))
                        {
                            var authenicatedPrincipal = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Principal;
                            SetPrincipal(authenicatedPrincipal);
                        }
                        break;
                    case SignInStatus.LockedOut:
                        responseMessage = request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new Exception("User locked out"));
                        return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
                    case SignInStatus.Failure:
                        responseMessage = request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new Exception("Invalid login attempt"));
                        return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
                    default:
                        responseMessage = request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new Exception("Invalid login attempt"));
                        return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
                }
            }
            else
            {
                responseMessage = request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
            }
            // Info
            return await base.SendAsync(request, cancellationToken);
        }
        #endregion

        #region Set principal method
        /// <summary>   
        /// Set principal method
        /// </summary>   
        /// <param name="principal">Principal parameter</param>   
        private static async void SetPrincipal(IPrincipal principal)
        {
            // setting
            Thread.CurrentPrincipal = principal;
            // Verification
            if (HttpContext.Current != null)
            {
                // Setting
                HttpContext.Current.User = principal;
            }
        }
        #endregion
    }
}