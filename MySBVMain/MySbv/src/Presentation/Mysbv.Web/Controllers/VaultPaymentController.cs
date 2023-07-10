using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Application.Dto.Account;
using Application.Dto.Device;
using Application.Dto.Merchant;
using Application.Dto.Site;
using Application.Dto.VaultPayment;
using Application.Modules.Common;
using Application.Modules.VaultPayment;
using Domain.Data.Model;
using Domain.Security;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Task = System.Threading.Tasks.Task;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Bank related functionality
    /// </summary>
    //[Authorize]
    //[CustomAuthorize(Roles = "SBVAdmin")]
    public class VaultPaymentController : BaseController
    {
        private readonly ILookup _lookup;
        private readonly ISecurity _security;
        private readonly IVaultPaymentValidation _vaultPaymentValidation;

        public VaultPaymentController()
        {
            _vaultPaymentValidation = LocalUnityResolver.Retrieve<IVaultPaymentValidation>();
            _security = LocalUnityResolver.Retrieve<ISecurity>();
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
        }


        #region Actions

        /// <summary>
        ///     Make Vault payments
        /// </summary>
        /// <returns>Bank</returns>
        [Authorize]
        [CustomAuthorize(Roles = "SBVAdmin, RetailSupervisor")]
        public async Task<ActionResult> Index()
        {
            // get loggin user
            User user = _lookup.GetLoggedUser(User.Identity.Name);
            bool isRetailSupervisor = User.IsInRole("RetailSupervisor");


            //Check if Current loggedIn User is an Admin OR RetailSupervisor with privileges to Make Vault Payment
            if (user.CanMakeVaultPayment && isRetailSupervisor || User.IsInRole("SBVAdmin"))
            {
                // generate model
                PaymentModel model = await _vaultPaymentValidation.GeneratePaymentModel(user, isRetailSupervisor);

                #region INITIALIZE DROPDOWNS            

                // accounts
                var accounts = new List<AccountDto>();
                accounts.Insert(0, new AccountDto
                {
                    AccountId = 0,
                    AccountNumber = "Please select..."
                });

                // sites
                var sites = new List<SiteDto>();

                sites.Insert(0, new SiteDto
                {
                    SiteId = 0,
                    Name = "Please select..."
                });

                // devices
                var devices = new List<DeviceDto>();
                devices.Insert(0, new DeviceDto
                {
                    DeviceId = 0,
                    Name = "Please select..."
                });

                #endregion

                if (!isRetailSupervisor)
                {
                    // merchants
                    model.Merchants.Insert(0, new MerchantDto
                    {
                        MerchantId = 0,
                        Name = "Please select..."
                    });

                    model.Sites = sites;
                }
                else
                {
                    int merchantId = user.MerchantId.HasValue ? user.MerchantId.Value : 0;

                    model.Sites = await _vaultPaymentValidation.GetSites(merchantId);

                    if (model.Sites != null)
                    {
                        model.Sites.Insert(0, new SiteDto
                        {
                            SiteId = 0,
                            Name = "Please select..."
                        });
                    }
                }

                model.Devices = devices;
                model.Accounts = accounts;

                // serialize model
                IHtmlString serializedModel = JavaScriptConvert.SerializeObject(model);

                // get model
                ViewBag.PaymentModel = serializedModel;

                return View();
            }
            return View("NotAuthorized");
        }


        /// <summary>
        ///     get merchant sites
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [CustomAuthorize(Roles = "SBVAdmin, RetailSupervisor")]
        public async Task<JsonResult> GetSites(int merchantId)
        {
            List<SiteDto> sites = await _vaultPaymentValidation.GetSites(merchantId);

            var devices = new List<DeviceDto>
            {
                new DeviceDto
                {
                    DeviceId = 0,
                    Name = "Please select..."
                }
            };
            sites.Insert(0, new SiteDto
            {
                SiteId = 0,
                Name = "Please select..."
            });
            return Json(
                new
                {
                    Devices = devices,
                    Sites = sites
                },
                JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     get merchant sites
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [CustomAuthorize(Roles = "SBVAdmin, RetailSupervisor")]
        public async Task<JsonResult> GetDevices(int siteId)
        {
            // get devices
            IEnumerable<DeviceDto> devices = await _vaultPaymentValidation.GetSiteDevices(siteId);

            // get site details
            SiteDto siteData = await _vaultPaymentValidation.GetSiteInfo(siteId);

            siteData.Accounts.Insert(0, new AccountDto
            {
                AccountId = 0,
                AccountNumber = "Please select..."
            });

            return Json(
                new
                {
                    Devices = devices,
                    SiteData = siteData,
                    siteData.Accounts
                },
                JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     get amount in dcevice
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [CustomAuthorize(Roles = "SBVAdmin, RetailSupervisor")]
        public async Task<JsonResult> GetAmountInDevice(int deviceId)
        {
            string bagNumber = await _vaultPaymentValidation.GetOpenBagNumber(deviceId);

            decimal amount = await _vaultPaymentValidation.GetAmountInDevice(bagNumber);

            return Json(
                new
                {
                    Amount = amount,
                    BagSerialNumber = bagNumber
                },
                JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [CustomAuthorize(Roles = "SBVAdmin, RetailSupervisor")]
        public async Task<JsonResult> ReleasePayment(VaultPaymentDto model)
        {
            int userId = _security.GetUserId(User.Identity.Name);

            string userRole = User.IsInRole("SBVAdmin") ? "SBVAdmin" : "RetailSupervisor";

            MethodResult<PaymentResponseDto> result =
                await _vaultPaymentValidation.ReleasePayment(model, userId, userRole);

            if (result.Status == MethodStatus.Successful)
            {
                await GenerateAndEmailPaymentReceipt(result.EntityResult.VaultPartialPaymentId, model);
            }

            return Json(result.EntityResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        [CustomAuthorize(Roles = "SBVAdmin, RetailSupervisor")]
        public ActionResult PrintPaymentSlip(int id)
        {
            // Attach Deposit Slip
            const string reportPath = "/Payments/VaultPaymentSlip";
            var stream = new MemoryStream(GeneratePdf(id, reportPath));
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        #region helpers

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentDto"></param>
        /// <returns></returns>
        private async Task GenerateAndEmailPaymentReceipt(int id, VaultPaymentDto paymentDto)
        {
            const string reportPath = "/Payments/VaultPaymentSlip";
            byte[] attachmentBytes = GeneratePdf(id, reportPath);

            var ms = new MemoryStream(attachmentBytes);
            var attachement = new Attachment(ms, "mySBV.vault Payment Receipt.pdf");

            await CreateEmail(paymentDto, attachement);
        }


        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reportPath"></param>
        private byte[] GeneratePdf(long id, string reportPath)
        {
            var paramz = new Dictionary<string, string>();
            paramz.Add("VaultPartialPaymentId", id.ToString());
            paramz.Add("IsPayment", "true");
            byte[] result = GenerateReport(reportPath, paramz);
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="paymentDto"></param>
        /// <param name="attachement"></param>
        private async Task CreateEmail(VaultPaymentDto paymentDto, Attachment attachement)
        {
            //// Vault Payment Creator (or user)
            User user = await _vaultPaymentValidation.GetLoggedUser(User.Identity.Name);

            var message = new MailMessage();
            message.Attachments.Add(attachement);

            string siteEmail = await _vaultPaymentValidation.GetSiteContactPersonEmail(paymentDto.CitCode);

            var recipients = new List<string> {user.EmailAddress, siteEmail};

            // NOTE:    Email is sent to capturer and the Site Contact Person.
            string capturerEmailBody = CreateBody(paymentDto, user, true);

            await
                Task.Factory.StartNew(
                    () => SendEmailsToMany(recipients, message, "mySBV.vault Payment Request", capturerEmailBody));

            //NOTE:     Email is sent to beneficiary, only if the email address is supplied.
            if (!string.IsNullOrWhiteSpace(paymentDto.BeneficiaryEmailAddress))
            {
                string beneficiaryEmailBody = CreateBody(paymentDto, user, false);

                await
                    Task.Factory.StartNew(
                        () =>
                            SendEmailNotification(paymentDto.BeneficiaryEmailAddress, message, "mySBV.vault Payment Advice",
                                beneficiaryEmailBody));
            }
        }


        /// <summary>
        ///     Create email body
        /// </summary>
        /// <param name="user"></param>
        /// <param name="paymentDto"></param>
        /// <param name="isCapturer"></param>
        /// <returns></returns>
        private string CreateBody(VaultPaymentDto paymentDto, User user, bool isCapturer)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            string salutation;
            string pleaseText;

            if (isCapturer)
            {
                salutation = string.Format("Dear {0} {1}", user.FirstName, user.LastName);
                pleaseText = string.Format("Please find attached the payment receipt you have just made to {0}.",
                    paymentDto.BeneficiaryName);
            }
            else
            {
                salutation = string.Format("Dear {0}", paymentDto.BeneficiaryName);
                pleaseText = string.Format("Please find attached the payment request receipt from  {0}.",
                    paymentDto.SiteName);
            }

            html.RenderBeginTag(HtmlTextWriterTag.H4);
            html.WriteEncodedText(salutation);
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.Hr);
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText(pleaseText);

            if (isCapturer)
            {
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText("Thank you for using mySBV.vault.");
            }
            else
            {
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText(
                    "This payment request was brought to you by mySBV Cash Solutions. Please validate this payment by drawing a bank statement.");
            }

            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText("Best Regards,");
            html.WriteBreak();
            html.WriteEncodedText("SBV Services (Pty) Ltd");
            html.RenderEndTag();

            html.Flush();
            return writer.ToString();
        }

        #endregion
    }
}