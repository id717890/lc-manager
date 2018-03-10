using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PlizCard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{page}.aspx/{*webmethod}");

            routes.MapRoute(
                name: "Default",
                url: "{action}/{subaction}/{id}",
                defaults: new { controller = "Static", action = "Index",subaction="", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Email",
                url: "email/{client}&{code}",
                defaults: new { controller = "Static", action = "Email", client = UrlParameter.Optional, code = UrlParameter.Optional }
                );
        }
    }
}
