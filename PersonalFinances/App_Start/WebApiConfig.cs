using System.Web.Http;
using System.Web.Routing;

namespace PersonalFinances.App_Start
{
    public class WebApiConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "Default API routing",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}