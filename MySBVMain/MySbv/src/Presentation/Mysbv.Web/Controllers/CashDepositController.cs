using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Application.Dto.CashDeposit;
using Application.Modules.CashHandling.CashDepositManager;
using Application.Modules.Common;
using Application.Modules.Maintanance.Site;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Cash deposit Related Functionality
    /// </summary>
    public class CashDepositController : BaseController
    {

        private readonly ICashDepositValidation _cashDepositValidation;
        private readonly IUserAccountValidation _userAccount;
        private readonly ILookup _lookup;
        private readonly ISiteValidation _siteValidation;

        public CashDepositController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _cashDepositValidation = LocalUnityResolver.Retrieve<ICashDepositValidation>();
            _userAccount = LocalUnityResolver.Retrieve<IUserAccountValidation>();
            _siteValidation = LocalUnityResolver.Retrieve<ISiteValidation>();
        }



        #region List CashDeposits

        /// <summary>
        ///     cashDeposit Home Screen
        /// </summary>
        /// <returns>CashDeposit</returns>
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVTellerSupervisor, SBVAdmin")]
        public ActionResult Index()
        {
            ViewBag.IsRetailUser = User.IsInRole("RetailUser").ToString().ToLower();
            ViewBag.IsSBVTeller = User.IsInRole("SBVTeller").ToString().ToLower();
            ViewBag.IsSBVAdmin = User.IsInRole("SBVAdmin").ToString().ToLower();

            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            ViewBag.IsSubmitted = Convert.ToBoolean(TempData["IsSubmitted"]);
            ViewBag.CashDepositId = Convert.ToDecimal(TempData["CashDepositId"]);

            IEnumerable<ListCashDepositDto> result = _cashDepositValidation.All(user);

            return View(result);
        }

        #endregion

        #region Create CashDeposit

        public async Task<ActionResult> Add()
        {
            var user = await _userAccount.GetLoggedUserAsync("juliush");
            
            ViewBag.Container = new ContainerDto();
            ViewBag.ContainerDrop = new ContainerDropDto();
            ViewBag.ContainerDropItem = new ContainerDropItemDto();
            ViewBag.NotesCollection = await _lookup.GetDenominationsAsync("Notes");
            ViewBag.CoinsCollection = await _lookup.GetDenominationsAsync("Coins");

            ViewBag.CashDeposit = new CashDepositDto
            {
                IsSbvTeller = user.UserType.LookUpKey.ToUpper() == "SBV_USER",
                IsHeadOfficeUser = user.UserType.LookUpKey.ToUpper() == "HEAD_OFFICE_USER",
                IsMerchantUser = user.UserType.LookUpKey.ToUpper() == "MERCHANT_USER",
                CaptureDateString = DateTime.Now.ToShortDateString()
            };
            
            return View();
        }


        /// <summary>
        ///     Validate and  Create cashDeposit
        /// </summary>
        /// <returns>CashDeposit</returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Create()
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            Merchant merchant = _lookup.GetMerchantByUsername(user);

            var cashDeposit = new CashDepositDto
            {
                IsSbvTeller = user.UserType.LookUpKey == "SBV_USER",
                IsHeadOfficeUser = user.UserType.LookUpKey == "HEAD_OFFICE_USER",
                CreateDate = DateTime.Now,
                CreatedById = user.UserId,
                CaptureDateString = DateTime.Now.ToShortDateString()
            };

            ViewBag.UserMerchantId = user.MerchantId;

            if (user.UserType.LookUpKey == "MERCHANT_USER")
            {
                Site site = _lookup.GetMerchantUserSiteByUsername(user);

                ViewData.Add("Sites",
                    new SelectList(user.UserSites.Select(e => e.Site)
                        .ToSitesDropDownModel()
                        .OrderBy(e => e.Name), "Id", "Name"));

                ViewData.Add("Merchants",
                    new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name", user.MerchantId));

                ViewData.Add("ReferenceNumberComboBox",
                    new SelectList(_lookup.GetSiteDepositReference(0).ToDepositReferenceDropDownModel(), "Id", "Name"));

                cashDeposit.MerchantId = merchant.MerchantId;
                cashDeposit.ContractNumber = merchant.ContractNumber;
                cashDeposit.SiteId = site.SiteId;
            }
            else if (user.UserType.LookUpKey == "SBV_USER")
            {
                ViewData.Add("Sites",
                    new SelectList(_lookup.Sites().Where(o => o.SiteId == 0).ToSitesDropDownModel()
                        .OrderBy(e => e.Name), "Id", "Name"));

                ViewData.Add("ReferenceNumberComboBox",
                    new SelectList(_lookup.GetSiteDepositReference(0)
                        .ToDepositReferenceDropDownModel(), "Id", "Name"));
            }
            else if (user.UserType.LookUpKey == "HEAD_OFFICE_USER")
            {
                ViewData.Add("Merchants", new SelectList(_lookup.Merchants()
                    .ToDropDownModel()
                    .OrderBy(e => e.Name), "Id", "Name"));

                ViewData.Add("Sites", new SelectList(_lookup.Sites()
                    .ToSitesDropDownModel(), "Id", "Name"));

                ViewData.Add("ReferenceNumberComboBox",
                    new SelectList(_lookup.GetSiteDepositReference(0).ToDepositReferenceDropDownModel(), "Id", "Name"));
            }

            ViewData.Add("ContainerTypes",
                new SelectList(_lookup.GetSiteContainers(0).ToDropDownModel(), "Id", "Name", 0));

            ViewData.Add("dropDownListNote",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name"));

            ViewData.Add("dropDownListCoin",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name"));

            ViewData.Add("DepositTypes",
                new SelectList(_lookup.GetDepositTypes().ToDropDownModel().RemoveFirst(), "Id", "Name"));

            ViewData.Add("Banks", new SelectList(_lookup.GetBanksServicedBySite(0).ToDropDownModel(), "Id", "Name"));

            ViewData.Add("SiteSettlementAccounts",
                new SelectList(_lookup.GetSiteSettlementAccounts(0, 0).ToAccountsDropDownModel(), "Id", "Name"));
            ViewData.Add("emptyDropDown",
                new SelectList(_lookup.GetSiteSettlementAccounts(0, 0).ToAccountsDropDownModel(), "Id", "Name"));

            return View(cashDeposit);
        }


        // GET: /CashDeposit/Create

        /// <summary>
        ///     Create CashDeposit
        /// </summary>
        /// <param name="cashDepositDto">Represent cashDeposit</param>
        /// <returns>cashDeposit</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(CashDepositDto cashDepositDto)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<CashDeposit> result = _cashDepositValidation.Add(cashDepositDto, user);

            if (result.Status != MethodStatus.Successful)
                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message
                        }, JsonRequestBehavior.AllowGet
                    );

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        result.Message,
                        CashDeposit = result.EntityResult
                    }, JsonRequestBehavior.AllowGet
                );
        }

        #endregion

        #region Edit CashDeposit

        /// <summary>
        ///     Edit Cashdeposit by CashDepositId
        /// </summary>
        /// <param name="id">represent cashDepositId</param>
        /// <returns>CashDeposit</returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Edit(int id = 0)
        {
            CashDepositDto result = _cashDepositValidation.Find(id);

            //result.EntityResult.CaptureDateString = ""; //result.EntityResult.CreateDate.Value.ToShortDateString();

            if (result.IsSubmitted == true)
            {
                ShowMessage("You cannot Edit a " + result.Status.Name + " transaction.", MessageType.error,
                    "Operation Error");
                return RedirectToAction("Index");
            }

            SelectedDropDowns();

            ViewData.Add("DepositTypes",
                new SelectList(_lookup.GetDepositTypes().ToDropDownModel().RemoveFirst(), "Id", "Name",
                    result.DepositTypeId));

            if (result.Narrative != null)
            {
                if (Equals(result.Site.DepositReference.Trim().ToLower(), result.Narrative.Trim().ToLower()))
                {
                    ViewData.Add("ReferenceNumberComboBox",
                        new SelectList(
                            _lookup.GetSiteDepositReference(result.SiteId).ToDepositReferenceDropDownModel(), "Id",
                            "Name", result.SiteId));
                }
            }
            else
            {
                ViewData.Add("ReferenceNumberComboBox",
                    new SelectList(_lookup.GetSiteDepositReference(result.SiteId).ToDepositReferenceDropDownModel(),
                        "Id", "Name", 0));
            }

            ViewData.Add("Sites", new SelectList(_lookup.Sites().ToSitesDropDownModel(), "Id", "Name", result.SiteId));
            foreach (ContainerDto container in result.Containers)
            {
                ViewData.Add("ContainerType" + container.ContainerId,
                    new SelectList(_lookup.GetSiteContainers(result.SiteId).ToDropDownModel(), "Id", "Name",
                        container.ContainerTypeId));
            }

            ViewData.Add("ContainerTypes",
                new SelectList(_lookup.GetSiteContainers(result.SiteId).ToDropDownModel(), "Id", "Name", 0));

            ViewData.Add("Banks",
                new SelectList(_lookup.GetBanksServicedBySite(result.SiteId).ToDropDownModel(), "Id", "Name",
                    result.BankId));

            ViewData.Add("SiteSettlementAccounts",
                new SelectList(
                    _lookup.GetSiteSettlementAccounts(result.BankId, result.SiteId).ToAccountsDropDownModel(), "Id",
                    "Name", result.AccountId));

            ViewData.Add("emptyDropDown",
                new SelectList(_lookup.GetSiteSettlementAccounts(0, 0).ToAccountsDropDownModel(), "Id", "Name"));

            result.InitialCitSerialNumber = _lookup.GetCitInitialDigit(result.SiteId).ToString();

            ViewData.Add("Merchants",
                new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name", result.Site.MerchantId));

            return View(result);
        }


        //[HttpPost]

        /// <summary>
        ///     Edit,Validate ,Save CashDeposit
        /// </summary>
        /// <param name="cashDepositDto">represent CashDeposit</param>
        /// <returns>CashDeposit Home Screen</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Edit(CashDepositDto cashDepositDto)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<CashDeposit> result = _cashDepositValidation.Edit(cashDepositDto, user);
            if (result.Status == MethodStatus.Successful)
            {
                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message,
                            CashDeposit = result.EntityResult
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        result.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }

        #endregion

        #region Copy CashDeposit

        /// <summary>
        ///     CopycashDeposit by cashDepositId
        /// </summary>
        /// <param name="id">Represent CashDepositId</param>
        /// <returns>CashDeposit</returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Copy(int id = 0)
        {
            CashDepositDto result = _cashDepositValidation.Find(id);
            result.CaptureDateString = DateTime.Now.ToShortDateString();

            //result.EntityResult.CaptureDateString = ""; //result.EntityResult.CreateDate.Value.ToShortDateString();

            if (result.IsSubmitted == true)
            {
                ShowMessage("You cannot Edit a " + result.Status.Name + " transaction.", MessageType.error,
                    "Operation Error");
                return RedirectToAction("Index");
            }

            SelectedDropDowns();

            ViewData.Add("DepositTypes",
                new SelectList(_lookup.GetDepositTypes().ToDropDownModel().RemoveFirst(), "Id", "Name",
                    result.DepositTypeId));

            if (result.Narrative != null)
            {
                if (Equals(result.Site.DepositReference.Trim().ToLower(), result.Narrative.Trim().ToLower()))
                {
                    ViewData.Add("ReferenceNumberComboBox",
                        new SelectList(
                            _lookup.GetSiteDepositReference(result.SiteId).ToDepositReferenceDropDownModel(), "Id",
                            "Name", result.SiteId));
                }
            }
            else
            {
                ViewData.Add("ReferenceNumberComboBox",
                    new SelectList(_lookup.GetSiteDepositReference(result.SiteId).ToDepositReferenceDropDownModel(),
                        "Id", "Name", 0));
            }

            ViewData.Add("Sites", new SelectList(_lookup.Sites().ToSitesDropDownModel(), "Id", "Name", result.SiteId));
            foreach (ContainerDto container in result.Containers)
            {
                ViewData.Add("ContainerType" + container.ContainerId,
                    new SelectList(_lookup.GetSiteContainers(result.SiteId).ToDropDownModel(), "Id", "Name",
                        container.ContainerTypeId));
            }

            ViewData.Add("ContainerTypes",
                new SelectList(_lookup.GetSiteContainers(result.SiteId).ToDropDownModel(), "Id", "Name", 0));
            ViewData.Add("Merchants",
                new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name", result.MerchantId));

            ViewData.Add("Banks",
                new SelectList(_lookup.GetBanksServicedBySite(result.SiteId).ToDropDownModel(), "Id", "Name",
                    result.BankId));

            ViewData.Add("SiteSettlementAccounts",
                new SelectList(
                    _lookup.GetSiteSettlementAccounts(result.BankId, result.SiteId).ToAccountsDropDownModel(), "Id",
                    "Name", result.AccountId));

            ViewData.Add("emptyDropDown",
                new SelectList(_lookup.GetSiteSettlementAccounts(0, 0).ToAccountsDropDownModel(), "Id", "Name"));

            result.InitialCitSerialNumber = _lookup.GetCitInitialDigit(result.SiteId).ToString();

            return View(result);
        }


        //[HttpPost]
        /// <summary>
        ///     Copy CashDeposit
        /// </summary>
        /// <param name="cashDepositDto">Represent CashDeposit</param>
        /// <returns>Cashdeposit</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Copy(CashDepositDto cashDepositDto)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<CashDeposit> result = null;

            result = cashDepositDto.CashDepositId > 0
                ? _cashDepositValidation.Edit(cashDepositDto, user)
                : _cashDepositValidation.Add(cashDepositDto, user);

            if (result.Status == MethodStatus.Successful)
            {
                CashDepositDto cashDeposit = _cashDepositValidation.Find(result.EntityResult.CashDepositId);

                if (cashDepositDto.TransactionStatusName == "Submitted")
                {
                    ShowMessage("Cash Deposit Submitted successfully.", MessageType.success, "Cash Deposit");
                }

                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message,
                            CashDeposit = result.EntityResult
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        result.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }

        #endregion

        #region View CashDeposit

        /// <summary>
        ///     Cash Deposit View
        /// </summary>
        /// <param name="id">Represent CashDepositId</param>
        /// <returns>CashDeposit</returns>
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVTellerSupervisor, SBVAdmin")]
        public ActionResult View(int id)
        {
            CashDepositDto result = _cashDepositValidation.Find(id);
            ViewData.Add("Sites", new SelectList(_lookup.Sites().ToSitesDropDownModel(), "Id", "Name", result.SiteId));
            ViewData.Add("Merchants",
                new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name", result.Site.MerchantId));

            return result != null ? View(result) : View(new CashDepositDto());
        }

        #endregion

        /// <summary>
        ///     Cancel CashDeposit
        /// </summary>
        /// <param name="id">Represent CashDepositId</param>
        /// <returns>Cancel CashDeposit</returns>

        #region Delete CashDeposit
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Cancel(int id)
        {
            //var result = _cashDepositValidation.Find(id);
            ViewBag.HasTriedToEditSubmittedDeposit = "";
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            //if (result.EntityResult.IsSubmitted == true)
            //{
            //    ShowMessage("You cannot Delete a " + result.EntityResult.Status.StatusName + " transaction.", MessageType.error, "Operation Error");
            //    return Json(new { url = Url.Action("Index") });
            //}

            MethodResult<bool> deleteResult = _cashDepositValidation.Delete(id, user);
            if (deleteResult.Status == MethodStatus.Successful)
            {
                ShowMessage(deleteResult.Message, MessageType.success, "Delete Cash Deposit");
            }
            else
            {
                ShowMessage(deleteResult.Message,
                    deleteResult.Status == MethodStatus.Error ? MessageType.error : MessageType.warning,
                    "Delete Cash Deposit");
            }
            return Json(new {url = Url.Action("Index")});
        }

        #endregion

        /// <summary>
        ///     Cancel   Container Drop
        /// </summary>
        /// <param name="id">Represent ContainerDrop Id</param>
        /// <returns>Cancel   Container Drop</returns>

        #region Delete Container Drop
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult CancelDrop(int id)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            MethodResult<bool> result = _cashDepositValidation.DeleteDrop(id, user);

            if (result.Status == MethodStatus.Successful)
            {
                ShowMessage(result.Message, MessageType.success, "Delete Drop");
                return Json(new {url = Url.Action("Index")});
            }

            if (result.Status == MethodStatus.Warning)
            {
                ShowMessage(result.Message, MessageType.warning, "Delete Drop");
                return Json(new {url = ""});
            }

            ShowMessage(result.Message, MessageType.error, "Delete Drop");
            return Json(new {url = ""});
        }

        #endregion

        /// <summary>
        ///     Submit CashDeposit
        /// </summary>
        /// <param name="id">Represent CashDepositId</param>
        /// <returns>submit CashDeposit andRedirect to CashDeposit Home screen</returns>

        #region Submit CashDeposit
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVAdmin")]
        public ActionResult Submit(int id = 0)
        {
            CashDepositDto cashDeposit = _cashDepositValidation.Find(id);
            string dropDescription = cashDeposit.DepositType.Name == "Multi Drop" ? "Drops" : "Deposits";

            TempData["IsSubmitted"] = false;
            TempData["CashDepositId"] = id;

            var dropCount = cashDeposit.Containers.Select(e => e.ContainerDrops.Count());

            // Changed by Julius on 26-08-2015
            // This helps when the Teller decides to cancel a deposit with at least one
            // drop/deposit has been submitted. It sets right the correct submitted amount.
            cashDeposit.DepositedAmount = cashDeposit.Containers.Sum(e => e.Amount);
            
            if (dropCount.FirstOrDefault() == 0)
            {
                ShowMessage("Cannot submit a Cash deposit deposit without " + dropDescription, MessageType.error,
                    "Submit Failed");
                return RedirectToAction("Index");
            }

            if (cashDeposit.DepositedAmount <= 0)
            {
                ShowMessage("Cannot submit a Cash deposit deposit with a Zero or Non positive value", MessageType.error,
                    "Submit Failed");
                return RedirectToAction("Index");
            }

            foreach (ContainerDto container in cashDeposit.Containers)
            {
                foreach (ContainerDropDto containerDrop in container.ContainerDrops)
                {
                    if (containerDrop.Name == "SUBMITTED" || cashDeposit.DepositType.Name == "Single Deposit") continue;
                    ShowMessage("Please Submit all " + dropDescription + " first.", MessageType.warning, "Submit Failed");
                    return RedirectToAction("Index");
                }
            }

            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            bool isSubmitted = _cashDepositValidation.IsSubmitted(id);

            if (isSubmitted == false && cashDeposit != null)
            {
                MethodResult<CashDeposit> result = _cashDepositValidation.Submit(cashDeposit, user);

                if (result.Status == MethodStatus.Successful)
                {
                    GenerateCashDepositSlips(result.EntityResult.CashDepositId);
                    TempData["IsSubmitted"] = true;
                    ViewBag.CashDepositId = result.EntityResult;
                    ShowMessage("Cash Deposit Submitted successfully.", MessageType.success, "Cash Deposit");
                    return RedirectToAction("Index");
                }
            }
            else if (isSubmitted)
            {
                GenerateCashDepositSlips(cashDeposit.CashDepositId);
                ShowMessage("Cash Deposit Submitted successfully.", MessageType.success, "Cash Deposit");
                TempData["IsSubmitted"] = true;
                return RedirectToAction("Index");
            }

            ShowMessage("Failed to Submit Cash Deposit!", MessageType.error, "Cash Deposit");
            return RedirectToAction("Index");
        }

        // GET: /CashDeposit/Submit
        /// <summary>
        /// </summary>
        /// <param name="cashDepositDto"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Submit(CashDepositDto cashDepositDto)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<CashDeposit> result = _cashDepositValidation.Submit(cashDepositDto, user);

            if (result.Status == MethodStatus.Successful)
            {
                if (cashDepositDto.TransactionStatusName == "SUBMITTED")
                {
                    ShowMessage("Cash Deposit Submitted successfully.", MessageType.success, "Cash Deposit");
                }

                CashDeposit cashDeposit = result.EntityResult;

                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message,
                            CashDeposit = cashDeposit
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        result.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }

        #endregion

        /// <summary>
        ///     Get DepositSlip in PDF
        /// </summary>
        /// <param name="cashDepositSlipId">Is a CashDeposit Id</param>
        /// <returns>Return CashDeposit Slip in PDF</returns>

        #region Deposit Slip in PDF
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVAdmin")]
        public ActionResult DepositSlip(int cashDepositSlipId)
        {
            CashDepositDto cashDeposit = _cashDepositValidation.Find(cashDepositSlipId);

            // Attach Deposit Slip
            string reportPath = (cashDeposit.DepositType.Name == "Single Deposit")
                ? "/CashDeposit/CashDepositSlip"
                : "/CashDeposit/DropSlip";

            var stream = new MemoryStream(PdfGeneration(cashDepositSlipId, reportPath));
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        /// <summary>
        ///     Cancel Cash Order
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Delete()
        {
            return View(new List<ListCashDepositDto>());
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cdid"></param>
        /// <returns></returns>
        public ActionResult EachDropOrDepositAsPdf(int id, int cdid)
        {
            // Attach CashDeposit Slip
            const string reportPath = "/CashDeposit/MultiDepositAsSingleDepositSlip";

            byte[] reportAttachmentBytes = ReportParameters(id, cdid, reportPath);
            var stream = new MemoryStream(reportAttachmentBytes);

            return new FileStreamResult(stream, "application/pdf");
        }


        /// <summary>
        ///     Get TransactionSummary in PDF
        /// </summary>
        /// <param name="transactionSummaryId">Is a CashDeposit Id</param>
        /// <returns>Return TransactionSummary in PDF</returns>

        #region TransactionSummary in PDF
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVAdmin")]
        public ActionResult TransactionSummary(int transactionSummaryId)
        {
            // AttachTransactionSummary
            const string reportPath = "/CashDeposit/TransactionSummary";

            var stream = new MemoryStream(PdfGeneration(transactionSummaryId, reportPath));
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        /// <summary>
        ///     Generate CashdepositSlips
        /// </summary>
        /// <param name="id">represent cashdepositId</param>
        /// <returns>GenerateCashDepositSlip</returns>
        private ActionResult GenerateCashDepositSlips(int id)
        {
            // Cash deposit Creator (or user)
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            //
            // Get CashDeposit Details
            CashDepositDto cashDeposit = _cashDepositValidation.Find(id);

            var message = new MailMessage();

            // Attach Deposit Slip
            string depositTypeName = cashDeposit.DepositType.Name;

            string reportPath = (depositTypeName == "Single Deposit")
                ? "/CashDeposit/CashDepositSlip"
                : "/CashDeposit/DropSlip";

            byte[] reportAttachmentBytes = PdfGeneration(id, reportPath);
            var ms = new MemoryStream(reportAttachmentBytes);
            var attachement = new Attachment(ms, "CashDepositSlip.pdf");
            message.Attachments.Add(attachement);

            // Attach Transaction Summary Slip
            reportPath = "/CashDeposit/TransactionSummary";
            reportAttachmentBytes = PdfGeneration(id, reportPath);
            ms = new MemoryStream(reportAttachmentBytes);
            attachement = new Attachment(ms, "TransactionSummary.pdf");
            message.Attachments.Add(attachement);

            // Send Deposit Slips via Email.
            GenerateAndSendEmail(message, cashDeposit, user);
            ShowMessage("An Email with Cash deposit details was sent to your mailbox.", MessageType.success, "Email.");

            return View("Index");
        }

        /// <summary>
        ///     GenerateCashDepositSlip AndSendEmail to User
        /// </summary>
        /// <param name="attachments">User CashDepositSlip attachement</param>
        /// <param name="cashDeposit">User CashDeposit</param>
        /// <param name="user">Current User Requesting CashDepositSlip</param>
        private void GenerateAndSendEmail(MailMessage attachments, CashDepositDto cashDeposit, User user)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv Cash Deposit");
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.Hr);
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("You successfully captured a Cash Deposit:");
            html.WriteBreak();
            html.WriteEncodedText(
                string.Format(
                    "Transaction Reference Number : {0}\nTotal Deposit amount : {1}\nDeposit Type : {2}\nDate captured : {3}",
                    cashDeposit.TransactionReference,
                    cashDeposit.DepositedAmount,
                    cashDeposit.DepositType.Name,
                    DateTime.Now));
            html.WriteBreak();
            html.WriteEncodedText("Use the Transaction Reference Number to track the Deposit.");
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();
            string subject = cashDeposit.DepositType.Name + "_RefNo_" +
                             cashDeposit.TransactionReference + "_" + cashDeposit.SubmitDateTime;
            SendEmailNotification(user.EmailAddress, attachments, subject, htmlString);
        }

        /// <summary>
        ///     Generate CashDepositSlip to Pdf
        /// </summary>
        /// <param name="id">Represent CashDepositId</param>
        /// <param name="reportPath">Is a Report Path</param>
        /// <returns>CashDepsitSlip in Pdf</returns>
        public byte[] PdfGeneration(long id, string reportPath)
        {
            var paramz = new Dictionary<string, string>();
            paramz.Add("CashDepositId", id.ToString());
            byte[] result = GenerateReport(reportPath, paramz);
            return result;
        }

        /// <summary>
        ///     Get Cash Deposit
        /// </summary>
        /// <param name="cashDepositId">is a cashDeposit Id</param>
        /// <returns>Return cashDeposit</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Get(int cashDepositId)
        {
            CashDepositDto cashDeposit = _cashDepositValidation.Find(cashDepositId);

            cashDeposit.CaptureDateString = cashDeposit.CreateDate.Value.ToShortDateString();

            Merchant merchant = _lookup.GetMerchantBySiteId(cashDeposit.SiteId);
            cashDeposit.MerchantName = merchant.Name;
            cashDeposit.ContractNumber = merchant.ContractNumber;
            cashDeposit.MerchantId = merchant.MerchantId;

            cashDeposit.DepositType = null;
            cashDeposit.Status = null;
            cashDeposit.Site = null;

            JsonResult cashDepositDto = Json(cashDeposit);
            return cashDepositDto;
        }


        /// <summary>
        ///     Get CashDepositCopy
        /// </summary>
        /// <param name="cashDepositId">is a cashDeposit Id</param>
        /// <returns>Return cashDepositCopy</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCopy(int cashDepositId)
        {
            CashDepositDto cashDeposit = _cashDepositValidation.CashDepositInitializer(cashDepositId);

            cashDeposit.CaptureDateString = DateTime.Now.ToShortDateString();
            cashDeposit.CreateDate = DateTime.Now;

            Merchant merchant = _lookup.GetMerchantBySiteId(cashDeposit.SiteId);
            cashDeposit.MerchantName = merchant.Name;
            cashDeposit.ContractNumber = merchant.ContractNumber;
            cashDeposit.MerchantId = merchant.MerchantId;

            cashDeposit.DepositType = null;
            cashDeposit.Status = null;

            JsonResult cashDepositDto = Json(cashDeposit);
            return cashDepositDto;
        }

        /// <summary>
        ///     Submit Drop in a container with other DROPS submitted
        /// </summary>
        /// <param name="containerDrop">is a ContainerDrop</param>
        /// <param name="containerAmount">is a containerAmount</param>
        /// <param name="description">is a Container description</param>
        /// <returns>SubmitDrop</returns>

        #region Submit Drop in a container with other DROPS submitted
        public JsonResult SubmitDrop(ContainerDropDto containerDrop, decimal containerAmount, string description)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<ContainerDrop> result = _cashDepositValidation.SubmitDrop(containerDrop, containerAmount,
                description, user);

            if (result.Status == MethodStatus.Error)
            {
                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            string bagReferenceNumber = result.EntityResult.Container.ReferenceNumber;

            result.EntityResult.Container = null;

            foreach (ContainerDropItem item in result.EntityResult.ContainerDropItems)
            {
                item.ContainerDrop = null;
                item.Denomination = null;
            }

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        result.Message,
                        BagReferenceNumber = bagReferenceNumber,
                        ContainerDrop = result.EntityResult
                    }, JsonRequestBehavior.AllowGet
                );
        }

        /// <summary>
        ///     Submit a Drop
        /// </summary>
        /// <param name="containerDropId">is a containerDropId </param>
        /// <returns>Submit Drop</returns>
        [HttpPost]
        public JsonResult SubmitDropOnPostReturn(int containerDropId)
        {
            Container container = _cashDepositValidation.GetContainerByDropId(containerDropId);

            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            GenerateDropSlipFropIds(container, containerDropId, user);

            JsonResult cashDepositDto = Json(string.Empty);
            return cashDepositDto;
        }

        #endregion

        /// <summary>
        ///     Drop submission Report Generation
        /// </summary>
        /// <param name="container">is a Container</param>
        /// <param name="containerDropId">is a containerDropId</param>
        /// <param name="user">is a User</param>


        private void GenerateDropSlipFropIds(Container container, int containerDropId, User user)
        {
            string depositTypeName = _cashDepositValidation.GetDepositType(container.CashDepositId);

            string slipName = (depositTypeName == "Multi Deposit") ? "DepositSlip.pdf" : "DropSlip.pdf";
            var message = new MailMessage();

            // Attach CashDeposit Slip
            const string reportPath = "/CashDeposit/MultiDepositAsSingleDepositSlip";

            byte[] reportAttachmentBytes = ReportParameters(container.CashDepositId, containerDropId, reportPath);
            var ms = new MemoryStream(reportAttachmentBytes);
            var attachement = new Attachment(ms, slipName);
            message.Attachments.Add(attachement);

            CashDeposit cashDeposit = _cashDepositValidation.FindUnMappedDeposit(container.CashDepositId);

            // Send CashDeposit Slip via Email.
            GenerateDropSlipAndSendEmail(message, cashDeposit, user);
            ShowMessage("An Email with Cash deposit details was sent to your mailbox.", MessageType.success, "Email.");
        }



        #region Reprint Deposit in PDF

        /// <summary>
        /// Reprint deposit slip
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dt"></param>
        //[CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        [AllowAnonymous]
        public ActionResult ReprintDeposit(int id, int dt)  
        {
            try
            {
                if (dt == 0)
                {
                    var containerDropId = id;

                    var container = _cashDepositValidation.GetContainerByDropId(id);

                    // Attach CashDeposit Slip
                    const string reportPath = "/CashDeposit/MultiDepositAsSingleDepositSlip";

                    byte[] bytes = ReportParameters(container.CashDepositId, containerDropId, reportPath);

                    var stream = new MemoryStream(bytes);
                    return new FileStreamResult(stream, "application/pdf");
                }
                else
                {
                    // Get CashDeposit Details
                    CashDepositDto cashDeposit = _cashDepositValidation.Find(id);

                    // Attach Deposit Slip
                    string depositTypeName = cashDeposit.DepositType.Name;

                    string reportPath = (depositTypeName == "Single Deposit")
                        ? "/CashDeposit/CashDepositSlip"
                        : "/CashDeposit/DropSlip";

                    byte[] bytes = PdfGeneration(id, reportPath); 

                    var stream = new MemoryStream(bytes);
                    return new FileStreamResult(stream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        /// <summary>
        /// Reprint deposits
        /// </summary>
        /// <param name="id"></param>
        /// <param name="drp"></param>
        //[CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        [AllowAnonymous]
        public FileResult ReprintFileDownload(int id, int drp)
        {
            try
            {
                if (drp == 1)
                {
                    var containerDropId = id;

                    var container = _cashDepositValidation.GetContainerByDropId(id);
                    string depositTypeName = _cashDepositValidation.GetDepositType(container.CashDepositId);

                    string slipName = (depositTypeName == "Multi Deposit") ? "DepositSlip.pdf" : "DropSlip.pdf";

                    // Attach CashDeposit Slip
                    const string reportPath = "/CashDeposit/MultiDepositAsSingleDepositSlip";

                    byte[] bytes = ReportParameters(container.CashDepositId, containerDropId, reportPath);

                    // send the PDF file to browser
                    FileResult fileResult = new FileContentResult(bytes, "application/pdf");
                    fileResult.FileDownloadName = slipName;

                    return fileResult;
                }
                else
                {
                    // Get CashDeposit Details
                    CashDepositDto cashDeposit = _cashDepositValidation.Find(id);

                    // Attach Deposit Slip
                    string depositTypeName = cashDeposit.DepositType.Name;

                    string reportPath = (depositTypeName == "Single Deposit")
                        ? "/CashDeposit/CashDepositSlip"
                        : "/CashDeposit/DropSlip";

                    byte[] reportAttachmentBytes = PdfGeneration(id, reportPath);

                    // send the PDF file to browser
                    FileResult fileResult = new FileContentResult(reportAttachmentBytes, "application/pdf");
                    fileResult.FileDownloadName = "CashDepositSlip.pdf";

                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        ///     Report
        /// </summary>
        /// <param name="cashDepositId">is cashDepositId</param>
        /// <param name="containerDropId">is a containerDropId</param>
        /// <param name="reportPath">is a reportPath</param>
        /// <returns></returns>
        private byte[] ReportParameters(int cashDepositId, int containerDropId, string reportPath)
        {
            var paramz = new Dictionary<string, string>();

            paramz.Add("CashDepositId", cashDepositId.ToString());
            paramz.Add("ContainerDropId", containerDropId.ToString());

            byte[] result = GenerateReport(reportPath, paramz);
            return result;
        }

        /// <summary>
        ///     Generate Drop Slip And Send Email
        /// </summary>
        /// <param name="attachments">is CashdepositSlip attachement</param>
        /// <param name="cashDeposit">is a Cash deposit</param>
        /// <param name="user">Is a User</param>
        private void GenerateDropSlipAndSendEmail(MailMessage attachments, CashDeposit cashDeposit, User user)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv Cash Deposit");
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.Hr);
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("You successfully captured a Cash Deposit:");
            html.WriteBreak();
            html.WriteEncodedText(
                string.Format(
                    "Transaction Reference Number : {0}\nDeposit Type : {1}\nDate captured : {2}",
                    cashDeposit.TransactionReference,
                    cashDeposit.DepositTypeName,
                    DateTime.Now));
            html.WriteBreak();
            html.WriteEncodedText("Use the Transaction Reference Number to track the Deposit.");
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();
            string subject = cashDeposit.DepositTypeName + "_RefNo_" +
                             cashDeposit.TransactionReference + "_" + cashDeposit.SubmitDateTime;
            SendEmailNotification(user.EmailAddress, attachments, subject, htmlString);
        }

        #region Static Data

        /// <summary>
        ///     Selected Drop Downs
        /// </summary>
        private void SelectedDropDowns()
        {
            ViewData.Add("20000", _lookup.GetDenominations("Notes"));
            ViewData.Add("10000", _lookup.GetDenominations("Notes"));
            ViewData.Add("5000", _lookup.GetDenominations("Notes"));
            ViewData.Add("2000", _lookup.GetDenominations("Notes"));
            ViewData.Add("1000", _lookup.GetDenominations("Notes"));

            ViewData.Add("500", _lookup.GetDenominations("Coins"));
            ViewData.Add("200", _lookup.GetDenominations("Coins"));
            ViewData.Add("100", _lookup.GetDenominations("Coins"));
            ViewData.Add("50", _lookup.GetDenominations("Coins"));
            ViewData.Add("20", _lookup.GetDenominations("Coins"));
            ViewData.Add("10", _lookup.GetDenominations("Coins"));
            ViewData.Add("5", _lookup.GetDenominations("Coins"));

            ViewData.Add("dropDownListNote",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name"));
            ViewData.Add("dropDownListCoin",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name"));
        }

        #endregion

        #region KendoGrid Grid on List CashDeposit

        /// <summary>
        ///     ContainerDrops
        /// </summary>
        /// <param name="id">is a Containerid</param>
        /// <param name="request">is a request </param>
        /// <returns></returns>
        public ActionResult ContainerDrops(int id, [DataSourceRequest] DataSourceRequest request)
        {
            MethodResult<IEnumerable<ContainerDropDto>> result = _cashDepositValidation.GetContainerDrops(id);

            return Json(result.EntityResult.ToDataSourceResult(request));
        }

        /// <summary>
        ///     Container
        /// </summary>
        /// <param name="id">ContainerId</param>
        /// <param name="request">is a Container request</param>
        /// <returns></returns>
        public ActionResult Containers(int id, [DataSourceRequest] DataSourceRequest request)
        {
            MethodResult<IEnumerable<ContainerDto>> result = _cashDepositValidation.GetContainers(id);

            return Json(result.EntityResult.ToDataSourceResult(request));
        }

        #endregion

        #region JsonResult Methods

        /// <summary>
        ///     Cash Deposit dropdwon
        /// </summary>
        /// <returns>Id,Name,Tag</returns>
        public ActionResult CashDepositsColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Deposit Number", Tag = "TransactionReference"},
                new DropDownModel {Id = 2, Name = "Deposit Type", Tag = "CashDepositType"},
                new DropDownModel {Id = 3, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 4, Name = "Account Number", Tag = "BankAccount"},
                new DropDownModel {Id = 5, Name = "Status", Tag = "StatusName"}
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     AutoCompleteCashDeposits as User is Typing...
        /// </summary>
        /// <param name="columName">Is User text as Typing...</param>
        /// <param name="searchData">Is user result as Suggestion or result</param>
        /// <returns>Return user Results</returns>
        public JsonResult AutoCompleteCashDepositsByColumn(string columName, string searchData)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<ListCashDepositDto> result = _cashDepositValidation.All(user);

            var items = new List<string>();

            switch (columName)
            {
                case "TransactionReference":
                {
                    foreach (
                        ListCashDepositDto cashDeposit in
                            result.Where(e => string.IsNullOrEmpty(e.TransactionReference) == false))
                    {
                        if (cashDeposit.TransactionReference.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashDeposit.TransactionReference);
                        }
                    }
                    break;
                }
                case "SiteName":
                {
                    foreach (
                        ListCashDepositDto cashDeposit in result.Where(e => string.IsNullOrEmpty(e.SiteName) == false))
                    {
                        if (cashDeposit.SiteName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashDeposit.SiteName);
                        }
                    }
                    break;
                }
                case "CashDepositType":
                {
                    foreach (
                        ListCashDepositDto cashDeposit in
                            result.Where(e => string.IsNullOrEmpty(e.CashDepositType) == false))
                    {
                        if (cashDeposit.CashDepositType.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashDeposit.CashDepositType);
                        }
                    }
                    break;
                }
                case "BankAccount":
                {
                    IEnumerable<ListCashDepositDto> deposits =
                        result.Where(e => string.IsNullOrEmpty(e.BankAccount) == false);
                    foreach (ListCashDepositDto cashDeposit in deposits)
                    {
                        if (cashDeposit.BankAccount.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashDeposit.BankAccount);
                        }
                    }
                    break;
                }
                case "StatusName":
                {
                    foreach (
                        ListCashDepositDto cashCenter in result.Where(e => string.IsNullOrEmpty(e.StatusName) == false))
                    {
                        if (cashCenter.StatusName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashCenter.StatusName);
                        }
                    }
                    break;
                }
            }
            return Json(items.Distinct(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Deposit
        /// </summary>
        /// <param name="request">CashDeposit request</param>
        /// <returns>CashDeposit</returns>
        public ActionResult Deposits([DataSourceRequest] DataSourceRequest request)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<ListCashDepositDto> result = _cashDepositValidation.All(user);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     Get ContairnerType
        /// </summary>
        /// <param name="containerTypeId">is a containerId</param>
        /// <returns>Return container</returns>
        public ActionResult GetContainerTypeAttributes(int containerTypeId)
        {
            // The value variable that will be passed here will represent
            // the selected value of the ContainerType dropdown list. So we must go ahead 
            // and retrieve the corresponding description here from the database
            ContainerTypeAttribute containerTypeAttributes = _lookup.GetContainerTypeAttributes(containerTypeId);
            if (containerTypeAttributes != null)
            {
                return Json(
                    new
                    {
                        containerTypeAttributes.Attribute1,
                        containerTypeAttributes.Attribute1MinLength,
                        containerTypeAttributes.Attribute1MaxLength,
                        containerTypeAttributes.Attribute2,
                        containerTypeAttributes.Attribute2MinLength,
                        containerTypeAttributes.Attribute2MaxLength,
                        containerTypeAttributes.Attribute3,
                        containerTypeAttributes.Attribute3MinLength,
                        containerTypeAttributes.Attribute3MaxLength,
                        containerTypeAttributes.Attribute4,
                        containerTypeAttributes.Attribute4MinLength,
                        containerTypeAttributes.Attribute4MaxLength,
                        containerTypeAttributes.Attribute5,
                        containerTypeAttributes.Attribute5MinLength,
                        containerTypeAttributes.Attribute5MaxLength
                    }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {Attribute1 = string.Empty}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     GetMerchantSites
        /// </summary>
        /// <param name="merchantId">is merchantId</param>
        /// <returns>Returns merchant</returns>
        public JsonResult GetMerchantSites(int merchantId)
        {
            Merchant merchant = _lookup.GetMerchantById(merchantId);
            return Json(new {merchant.ContractNumber, merchant.Name}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     GetMerchantSites
        /// </summary>
        /// <param name="merchantId">is merchantId</param>
        /// <returns>Returns merchant</returns>
        public JsonResult GetCashCenterTellerAllowedSites(int merchantId)
        {
            bool inrole = Roles.IsUserInRole(User.Identity.Name, "SBVAdmin");
            if (inrole)
            {
                return GetSites(merchantId);
            }
            IEnumerable<DropDownModel> sites =
                _lookup.GetCashCenterTellerAllowedSites(merchantId).ToSitesDropDownModel();
            return Json(sites, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     GetMerchantSites
        /// </summary>
        /// <param name="merchantId">is merchantId</param>
        /// <returns>Returns merchant</returns>
        public JsonResult GetSites(int merchantId)
        {
            IEnumerable<DropDownModel> sites = _lookup.GetSites(merchantId).ToSitesDropDownModel();
            return Json(sites, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     GetMerchantSites MerchantUser
        /// </summary>
        /// <returns>MerchantSites MerchantUser</returns>
        public JsonResult GetMerchantSitesForMerchantUserKendo()
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<DropDownModel> merchants = _lookup.GetSitesForRetailUsers(user).ToSitesDropDownModel();
            return Json(merchants, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     GetMerchants
        /// </summary>
        /// <returns>Return Merchants</returns>
        public JsonResult GetMerchantsKendo()
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<DropDownModel> merchants = _lookup.GetMerchantsServicedByCashCenter(user).ToDropDownModel();
            return Json(merchants, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     GetMerchants
        /// </summary>
        /// <returns>Return Merchants</returns>
        public JsonResult GetHeadOfficeMerchantsKendo()
        {
            IEnumerable<DropDownModel> merchants = _lookup.Merchants().ToDropDownModel();
            return Json(merchants, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     Gets BanksServicedBySite
        /// </summary>
        /// <param name="siteId">is a SiteId</param>
        /// <returns>Return Site</returns>
        public JsonResult GetBanksServicedBySite(int siteId)
        {
            IEnumerable<DropDownModel> merchants = _lookup.GetBanksServicedBySite(siteId).ToDropDownModel();
            return Json(merchants, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     GetSiteSettlementAccounts
        /// </summary>
        /// <param name="bankId">is a BankId</param>
        /// <param name="siteId">is a SiteId</param>
        /// <returns></returns>
        public JsonResult GetSiteSettlementAccounts(int bankId, int siteId)
        {
            IEnumerable<DropDownModel> settlementAccounts =
                _lookup.GetSiteSettlementAccounts(bankId, siteId).ToAccountsDropDownModel();
            return Json(new {SettlementAccounts = settlementAccounts}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Get BanksContainer Types And Deposit References
        /// </summary>
        /// <param name="siteId">is a SiteId</param>
        /// <returns>Return ContainerTypes Deposit References</returns>
        public JsonResult GetBanksContainerTypesAndDepositReferences(int siteId = 0)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<DropDownModel> banks = _lookup.GetBanksServicedBySite(siteId).ToDropDownModel();
            IEnumerable<DropDownModel> containerTypes = _lookup.GetSiteContainerTypesBySiteId(siteId).ToDropDownModel();
            IEnumerable<DropDownModel> depositReferences =
                _lookup.GetSiteDepositReference(siteId).ToDepositReferenceDropDownModel();

            Site siteObject = _cashDepositValidation.GetSite(siteId);
            string citCode = siteObject != null ? siteObject.CitCode : "";

            return
                Json(
                    new
                    {
                        Banks = banks,
                        DepositReferences = depositReferences,
                        ContainerTypes = containerTypes,
                        CarrierStartNumber = _lookup.GetCitInitialDigit(siteId),
                        CitCode = citCode
                    }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Submit New Drop In New Container
        /// </summary>
        /// <param name="container">is a Container</param>
        /// <param name="totalDepositAmount">is totalDepositAmount</param>
        /// <param name="dropDescription">is a dropDescription</param>
        /// <returns>Submit New Drop In New Container</returns>

        #region Submit New Drop In New Container
        public JsonResult SubmitNewDropInNewContainer(ContainerDto container, decimal totalDepositAmount,
            string dropDescription)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<Container> result = _cashDepositValidation.SubmitNewDropInNewContainer(container,
                totalDepositAmount, dropDescription, user);

            if (result.Status == MethodStatus.Successful)
            {
                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message,
                            Container = result.EntityResult,
                            ContainerDrop = result.EntityResult.ContainerDrops[0]
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        result.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }

        /// <summary>
        ///     Submit New Drop In New Container On Post Return
        /// </summary>
        /// <param name="containerId">is a ContainerId</param>
        /// <returns>Submit New Drop In New Container</returns>
        [HttpPost]
        public JsonResult SubmitNewDropInNewContainerOnPostReturn(int containerId)
        {
            Container container = _cashDepositValidation.FindContainer(containerId);
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            GenerateDropSlipFropIds(container, container.ContainerDrops.FirstOrDefault().ContainerDropId, user);

            JsonResult cashDepositDto = Json(string.Empty);
            return cashDepositDto;
        }

        #region Submit ContainerDrop In New Container In New CashDeposit

        /// <summary>
        ///     Submit Container Drop In New Container In New CashDeposit
        /// </summary>
        /// <param name="cashDeposit">is CashDepositId</param>
        /// <param name="dropDescription">is a DropDescription</param>
        /// <returns></returns>
        public JsonResult SubmitContainerDropInNewContainerInNewCashDeposit(CashDepositDto cashDeposit,
            string dropDescription)
        {
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);
            MethodResult<CashDeposit> result = _cashDepositValidation.Add(cashDeposit, user);

            if (result.Status == MethodStatus.Error)
                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message
                        }, JsonRequestBehavior.AllowGet
                    );

            CashDeposit deposit = result.EntityResult;
            string bagReferenceNumber = deposit.Containers[0].ReferenceNumber;
            Container container = result.EntityResult.Containers.FirstOrDefault();
            ContainerDrop containerDrop = container.ContainerDrops.FirstOrDefault();

            return Json
                (
                    new
                    {
                        ResponseCode = result.EntityResult,
                        Message = dropDescription + " was submitted successfully.",
                        BagReferenceNumber = bagReferenceNumber,
                        ContainerDrop = containerDrop
                    }, JsonRequestBehavior.AllowGet
                );
        }

        /// <summary>
        ///     Submit Container Drop In New Container In New Cash Deposit On Post Return
        /// </summary>
        /// <param name="cashDepositId">is cashDepositId</param>
        /// <returns>Submit Container Drop In New Container In NewCash Deposit</returns>
        [HttpPost]
        public JsonResult SubmitContainerDropInNewContainerInNewCashDepositOnPostReturn(int cashDepositId)
        {
            Container container = _cashDepositValidation.FindUnMappedDeposit(cashDepositId).Containers.FirstOrDefault();
            User user = _cashDepositValidation.GetLoggedUser(User.Identity.Name);

            GenerateDropSlipFropIds(container, container.ContainerDrops.FirstOrDefault().ContainerDropId, user);

            JsonResult cashDepositDto = Json(string.Empty);
            return cashDepositDto;
        }

        #endregion

        #endregion

        #endregion
    }
}