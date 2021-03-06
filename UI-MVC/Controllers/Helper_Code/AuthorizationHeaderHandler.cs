﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UI_MVC.Controllers.Resource.Constants;

namespace UI_MVC.Controllers.API.Helper_Code.Common
{
    /// <summary>
    ///     Authorization for web API class
    /// </summary>
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        #region Send method

        /// <summary>
        ///     Send method
        /// </summary>
        /// <param name="request">Request parameter</param>
        /// <param name="cancellationToken">Cancellation token parameter</param>
        /// <returns>Return HTTP response Task</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Initialization
            HttpResponseMessage responseMessage;

            // Verification
            if (request.Headers.TryGetValues(ApiInfo.API_KEY_HEADER, out IEnumerable<string> apiKeyHeaderValues))
            {
                // Get the API key
                var apiKeyHeaderValue = apiKeyHeaderValues.First();

                // Validate API Key
                if (apiKeyHeaderValue.Equals(ApiInfo.API_KEY_VALUE))
                {
                    // Return Request
                    Task<HttpResponseMessage> response = base.SendAsync(request, cancellationToken);
                    return await response;
                }
            }
            else
            {
                responseMessage = request.CreateResponse(HttpStatusCode.BadRequest);
                return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
            }

            // Info
            responseMessage = request.CreateResponse(HttpStatusCode.BadRequest);
            return await Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage);
        }

        #endregion
    }
}