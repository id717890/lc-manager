using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LC_Manager.Implementation;
using Serilog;
using Site.Infrastrucure;

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

            //Первый провайдер (старый вариант, от него нужно избавляться)
            JwtProvider.Create(ConfigurationManager.AppSettings["issuer"]);

            //Второй провайдер (новый вариант, к нему нужно прийти)
            LCManager.JWT.JwtProvider.Create(Config.GetIssuer());

            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
        }
    }
}
