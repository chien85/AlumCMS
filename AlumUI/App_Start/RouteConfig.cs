using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlumUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}",
            defaults: new { controller = "Home", action = "Main" },
            namespaces: new[] { "AlumUI.Controllers" }
        );
            routes.MapRoute(
                  name: "Default2",
                  url: "Home/Pages/{idcategory}/{idsub}",
                  defaults: new { controller = "Home", action = "Pages", page = 1 }
              );

            routes.MapRoute(
                name: "Pages1",
                url: "Home/Pages/{idcategory}/{idsub}/page{page}",
                defaults: new { controller = "Home", action = "Pages" },
                constraints: new { page = @"\d+" }
            );
            routes.MapRoute(
              name: "Cates",
              url: "Home/Cates/{idcategory}",
              defaults: new { controller = "Home", action = "Cates", page = 1 }
          );

            routes.MapRoute(
              name: "PageOnlyCate",
              url: "Home/Cates/{idcategory}/page{page}",
              defaults: new { controller = "Home", action = "Cates", page = 1 },
              constraints: new { page = @"\d+" }
          );

            routes.MapRoute(
            name: "Article",
            url: "Home/Content/{idcontent}/{slug}",
            defaults: new { controller = "Home", action = "Content" }
        );
        }
    }
}
