using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TodoAPI
{
    public class TokenValidator : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
                return Task.FromResult(
                   request.CreateResponse(HttpStatusCode.Unauthorized));

            // income authentication.
            if (request.Headers.Authorization.Scheme != "Bearer")
                return Task.FromResult(
                    request.CreateResponse(HttpStatusCode.Unauthorized));
            if (request.Headers.Authorization.Parameter != "dtuf438u09qwhcg9WOhv93hwioeahdv")
                return Task.FromResult(
                    request.CreateResponse(HttpStatusCode.Unauthorized));

            return base.SendAsync(request, cancellationToken);
        }
    }
}