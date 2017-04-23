using System.Web.Http;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web-API-Konfiguration und -Dienste

            // Web-API-Routen
            config.MapHttpAttributeRoutes();

            /*            config.Routes.MapHttpRoute(
                            name: "DefaultApi",
                            routeTemplate: "api/{controller}/{action}/{id}",
                            defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
                        );*/
        }
    }
}