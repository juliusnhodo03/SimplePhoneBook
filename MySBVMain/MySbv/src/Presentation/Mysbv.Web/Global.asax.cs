using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mysbv.Web
{
    public class MvcApplication : HttpApplication
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LocalUnityConfig.RegisterDependencies();
            Application["IsSendingEmails"] = false;
            SqlDependency.Start(ConnectionString);
        }


        protected void Application_Stop()
        {
            SqlDependency.Stop(ConnectionString);
        }
    }
}