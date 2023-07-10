using System.Web;
using System.Web.Mvc;

namespace Mysbv.Web.CustomAttributes
{
    public class DisableCache : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}