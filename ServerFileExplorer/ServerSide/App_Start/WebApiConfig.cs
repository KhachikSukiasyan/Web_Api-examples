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

        //static void CreateRootFolder()
        //{
        //    string s = Directory.GetCurrentDirectory();

        //    DirectoryInfo[] projectDirectorysFolders = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetDirectories();
        //    bool contains = false;
        //    foreach (DirectoryInfo item in projectDirectorysFolders)
        //    {
        //        if (item.Name == "root")
        //            contains = true;
        //    }
        //    if (!contains)
        //    {
        //        DirectoryInfo projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
        //        projectDirectory.CreateSubdirectory("root");
        //    }
        //}
    }
}
