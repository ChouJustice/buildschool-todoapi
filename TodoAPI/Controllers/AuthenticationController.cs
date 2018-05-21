using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [RoutePrefix("auth")]
    public class AuthenticationController : ApiController
    {
        private string _clientKey = "123456";
        private string _clientSecret = "654321";

        [Route("authorize")]
        [HttpGet]
        public HttpResponseMessage Authorize(
            [FromUri] AuthenticationRequest authRequest)
        {
            if (authRequest.ClientId != _clientKey)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            var oResponse = new JObject();
            oResponse.Add(new JProperty("code", "abcdef"));
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(
                oResponse.ToString(), 
                Encoding.UTF8, 
                "application/json");

            return response;
        }

        [Route("access_token")]
        [HttpGet]
        public HttpResponseMessage ExchangeAccessToken(
            [FromUri] AuthorizationCode authorizationCode)
        {
            if (authorizationCode.Code != "abcdef")
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            if (authorizationCode.ClientSecret != _clientSecret)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            var oResponse = new JObject();
            oResponse.Add(new JProperty("access_token", "dtuf438u09qwhcg9WOhv93hwioeahdv"));
            oResponse.Add(new JProperty("expires_in", 20 * 60));
            oResponse.Add(new JProperty("token_type", "Bearer"));
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(
                oResponse.ToString(),
                Encoding.UTF8,
                "application/json");

            return response;
        }
    }
}
