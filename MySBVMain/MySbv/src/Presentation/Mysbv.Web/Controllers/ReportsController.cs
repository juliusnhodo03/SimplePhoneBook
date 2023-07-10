using System.Web.Mvc;
using Application.Dto.Report;
using Application.Modules.Common;
using Application.Modules.Reporting;
using Application.Modules.UserAccountValidation;
using MvcReportViewer;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    public class ReportsController : BaseController
	{
		private readonly IReportingValidation _reportingValidation;
        private readonly IUserAccountValidation _userAccountValidation;
        private readonly ILookup _lookup;

        public ReportsController()
        {
            _reportingValidation = LocalUnityResolver.Retrieve<IReportingValidation>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
        }


        #region Reports

        public ActionResult Index()
        {
            ViewBag.IsLoaded = false;
            var user = _userAccountValidation.LoggedOnUser(User.Identity.Name);

            if (user != null)
            {
                InitializeDropDown();
                return View(new ReportDto { UserTypeId = user.UserTypeId });
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Index(ReportDto reportDto)
        {
	        if (reportDto.ReportId < 1)
			{
				ViewBag.IsLoaded = false;
				var user = _userAccountValidation.LoggedOnUser(User.Identity.Name);

				InitializeDropDown();

				ShowMessage("Please select a Report to load!", MessageType.info, "Response");
				return View("Index", new ReportDto { UserTypeId = user.UserTypeId });
	        }

            ViewBag.IsLoaded = true;
            InitializeDropDown();
            ViewBag.UserIdParameter = _userAccountValidation.GetUserId(User.Identity.Name);

			var reportName = _reportingValidation.GetReportName(reportDto.ReportId);
			reportDto.Name = @"/mySBVReports/" + reportName;
			ViewBag.ReportName = reportName;
			
            return View(reportDto);
        }

        public ActionResult DownloadExcel(string reportName)
        {
            return DownloadReport(ReportFormat.Excel, reportName);
        }

        public ActionResult DownloadWord(string reportName)
        {
            return DownloadReport(ReportFormat.Word, reportName);
        }

        public ActionResult DownloadPdf(string reportName)
        {
            return DownloadReport(ReportFormat.PDF, reportName);
        }

        public ActionResult DownloadImage(string reportName)
        {
            return DownloadReport(ReportFormat.Image, reportName);
        }

        private ActionResult DownloadReport(ReportFormat format, string reportName)
        {
            var userId = _userAccountValidation.GetUserId(User.Identity.Name);
            return this.Report(format, reportName, new { Parameter1 = userId });
        }

        #endregion

        #region Helpers

        private void InitializeDropDown()
        {
            ViewData.Add("Reports", new SelectList(_lookup.GetReports(User.Identity.Name).ToDropDownModel(), "Id", "Text"));
        }

        #endregion
    }
}