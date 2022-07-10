using SmartMenu.WEB.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartMenu.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.Add(
            //          new SubdomainRoute("m",
            //              new { controller = "Menu", action = "GetMenu", id = UrlParameter.Optional },
            //              namespaces: new[] { "SmartMenu.WEB.Controllers" }));
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "SmartMenu.WEB.Controllers" }
            //);
            //routes.Add("ProductDetails", new SeoFriendlyRoute("Home/Index/{id}",
            //  new RouteValueDictionary(new { controller = "Home", action = "Index" }),
            //  new MvcRouteHandler()));

            routes.Add(new SubdomainRoute());
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "SmartMenu.WEB.Controllers" }
            );

           
            routes.MapRoute(
               name: "Default1",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
