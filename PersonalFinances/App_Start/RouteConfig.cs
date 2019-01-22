using System.Web.Mvc;
using System.Web.Routing;

namespace PersonalFinances
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{Id}",
                defaults: new { controller = "Dashboard", action = "Index", Id = UrlParameter.Optional }
            );
        }
    }
}
