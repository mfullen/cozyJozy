using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace cozyjozywebapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Filters.Add(new AuthorizeAttribute());

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "ApiWithAction",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "ControllerActionIdApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { },
                constraints: new { id = @"\d+" }
            );
            //

            config.Routes.MapHttpRoute(
                name: "ControllerIdApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { },
                constraints: new { id = @"\d+" }
            );
            //
            config.Routes.MapHttpRoute(
                name: "ControllerActionApi",
                routeTemplate: "api/{controller}/{action}"
            );
            //
            config.Routes.MapHttpRoute(
                name: "ControllerApi",
                routeTemplate: "api/{controller}"
            );
        }
    }
}
