using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading.Tasks;

using PersonalFinances.App_Start;
using PersonalFinances.Models;
using PersonalFinances.Services;

namespace PersonalFinances
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Error handler (only for unhandled exceptions)
        /// </summary>
        protected void Application_Error ()
        {
            var errorModel = new Error(Request.Browser.Browser, Request.RawUrl, Request.HttpMethod, Request.UserHostAddress
                , Server.GetLastError());
            var errorService = new LoggingService();

            if (!(errorModel.Exception is HttpException))
                errorService.Log(errorModel);
        }
    }
}