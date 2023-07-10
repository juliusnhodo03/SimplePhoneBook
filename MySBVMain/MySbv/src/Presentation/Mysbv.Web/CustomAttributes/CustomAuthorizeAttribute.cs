using System.Web;
using System.Web.Mvc;

namespace Mysbv.Web.CustomAttributes
{
    /// <summary>
    /// Custom Attributes
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
       
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            throw new NotauthorizedHttpException(Roles);
        }
    }

    /// <summary>
    /// Handles UnAuthorised Http exception
    /// </summary>
    internal class NotauthorizedHttpException : HttpException
    {

        public NotauthorizedHttpException(string missingRoles)
            : base(401, string.Format("You do not have the required role(s) '{0}'", string.Join(",", missingRoles)))
        {
        }
    }
}