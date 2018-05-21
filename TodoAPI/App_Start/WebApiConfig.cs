using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TodoAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // enable CORS.
            //config.EnableCors();

            // enable CORS at global.
            var globalCors = new EnableCorsAttribute(
                "*", "*", "GET,POST,PUT,DELETE,PATCH");
            config.EnableCors(globalCors);

            // remove XML formatter.
            var xmlencoder = config
                .Formatters
                .XmlFormatter
                .SupportedMediaTypes
                .FirstOrDefault(
                x => x.MediaType == "application/xml");
            config
                .Formatters
                .XmlFormatter
                .SupportedMediaTypes
                .Remove(xmlencoder);

            // add token validator for all income requests.
            config.MessageHandlers.Add(new TokenValidator());
        }
    }
}
