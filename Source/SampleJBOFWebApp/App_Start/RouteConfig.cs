using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SampleJBOFWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            
            //Order is important when dealing with catch-all routes.
            routes.MapRoute("Premium", "premium/{*url}",
              new { controller = "JBOF", action = "Premium" }
            );

            routes.MapRoute("Error", "{*url}",
              new { controller = "JBOF", action = "Index" }
            );

            //If we don't put this last we can't have catch-all with more than two in depth. Seems buggy.
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}"
            );


        }
    }
}
