using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LC_Manager.Implementation;
using Serilog;

namespace LC_Manager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JwtProvider.Create(ConfigurationManager.AppSettings["issuer"]);
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
        }
    }
}
