using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Globalization;
using System.Threading;


namespace PlizCard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApiConfig.RegisetrApiConfig();
            UserSession.RegisterUserSession();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CultureInfo cInfo = new CultureInfo("en-IN");
            cInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            cInfo.DateTimeFormat.DateSeparator = ".";
            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;
        }

        protected void Sesion_Start(object sender, EventArgs e)
        {
            UserSession.set("s", 1);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session.Clear(); 
            Session.Abandon();
        }
    }
}
