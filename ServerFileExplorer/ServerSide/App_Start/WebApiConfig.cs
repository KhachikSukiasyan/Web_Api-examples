using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.IO;

namespace ServerSide
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{model}",
                defaults: new {
                    model = RouteParameter.Optional          
                });
        }
    }
}
