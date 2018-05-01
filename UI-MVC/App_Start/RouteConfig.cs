using System.Web.Mvc;
using System.Web.Routing;

namespace UI_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{subplatform}/{controller}/{action}/{id}",
                defaults: new { subplatform = "politieke-barometer", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
