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
        #region Send method
        /// <summary>   
        /// Send method
        /// </summary>   
        /// <param name="request">Request parameter</param>   
        /// <param name="cancellationToken">Cancellation token parameter</param>   
        /// <returns>Return HTTP response Task</returns>   
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Initialization
            HttpResponseMessage responseMessage;

            // Verification
<<<<<<< HEAD
            if (request.Headers.TryGetValues(ApiInfo.API_KEY_HEADER, out apiKeyHeaderValues) && !string.IsNullOrEmpty(authorization?.Parameter))
=======
            if (request.Headers.TryGetValues(ApiInfo.API_KEY_HEADER, out IEnumerable<string> apiKeyHeaderValues))
>>>>>>> master
            {
                // Get the API key
                var apiKeyHeaderValue = apiKeyHeaderValues.First();

                // Validate API Key
                if (apiKeyHeaderValue.Equals(ApiInfo.API_KEY_VALUE))
                {
                    // Return Request
                    return await base.SendAsync(request, cancellationToken);
                }
            }
            else
            {
                responseMessage = request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
            }
            // Info
            responseMessage = request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
        }
        #endregion
    }
}