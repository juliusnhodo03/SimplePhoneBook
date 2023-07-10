using System.Web.Mvc;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Home page related functionality
    /// </summary>
    [Authorize]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index Home Page
        /// </summary>
        /// <param name="passwordReset">Reset Password</param>
        /// <returns></returns>
        public ActionResult Index(bool passwordReset = false)
		{
			TempData["IsSubmitted"] = false;
			TempData["CashDepositId"] = 0;

            if (VerifyAuthentication())
            {
                if (passwordReset)
                {
                    ShowMessage("Password was reset successfully.", MessageType.success, "Reset Password");
                    return View();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// About
        /// </summary>s
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        /// <summary>
        /// Contact
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        /// <summary>
        /// Page Not Found
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNotFound()
        {
            ViewBag.Message = "Page Not Found";
            return View();
        }


    }
}