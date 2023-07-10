using System.Web.Mvc;
using Mysbv.Web.CustomAttributes;

namespace Mysbv.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DisableCache());
        }
    }
}