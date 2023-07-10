using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.UI;
using Application.Dto.CashOrder;
using Application.Dto.CashOrderTask;
using Application.Modules.CashOrdering.Approval;
using Application.Modules.CashOrdering.CashOrders;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Cash Order related functionality
    /// </summary>
    public class CashOrderController : BaseController
    {
        private readonly ICashOrderingValidation _cashOrderingValidation;
        private readonly ICashOrderApprovalValidation _cashOrderApprovalValidation;
        private readonly ILookup _lookup;
        private readonly ISecurity _security;

        public CashOrderController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _security = LocalUnityResolver.Retrieve<ISecurity>();
            _cashOrderApprovalValidation = LocalUnityResolver.Retrieve<ICashOrderApprovalValidation>();
            _cashOrderingValidation = LocalUnityResolver.Retrieve<ICashOrderingValidation>();
        }



        #region List CashOrders

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVTellerSupervisor, SBVAdmin")]
        public ActionResult Index()
        {
            ViewBag.IsRetailUser = User.IsInRole("RetailUser").ToString().ToLower();
            ViewBag.IsSBVTeller = User.IsInRole("SBVTeller").ToString().ToLower();
            ViewBag.IsSBVAdmin = User.IsInRole("SBVAdmin").ToString().ToLower();

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            ViewBag.IsValidUser = (user.UserTypeId != null);

            IEnumerable<ListCashOrderDto> cashOders = _cashOrderingValidation.All(user);

            return View(cashOders);
        }

        #endregion

        #region Create CashOrder

        /// <summary>
        ///     Create Cash Order
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Create()
        {
            //Clear all old attachments by the user
            RemoveOldFiles();

            StaticData();

            var cashOrder = new CashOrderDto
            {
                CapturedDateString = DateTime.Today.ToShortDateString(),
                CapturedDateTime = DateTime.Now,
                //CashOrderAmount = 0.00M,
                InitialCitSerialNumber = 0,
                CreateDate = DateTime.Now,
                CreatedById = _security.GetUserId(User.Identity.Name)
            };
            
            return View(cashOrder);
        }

        /// <summary>
        ///     Create Cash Order
        /// </summary>
        /// <param name="cashOrderDto">Database entity instance</param>
        [HttpPost]
        public JsonResult Create(CashOrderDto cashOrderDto)
        {
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);

            var isEft = _lookup.IsEft(cashOrderDto.CashOrderTypeId); 

            MethodResult<CashOrder> result = _cashOrderingValidation.Add(cashOrderDto, user);

            if (result.Status == MethodStatus.Successful)
            {
                TrimCashOrder(result.EntityResult);

                if (isEft)
                {                    
                    System.Threading.Tasks.Task.Factory.StartNew(
                        () => MapAttachmentsToOrder(result.EntityResult.CashOrderId, User.Identity.Name)
                        );
                }

                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult.CashOrderId,
                            result.Message,
                            CashOrder = result.EntityResult
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
        /// 
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <param name="userName"></param>
        private void MapAttachmentsToOrder(int cashOrderId, string userName)
        {
            var baseUrl = _lookup.GetConfigurationValue("CASH_ORDER_EFT_ATTACHMENTS_URL");

            var newPhysicalPath = _lookup.CreateTemporaryFolder(cashOrderId, userName);

            if (Directory.Exists(newPhysicalPath))
            {
                var moveFromFolder = Path.Combine(baseUrl, userName);

                if (Directory.Exists(moveFromFolder))
                {
                    var files = Directory.GetFiles(moveFromFolder, "*");

                    foreach (var fullName in files)
                    {
                        var fileName = Path.GetFileName(fullName);

                        string newFilePath = Path.Combine(newPhysicalPath, fileName);
                        string moveFromPath = Path.Combine(moveFromFolder, fileName);

                        System.IO.File.Move(moveFromPath, newFilePath);
                    }
                }
            }
        }


        private void RemoveOldFiles()
        {
           User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
           var userOldAttachUrl = _lookup.CreateTemporaryFolder(0, user.UserName);

           if (Directory.Exists(userOldAttachUrl))
               Directory.Delete(userOldAttachUrl, true);
        }

        #endregion

        #region View CashOrder

        /// <summary>
        ///     View existing Cash Orders
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVTellerSupervisor, SBVAdmin")]
        public ActionResult View(int id = 0)
        {
            MethodResult<CashOrderDto> cashOrder = _cashOrderingValidation.Find(id);
            SelectedDropDowns(cashOrder.EntityResult);
            return View(cashOrder.EntityResult);
        }

        #endregion

        #region Approve CashOrder

        /// <summary>
        /// Approve Cash Order.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVFinanceReviewer, SBVAdmin")]
        public ActionResult Approval(int id = 0)
        {
            if (VerifyAuthentication())
            {
                var submittedStatusId = _lookup.GetStatusId("SUBMITTED");
                var activeStatusId = _lookup.GetStatusId("ACTIVE");

                MethodResult<CashOrderDto> cashOrder = _cashOrderingValidation.Find(id);

                if (cashOrder.EntityResult == null)
                {
                    ShowMessage("You cannot approve an order that has already been deleted from the system!", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("TaskList");
                }

                if (!_lookup.IsEft(cashOrder.EntityResult.CashOrderTypeId))
                {
                    ShowMessage("You cannot approve an order that's not an EFT Order", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("TaskList");
                }

                if (cashOrder.EntityResult.StatusId == submittedStatusId)
                {
                    ShowMessage("You cannot approve an already submitted Order", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("TaskList");
                }

                if (cashOrder.EntityResult.StatusId == activeStatusId)
                {
                    ShowMessage("You cannot approve an Order that is in an Active Status", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("TaskList");
                }
            
                if (cashOrder.EntityResult == null)
                    return RedirectToAction("TaskList");

                ViewBag.Attachments = _cashOrderingValidation.GetFiles(id);

                SelectedDropDowns(cashOrder.EntityResult);
                return View(cashOrder.EntityResult);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Approve Cash Order.
        /// </summary>
        /// <param name="id">Database Entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Approve(int id = 0)
        {
            var cashOrder = new MethodResult<CashOrder>(MethodStatus.Error, null, "Cash Order not Approved");
            CashOrderDto foundCashOrder = _cashOrderingValidation.Find(id).EntityResult;
            CashOrderTaskDto foundCashOrderTask = _cashOrderingValidation.FindCashOrderTask(id).EntityResult;

            if (foundCashOrderTask == null)
            {
                cashOrder = new MethodResult<CashOrder>(MethodStatus.Error, null, "Cash Order has already been deleted by the client!");
                return Json(new{ResponseCode = -1,cashOrder.Message}, JsonRequestBehavior.AllowGet);}

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            
            DateTime date = DateTime.Now;
            foundCashOrder.OrderDate = date;
            foundCashOrder.DateSubmitted = date;
            foundCashOrder.IsSubmitted = true;
            foundCashOrder.StatusName = "SUBMITTED";
            foundCashOrder.Comments = foundCashOrder.Comments + Environment.NewLine + "CashOrder: Approved";

            cashOrder = _cashOrderingValidation.Approve(foundCashOrder, user);
           
            // was submitted successfully
            if (cashOrder.Status == MethodStatus.Successful)
            {
                //Generate Approval Request email - Financial Reviewer
                SendApprovalEmails(new MailMessage(), foundCashOrder, user);

                TrimCashOrder(cashOrder.EntityResult);
                return Json
                    (
                        new
                        {
                            ResponseCode = cashOrder.EntityResult.CashOrderId,
                            cashOrder.Message,
                            CashOrder = cashOrder.EntityResult
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            // failed to submit cash order
            return Json
                (
                    new
                    {
                        ResponseCode = -1,
                        cashOrder.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }

        /// <summary>
        /// Reject Cash Order.
        /// </summary>
        /// <param name="cashOrderId">Database Entity instance</param>
        /// <param name="prevComments">Database Entity instance</param>
        /// <param name="newComments">Database Entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Reject(int cashOrderId, string prevComments, string newComments)
        {
            var cashOrder = new MethodResult<CashOrder>(MethodStatus.Error, null, "Cash Order not Rejected");
            CashOrderDto foundCashOrder = _cashOrderingValidation.Find(cashOrderId).EntityResult;

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);

            DateTime date = DateTime.Now;
            foundCashOrder.OrderDate = date;
            foundCashOrder.DateSubmitted = date;

            //Add comment to previous comments
            foundCashOrder.Comments = prevComments + Environment.NewLine +
                                         "Declined: " + newComments;

            cashOrder = _cashOrderingValidation.Reject(foundCashOrder, user);

            // was submitted successfully
            if (cashOrder.Status == MethodStatus.Successful)
            {
                //Send out Rejection email notification
                SendDeclineEmails(new MailMessage(), foundCashOrder, user);

                TrimCashOrder(cashOrder.EntityResult);
                return Json
                    (
                        new
                        {
                            ResponseCode = cashOrder.EntityResult.CashOrderId,
                            cashOrder.Message,
                            CashOrder = cashOrder.EntityResult
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            // failed to submit cash order
            return Json
                (
                    new
                    {
                        ResponseCode = -1,
                        cashOrder.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }


        #endregion

        #region Edit CashOrder

        /// <summary>
        ///  Edit Cash Order
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVDataCapture, SBVAdmin")]
        public ActionResult Edit(int id = 0)
        {
            if (VerifyAuthentication())
            {
                var submittedStatusId = _lookup.GetStatusId("SUBMITTED");
                var pendingStatusId = _lookup.GetStatusId("PENDING");         

                MethodResult<CashOrderDto> cashOrder = _cashOrderingValidation.Find(id);

                if (cashOrder.EntityResult == null)
                {
                    ShowMessage("You cannot edit an Order that is not in a state that requires editing ", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("Index");
                }

                if (cashOrder.EntityResult.StatusId == submittedStatusId)
                {
                    ShowMessage("You cannot edit an already submitted Order", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("Index");
                }

                if (cashOrder.EntityResult.StatusId == pendingStatusId)
                {
                    ShowMessage("You cannot edit an Order awaiting Approval", MessageType.error, "Cash Order Operation");
                    return RedirectToAction("Index");
                }

                SelectedDropDowns(cashOrder.EntityResult);
                return View(cashOrder.EntityResult);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Edit Cash Order
        /// </summary>
        /// <param name="cashOrderDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit(CashOrderDto cashOrderDto)
        {
            MethodResult<CashOrderDto> cashOrderBeforeUpdate = _cashOrderingValidation.Find(cashOrderDto.CashOrderId);

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            cashOrderDto.StatusId = _lookup.GetStatusId("ACTIVE");
            
            MethodResult<int> result = _cashOrderingValidation.Edit(cashOrderDto, user);

            if (result.Status == MethodStatus.Successful)
            {
                MethodResult<CashOrderDto> cashOrder = _cashOrderingValidation.Find(result.EntityResult);

                //If Declined CashOrder AND CashOrder Type = EFT is updated for approval
                var declinedStatus = _lookup.GetStatusId("DECLINED");

                 if (cashOrderBeforeUpdate.EntityResult.StatusId == declinedStatus && 
                     cashOrderBeforeUpdate.EntityResult.CashOrderType.Name == _cashOrderingValidation.GetCashOrderType(cashOrderBeforeUpdate.EntityResult.CashOrderTypeId))
                {
                    //// Update Declined Task back to Pending Status
                    _cashOrderingValidation.CRUD_ApprovalTask(cashOrder.EntityResult.CashOrderId, user, "UPDATE");

                    //Send out emails to approver and other role players
                    EmailCashOrderSlip(cashOrder.EntityResult, user);
                }

                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message,
                            CashOrder = cashOrder.EntityResult
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

        #region Copy CashOrder

        /// <summary>
        ///     Copy Cash Order
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Copy(int id = 0)
        {
            CashOrderDto cashOrder = _cashOrderingValidation.Find(id).EntityResult;
            cashOrder.CapturedDateTime = DateTime.Now;
            cashOrder.DeliveryDate = string.Empty;
            cashOrder.ContainerNumberWithCashForExchange = string.Empty;
            cashOrder.EmptyContainerOrBagNumber = string.Empty;
            cashOrder.CreateDate = DateTime.Now;
            SelectedDropDowns(cashOrder);
            return View(cashOrder);
        }

        /// <summary>
        ///     Copy Cash Order
        /// </summary>
        /// <param name="cashOrderDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Copy(CashOrderDto cashOrderDto)
        {
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);

            var isEft = _lookup.IsEft(cashOrderDto.CashOrderTypeId);

            MethodResult<CashOrder> result = _cashOrderingValidation.Add(cashOrderDto, user);

            if (result.Status == MethodStatus.Successful)
            {
                TrimCashOrder(result.EntityResult);

                if (isEft)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(
                        () => MapAttachmentsToOrder(result.EntityResult.CashOrderId, User.Identity.Name)
                        );
                }

                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult.CashOrderId,
                            result.Message,
                            CashOrder = result.EntityResult
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

        #region Cancel CashOrder

        /// <summary>
        ///     Cancel Cash Order
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Cancel(int id)
        {
            ViewBag.HasTriedToEditSubmittedDeposit = "";

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            CashOrderDto foundCashOrder = _cashOrderingValidation.Find(id).EntityResult;
            var cashOrderTask = _cashOrderingValidation.FindCashOrderTask(id);

            MethodResult<bool> deleteResult = _cashOrderingValidation.Delete(id, user);
            
            if (deleteResult.Status == MethodStatus.Successful)
            {
                if (cashOrderTask.EntityResult != null)
                {
                    var pendingStatus = _lookup.GetStatusId("PENDING");
                    if (cashOrderTask.EntityResult.StatusId == pendingStatus)
                    {
                        // NOTE: 
                        //Send out Rejection email notification
                        SendDeletionEmails(new MailMessage(), foundCashOrder, user, foundCashOrder.ReferenceNumber);
                    }
                }

                ShowMessage(deleteResult.Message, MessageType.success, "Delete Cash Order");
            }
            else
            {
                ShowMessage(deleteResult.Message,
                    deleteResult.Status == MethodStatus.Error ? MessageType.error : MessageType.warning,
                    "Delete Cash Order");
            }
            return Json(new {url = Url.Action("Index")});
        }


        /// <summary>
        ///     Cancel Cash Order
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        public ActionResult Delete()
        {
            return View(new List<ListCashOrderDto>());
        }

        #endregion

        #region Submit CashOrder

        /// <summary>
        /// Submit cash Order
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "RetailUser, RetailSupervisor, SBVTeller, SBVAdmin")]
        public ActionResult Submit(int id = 0)
        {
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            CashOrderDto cashOrder = _cashOrderingValidation.Find(id).EntityResult;

            var pendingStatusId = _lookup.GetStatusId("PENDING");   
            
            if (cashOrder.StatusId == pendingStatusId)
            {
                ShowMessage("You cannot submit an Order awaiting Approval", MessageType.error, "Cash Order Operation");
                return RedirectToAction("Index");
            }

            if (cashOrder.StatusName == "Submitted")
            {
                EmailCashOrderSlip(cashOrder, user);

                ShowMessage("An Email with Cash order details was sent to your mailbox.", MessageType.success, "Email.");
                return RedirectToAction("Index");
            }

            if (_lookup.IsEft(cashOrder.CashOrderTypeId))
            {
                var files = _cashOrderingValidation.GetFiles(cashOrder.CashOrderId);

                if (files.Count == 0)
                {
                    ShowMessage("No Attachment has been added, Please attach Proof of Payment", MessageType.error, "Cash Order Submit");
                    return RedirectToAction("Index");
                }
            }

            MethodResult<CashOrder> submittedCashOrder = _cashOrderingValidation.Submit(cashOrder, user);

            if (submittedCashOrder.Status == MethodStatus.Successful)
            {
                ShowMessage("Cash Order Submitted successfully.", MessageType.success, "Cash Order Submit");
                //var sendEmailTask = System.Threading.Tasks.Task.Factory.StartNew(() => EmailCashOrderSlip(cashOrder, user)); 

                EmailCashOrderSlip(cashOrder, user);
                

            ViewBag.CashOrderId = submittedCashOrder.EntityResult;
                return RedirectToAction("Index");
            }

            ShowMessage("Failed to Submit Cash Order!", MessageType.error, "Cash Order Submit");
            return RedirectToAction("Index");
        }

        /// <summary>
        ///  Submit Cash Order
        /// </summary>
        /// <param name="cashOrderDto">Database Entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Submit(CashOrderDto cashOrderDto)
        {
            var user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);

            var cashOrder = UpdateOrderOnSubmit(cashOrderDto, user);

            // was submitted successfully
            if (cashOrder.Status == MethodStatus.Successful)
            {
                TrimCashOrder(cashOrder.EntityResult);

                ShowMessage("Cash Order has been submitted successfully", MessageType.success, "Cash Order");

                return Json
                    (
                        new
                        {
                            ResponseCode = cashOrder.EntityResult.CashOrderId,
                            cashOrder.Message,
                            CashOrder = cashOrder.EntityResult
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            // failed to submit cash order

            ShowMessage("Failed to submit Cash Order!", MessageType.error, "Cash Order");

            return Json
                (
                    new
                    {
                        ResponseCode = -1,
                        cashOrder.Message
                    }, JsonRequestBehavior.AllowGet
                );
        }


        /// <summary>
        /// Update Order On Submit
        /// </summary>
        /// <param name="cashOrderDto"></param>
        /// <param name="user"></param>
        public MethodResult<CashOrder> UpdateOrderOnSubmit(CashOrderDto cashOrderDto, User user)
        {
            var isEftOrder = _lookup.IsEft(cashOrderDto.CashOrderTypeId);

            if (isEftOrder)
            {
                cashOrderDto.IsSubmitted = false;
                cashOrderDto.StatusName = "PENDING";
                cashOrderDto.StatusId = _lookup.GetStatusId("PENDING");
                cashOrderDto.Comments = "CashOrder: Submission";
            }
            else
            {
                cashOrderDto.IsSubmitted = true;
                cashOrderDto.StatusName = "SUBMITTED";
            }

            MethodResult<CashOrder> cashOrder;
            
            if (cashOrderDto.CashOrderId > 0)
            {
                // update cash order,
                // mark it as submitted

                // edit order
                _cashOrderingValidation.Edit(cashOrderDto, user); 
            }
            else
            {
                // create cash order,
                // mark it as submitted
                var date = DateTime.Now;
                cashOrderDto.OrderDate = date;
                cashOrderDto.DateSubmitted = date;

                // add order
                cashOrder = _cashOrderingValidation.Add(cashOrderDto, user);
                cashOrderDto.CashOrderId = cashOrder.EntityResult.CashOrderId;
            }

            cashOrder = _cashOrderingValidation.GetOrder(cashOrderDto.CashOrderId);

            // Submit Cash Order
            if (cashOrder.Status == MethodStatus.Successful)
            {
                cashOrderDto.CashOrderId = cashOrder.EntityResult.CashOrderId;
                cashOrderDto.CashOrderTypeId = cashOrder.EntityResult.CashOrderTypeId;
                //System.Threading.Tasks.Task.Factory.StartNew(() => 
                EmailCashOrderSlip(cashOrderDto, user); //);
            }

            if (isEftOrder)
            {
                // map attachments to Order after Submitting/Saving.
                System.Threading.Tasks.Task.Factory.StartNew(
                    () =>
                    {
                        _cashOrderingValidation.CRUD_ApprovalTask(cashOrder.EntityResult.CashOrderId, user, "CREATE");

                        MapAttachmentsToOrder(cashOrder.EntityResult.CashOrderId, user.UserName);
                    });
            }
            
            return cashOrder;
        }

        #endregion

        #region Process Cash Order

        /// <summary>
        ///     Process Order
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVTeller, SBVTellerSupervisor, SBVAdmin")]
        public ActionResult ProcessOrder()
        {
            ViewBag.IsLoaded = false;
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            return View(new CashOrderDto {UserTypeId = user.UserTypeId, IsSerialNumber = true});
        }

        /// <summary>
        ///     Process Order
        /// </summary>
        /// <param name="cashOrderDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessOrder(CashOrderDto cashOrderDto)
        {
            ViewBag.IsLoaded = true;
            SelectedDropDowns(cashOrderDto);

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);

            MethodResult<CashOrderDto> result;

            if (cashOrderDto.IsSerialNumber)
            {
                result = _cashOrderingValidation.FindBySealSerialNumber(cashOrderDto.SealSerialNumber, user);
            }
            else
            {
                result = _cashOrderingValidation.FindByRefenceNumber(cashOrderDto.SealSerialNumber, user);
            }
            

            if (result.Status == MethodStatus.Error)
            {
                ShowMessage(string.Format("{0}", result.Message), MessageType.info, "Process Cash Order");
                return RedirectToAction("ProcessOrder");
            }

            result.EntityResult.OrderDateString = result.EntityResult.OrderDate.HasValue
                ? result.EntityResult.OrderDate.Value.ToShortDateString()
                : "";

            // Cointainer with Cash for exchange
            IEnumerable<ItemDenominationDto> cashSubMittedDenominations =
                _cashOrderingValidation.CashForwardedForExchange(result.EntityResult.CashOrderId);

            ViewBag.CashSubMittedNoteDenominations = (cashSubMittedDenominations == null)
                ? null
                : cashSubMittedDenominations.Where(e => e.DenominationType == "Notes");
            ViewBag.NotesSubTotal = (cashSubMittedDenominations == null)
                ? 0
                : cashSubMittedDenominations.Where(e => e.DenominationType == "Notes").Sum(o => o.Value);

            ViewBag.CashSubMittedCoinDenominations = (cashSubMittedDenominations == null)
                ? null
                : cashSubMittedDenominations.Where(e => e.DenominationType == "Coins");
            ViewBag.CoinsSubTotal = (cashSubMittedDenominations == null)
                ? 0
                : cashSubMittedDenominations.Where(e => e.DenominationType == "Coins").Sum(o => o.Value);


            // Cointainer with Cash Ordered
            IEnumerable<ItemDenominationDto> cashOrderedDenominations =
                _cashOrderingValidation.CashRequiredByClient(result.EntityResult.CashOrderId);

            ViewBag.CashRequiredNoteDenominations = cashOrderedDenominations.Where(e => e.DenominationType == "Notes");
            ViewBag.NotesRequiredSubTotal =
                cashOrderedDenominations.Where(e => e.DenominationType == "Notes").Sum(o => o.Value);

            ViewBag.CashRequiredCoinDenominations = cashOrderedDenominations.Where(e => e.DenominationType == "Coins");
            ViewBag.CoinsRequiredSubTotal =
                cashOrderedDenominations.Where(e => e.DenominationType == "Coins").Sum(o => o.Value);

            //string cashCenter = _lookup.GetUserCashCenter(User.Identity.Name);

            //ViewData.Add("Reasons", new SelectList(_lookup.GetReasons(), "Id", "Name"));

            if (result.Status == MethodStatus.Successful)
            {
                return View(result.EntityResult);
            }
            ShowMessage(string.Format("{0}", result.Message), MessageType.info, "Process Cash Order");
            return RedirectToAction("ProcessOrder");
        }

        /// <summary>
        ///     Cash order Process
        /// </summary>
        /// <param name="cashOrderContainerDto">Database entity instance</param>
        /// <param name="containerNumberWithCashForExchange">Container Number</param>
        /// <param name="emptyContainerOrBagNumber">Empty Container or BagNumber</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Process(CashOrderContainerDto cashOrderContainerDto, string containerNumberWithCashForExchange, string emptyContainerOrBagNumber)
        {
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);

            MethodResult<int> result = _cashOrderingValidation.Process(cashOrderContainerDto, containerNumberWithCashForExchange, emptyContainerOrBagNumber, user);

            if (result.Status == MethodStatus.Successful)
            {
                CashOrderDto cashOrder = _cashOrderingValidation.Find(result.EntityResult).EntityResult;
                
                // send verification slip
                System.Threading.Tasks.Task.Factory.StartNew(() => EmailCashOrderSlip(cashOrder, user)); 

                return Json
                    (
                        new
                        {
                            ResponseCode = result.EntityResult,
                            result.Message,
                            CashOrder = cashOrder
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
        /// Generate cash Order Slip
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVTeller, SBVTellerSupervisor, SBVAdmin")]
        public ActionResult GenerateCashOrderProcessingSlip(int id = 0)
        {
            CashOrderDto cashOrder = _cashOrderingValidation.Find(id).EntityResult;

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            //var submittedCashOrder = _cashOrderingValidation.ValidateAndSubmit(cashOrder, user);

            if (cashOrder.StatusName == "Processed")
            {
                ShowMessage("An email with Cash Order Processed successfully.", MessageType.success, "Cash Order");
                //var sendEmailTask = System.Threading.Tasks.Task.Factory.StartNew(() => EmailCashOrderSlip(cashOrder, user)); 
                EmailCashOrderProcessingSlip(cashOrder, user);
                //ViewBag.CashOrderId = submittedCashOrder.EntityResult;
                return RedirectToAction("ProcessOrder");
            }
            ShowMessage("Failed to Process Cash Order!", MessageType.error, "Cash Order");
            return RedirectToAction("ProcessOrder");
        }

        /// <summary>
        /// Email Cash order Slip
        /// </summary>
        /// <param name="cashOrder">Database entity instance</param>
        /// <param name="user">User</param>
        private void EmailCashOrderProcessingSlip(CashOrderDto cashOrder, User user)
        {
            var message = new MailMessage();

            // Attach CashOrder Slip
            string reportPath = cashOrder.CashOrderType.Name == "EFT"
                ? "/CashOrder/EFTCashVerificationReport"
                : "/CashOrder/CashForCashVerificationReport";

            byte[] reportAttachmentBytes = CashOrderProcessingSlipParametersAndPdfGeneration(cashOrder, reportPath);
            var ms = new MemoryStream(reportAttachmentBytes);
            var attachement = new Attachment(ms, "CashOrderVerificationReport.pdf");
            message.Attachments.Add(attachement);

            // Send CashOrder Slip via Email.
            SendEmail(message, cashOrder, user);
            ShowMessage("An Email with processed Cash order details was sent to your mailbox.", MessageType.success,
                "Email Cash order.");
        }

        /// <summary>
        ///     Cash order Processing Slip Generation
        /// </summary>
        /// <param name="cashOrder">Database entity instance</param>
        /// <param name="reportPath">The path of the report</param>
        /// <returns></returns>
        public byte[] CashOrderProcessingSlipParametersAndPdfGeneration(CashOrderDto cashOrder, string reportPath)
        {
            int requiredDropId =
                cashOrder.CashOrderContainer.CashOrderContainerDrops.FirstOrDefault(e => e.IsCashRequiredInExchange)
                    .CashOrderContainerDropId;
            CashOrderContainerDropDto forwardedDrop =
                cashOrder.CashOrderContainer.CashOrderContainerDrops.FirstOrDefault(e => e.IsCashForwardedForExchange);
            int forwardedDropId = forwardedDrop == null ? 0 : forwardedDrop.CashOrderContainerDropId;

            var paramz = new Dictionary<string, string>();
            paramz.Add("CashOrderId", cashOrder.CashOrderId.ToString());

            if (forwardedDrop != null)
            {
                paramz.Add("ContainerForwardedDropId", forwardedDropId.ToString());
            }
            paramz.Add("ContainerRequiredDropId", requiredDropId.ToString());
            byte[] result = GenerateReport(reportPath, paramz);
            return result;
        }

        #endregion

        #region List GridView

        /// <summary>
        ///     Get Denominations
        /// </summary>
        /// <param name="denominationType">The type of denomination</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDenominations(string denominationType)
        {
            IEnumerable<Denomination> denominations = _lookup.GetDenominations(denominationType);

            return Json(denominations);
        }

        /// <summary>
        ///     Get Cash Order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetCashOrdersKendo([DataSourceRequest] DataSourceRequest request)
        {
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<ListCashOrderDto> cashOders = _cashOrderingValidation.All(user);

            return Json(cashOders.ToDataSourceResult(request));
        }

        /// <summary>
        ///     Cash Order Column Listing
        /// </summary>
        /// <returns></returns>
        public ActionResult CashOrdersColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Transaction Number", Tag = "ReferenceNumber"},
                new DropDownModel {Id = 2, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 3, Name = "Delivery Date", Tag = "DeliveryDate"},
                new DropDownModel {Id = 4, Name = "Order Type", Tag = "OrderType"}
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Auto complete cash Order  by Column
        /// </summary>
        /// <param name="columName">The column used to filter</param>
        /// <param name="searchData">Search String</param>
        /// <returns></returns>
        public JsonResult AutoCompleteCashOrdersByColumn(string columName, string searchData)
        {
            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            IEnumerable<ListCashOrderDto> cashOrders = _cashOrderingValidation.All(user);

            var items = new List<string>();

            switch (columName)
            {
                case "ReferenceNumber":
                {
                    foreach (
                        ListCashOrderDto cashOrder in
                            cashOrders.Where(e => string.IsNullOrEmpty(e.ReferenceNumber) == false))
                    {
                        if (cashOrder.ReferenceNumber.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashOrder.ReferenceNumber);
                        }
                    }
                    break;
                }
                case "SiteName":
                {
                    foreach (ListCashOrderDto cashOrder in
                        cashOrders.Where(e => string.IsNullOrEmpty(e.SiteName) == false).Distinct())
                    {
                        if (cashOrder.SiteName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashOrder.SiteName);
                        }
                    }
                    break;
                }
                case "DeliveryDate":
                {
                    foreach (ListCashOrderDto cashOrder in
                        cashOrders.Where(e => string.IsNullOrEmpty(e.DeliveryDate) == false).Distinct())
                    {
                        if (cashOrder.DeliveryDate.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashOrder.DeliveryDate);
                        }
                    }
                    break;
                }
                case "OrderType":
                {
                    foreach (ListCashOrderDto cashOrder in
                        cashOrders.Where(e => string.IsNullOrEmpty(e.OrderType) == false).Distinct())
                    {
                        if (cashOrder.OrderType.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cashOrder.OrderType);
                        }
                    }
                    break;
                }
            }
            IEnumerable<string> itemList = items.Select(e => e).Distinct();
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region List Constructor Actions - CashOrderTasks 

        /// <summary>
        /// </summary>
        /// <returns>Redirect to Approval Home Page</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVFinanceReviewer")]
        public ActionResult TaskList()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<ListCashOrderTaskDto> coApprovals = _cashOrderApprovalValidation.All();
                return View(coApprovals);
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region CashOrderTasks - ListView Actions

        /// <summary>
        ///     DropDowns for Approval
        /// </summary>
        /// <returns></returns>
        public ActionResult CashOrderApprovalColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Reference Number", Tag = "ReferenceNumber"},
                new DropDownModel {Id = 3, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 4, Name = "User Name", Tag = "UserName"},
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     AutoComplete as User is typing......
        /// </summary>
        /// <param name="columName">User Text as is Typing....</param>
        /// <param name="searchData">Display suggestion Based on what is Typing....</param>
        /// <returns>Return user Result Information</returns>
        public JsonResult AutoCompleteCashOrderApprovalByColumn(string columName, string searchData)
        {
            List<ListCashOrderTaskDto> approvals = _cashOrderApprovalValidation.All().ToList();

            var items = new List<string>();

            switch (columName)
            {
                case "ReferenceNumber":
                    {
                        foreach (
                            ListCashOrderTaskDto approval in approvals.Where(e => string.IsNullOrEmpty(e.ReferenceNumber) == false))
                        {
                            if (approval.ReferenceNumber.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(approval.ReferenceNumber);
                            }
                        }
                        break;
                    }
                case "SiteName":
                    {
                        foreach (
                            ListCashOrderTaskDto approval in approvals.Where(e => string.IsNullOrEmpty(e.SiteName) == false))
                        {
                            if (approval.SiteName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(approval.SiteName);
                            }
                        }
                        break;
                    }
            }

            return Json(items.Distinct(), JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        private void TrimCashOrder(CashOrder cashOrder)
        {
            foreach (CashOrderContainerDrop containerDrop in cashOrder.CashOrderContainer.CashOrderContainerDrops)
            {
                containerDrop.CashOrderContainer = null;

                foreach (CashOrderContainerDropItem item in containerDrop.CashOrderContainerDropItems)
                {
                    item.CashOrderContainerDrop = null;
                }
            }
        }
        
        /// <summary>
        /// Reprint cash orders
        /// </summary>
        /// <param name="id"></param>
        //[CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        [AllowAnonymous]
        public FileResult ReprintFileDownload(int id)
        {
            try
            {
                var cashOrder = _cashOrderingValidation.Find(id).EntityResult;

                // Attach CashOrder Slip
                const string reportPath = "/CashOrder/CashOrderSlip";
                byte[] bytes = PdfGeneration(cashOrder.CashOrderId, reportPath);

                // send the PDF file to browser
                FileResult fileResult = new FileContentResult(bytes, "application/pdf");
                fileResult.FileDownloadName = "CashOrderSlip.pdf";

                return fileResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        #region Reprint Cash Order in PDF
        /// <summary>
        /// Reprint deposit slip
        /// </summary>
        /// <param name="id"></param>
        //[CustomAuthorize(Roles = "RetailUser, SBVTeller, SBVAdmin")]
        [AllowAnonymous]
        public ActionResult ReprintCashOrder(int id)
        {
            try
            {
                var cashOrder = _cashOrderingValidation.Find(id).EntityResult;

                // Attach CashOrder Slip
                const string reportPath = "/CashOrder/CashOrderSlip";
                byte[] bytes = PdfGeneration(cashOrder.CashOrderId, reportPath);

                var stream = new MemoryStream(bytes);
                return new FileStreamResult(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region generate CashOrder Slip

        /// <summary>
        ///     Email Cash order Slip
        /// </summary>
        /// <param name="cashOrder">Database Entity instance</param>
        /// <param name="user">user</param>
        private void EmailCashOrderSlip(CashOrderDto cashOrder, User user)
        {
            var mailMessage = new MailMessage();

            // Attach CashOrder Slip
            const string reportPath = "/CashOrder/CashOrderSlip";

            byte[] reportAttachmentBytes = PdfGeneration(cashOrder.CashOrderId, reportPath);
            var ms = new MemoryStream(reportAttachmentBytes);
            var attachement = new Attachment(ms, "CashOrderSlip.pdf");
            mailMessage.Attachments.Add(attachement);

            // Send CashOrder Slip via Email.
            SendEmail(mailMessage, cashOrder, user);
            ShowMessage("An Email with Cash order details was sent to your mailbox.", MessageType.success, "Email.");
        }


        /// <summary>
        ///     Send email
        /// </summary>
        /// <param name="message">Message in the email</param>
        /// <param name="cashOrder"> The order</param>
        /// <param name="user">user</param>
        private void SendEmail(MailMessage message, CashOrderDto cashOrder, User user)
        {
            var reviewerWriter = new StringWriter();
            var reviewerhtml = new HtmlTextWriter(reviewerWriter);

            var site = _lookup.GetSite(cashOrder.SiteId);

            string subject = "CashOrder RefNo_" + cashOrder.ReferenceNumber;

            if (cashOrder.StatusName == "Processed")
                subject = "CashOrder Processed RefNo_" + cashOrder.ReferenceNumber;

            // NOTE: 
            // Email sent to the Finance Reviewer
            var financeReviewersEmailAddresses = _lookup.GetFinanceReviewerEmail();

            if (_lookup.IsEft(cashOrder.CashOrderTypeId) && cashOrder.IsProcessed != true)
            {
                if (financeReviewersEmailAddresses.Any())
                {
                    var emailHeading = "The following Cash Order has been submitted for approval";
                    
                    reviewerhtml.RenderBeginTag(HtmlTextWriterTag.H2);
                    reviewerhtml.WriteEncodedText("mySBV.deposit EFT Cash Order");
                    reviewerhtml.RenderEndTag();
                    reviewerhtml.RenderBeginTag(HtmlTextWriterTag.Hr);
                    reviewerhtml.RenderEndTag();
                    reviewerhtml.WriteBreak();
                    reviewerhtml.RenderBeginTag(HtmlTextWriterTag.P);
                    reviewerhtml.WriteEncodedText(emailHeading);
                    reviewerhtml.WriteBreak();
                    reviewerhtml.WriteBreak();
                    reviewerhtml.WriteEncodedText(string.Format("Transaction Reference Number : {0}",
                                                cashOrder.ReferenceNumber));
                    reviewerhtml.WriteBreak();
                    reviewerhtml.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                    reviewerhtml.WriteBreak();
                    reviewerhtml.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                    reviewerhtml.WriteBreak();
                    reviewerhtml.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                    reviewerhtml.WriteBreak();
                    reviewerhtml.WriteBreak();

                    reviewerhtml.WriteEncodedText(
                        "Please check the SBV bank account if the EFT money has been received and approve the order via the following link");
                    reviewerhtml.WriteBreak();

                    string hostUrl = _lookup.GetServerAddress();
                    hostUrl += "/Account/Login/";

                    reviewerhtml.RenderBeginTag(HtmlTextWriterTag.A);
                    reviewerhtml.WriteEncodedText(hostUrl);

                    reviewerhtml.WriteBreak();
                    reviewerhtml.RenderEndTag();

                    reviewerhtml.Flush();
                    var reviewerHtmlString = reviewerWriter.ToString();
                    SendEmailsToMany(financeReviewersEmailAddresses, message, subject, reviewerHtmlString);
                }
            }


            // NOTE: 
            // Email sent to the Cash Center
            var emailAddresses = _lookup.GetEmailAddresses(cashOrder.SiteId, user);

            if (emailAddresses.Any())
            {
                var emailHeading = "The following Cash Order has been submitted";
                var mainHeader = "mySBV.deposit Cash Order";
                
                if (_lookup.IsEft(cashOrder.CashOrderTypeId))
                {
                    emailHeading = emailHeading + " for approval";
                    mainHeader = "mySBV.deposit EFT Cash Order";
                }

                if (cashOrder.StatusName == "Processed")
                {
                    emailHeading = "The following Cash Order has been Processed:";
                }
               
                var writer = new StringWriter();
                var html = new HtmlTextWriter(writer);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.WriteEncodedText(mainHeader);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Hr);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.WriteEncodedText(emailHeading);
                html.WriteBreak();
                    html.WriteBreak();
                    html.WriteEncodedText(string.Format("Transaction Reference Number : {0}", cashOrder.ReferenceNumber));
                    html.WriteBreak();
                    html.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                    html.WriteBreak();
                    html.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                    html.WriteBreak();
                    html.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                    html.WriteBreak();
                    html.WriteBreak();
                    html.WriteEncodedText("Please use the Transaction Reference Number to track a Cash Order.");
                html.RenderEndTag();
                html.Flush();

                    var htmlString = writer.ToString();
                SendEmailsToMany(emailAddresses, message, subject, htmlString);
            }


            // NOTE: 
            // Email sent to the Current Loqgged In User
            var currentUserAddress = user.EmailAddress;

            if (currentUserAddress.Any())
            {
                var emailHeading = "You have successfully submitted the following Cash Order:";

                if (cashOrder.StatusName == "Processed")
                    emailHeading = "You have successfully " + cashOrder.StatusName + " the following Cash Order:";

                var mainHeader = "mySBV.deposit Cash Order";
                if (_lookup.IsEft(cashOrder.CashOrderTypeId))
                {
                    mainHeader = "mySBV.deposit EFT Cash Order";
                }

                var writer = new StringWriter();
                var html = new HtmlTextWriter(writer);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.WriteEncodedText(mainHeader);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Hr);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.WriteEncodedText(emailHeading);
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Transaction Reference Number : {0}", cashOrder.ReferenceNumber));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText("Please use the Transaction Reference Number to track a Cash Order.");
                html.RenderEndTag();
                html.Flush();

                var htmlString = writer.ToString();
                SendEmailNotification(currentUserAddress, message, subject, htmlString);
            }
        }


        /// <summary>
        ///     Generate pdf
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="reportPath">The path of the Report</param>
        /// <returns></returns>
        public byte[] PdfGeneration(int id, string reportPath)
        {
            var paramz = new Dictionary<string, string>();
            paramz.Add("CashOrderId", id.ToString());
            byte[] result = GenerateReport(reportPath, paramz);
            return result;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        ///     Cash Order Hour
        /// </summary>
        /// <returns></returns>
        public int CashOrderHour()
        {
            int orderHour = DateTime.Now.Hour;
            return orderHour;
        }

        /// <summary>
        ///     Static Data
        /// </summary>
        private void StaticData()
        {
            ViewData.Add("dropDownListNote",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name"));
            ViewData.Add("dropDownListCoin",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name"));
            ViewData.Add("OrderTypes", new SelectList(_lookup.GetOrderTypes().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Sites", new SelectList(_lookup.GetSites(0).ToSitesDropDownModel(), "Id", "Name"));

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            ViewData.Add("Merchants", new SelectList(_lookup.GetMerchantsByTeller(user).ToDropDownModel(), "Id", "Name"));

            DateTime deliveryDate = _cashOrderingValidation.GetNextDeliveryDate();
            TimeSpan difference = deliveryDate.AddDays(7) - DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            ViewBag.DeliveryDate = deliveryDate.ToShortDateString();

            var cashOrderNumberOfDays = (int)difference.TotalDays;
            ViewBag.CashOrderNumberOfDays = cashOrderNumberOfDays;

            var numberOfMonthsToShowOnCalendar = (daysInMonth - DateTime.Now.Day <= 9) ? 2 : 1;
            ViewBag.NumberOfMonthsToShowOnCalendar = numberOfMonthsToShowOnCalendar;
        }

        /// <summary>
        /// Selected DropDowns
        /// </summary>
        /// <param name="cashOrder">Database entity instance</param>
        private void SelectedDropDowns(CashOrderDto cashOrder)
        {
            ViewData.Add("20000",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 20000));
            ViewData.Add("10000",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 10000));
            ViewData.Add("5000",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 5000));
            ViewData.Add("2000",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 2000));
            ViewData.Add("1000",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 1000));

            ViewData.Add("500",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 500));
            ViewData.Add("200",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 200));
            ViewData.Add("100",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 100));
            ViewData.Add("50",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 50));
            ViewData.Add("20",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 20));
            ViewData.Add("10",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 10));
            ViewData.Add("5",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 5));

            ViewData.Add("dropDownListNote",
                new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name"));
            ViewData.Add("dropDownListCoin",
                new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name"));

            ViewData.Add("OrderTypes",
                new SelectList(_lookup.GetOrderTypes().ToDropDownModel(), "Id", "Name", cashOrder.CashOrderTypeId));
            ViewData.Add("Sites", new SelectList(_lookup.Sites().ToSitesDropDownModel(), "Id", "Name", cashOrder.SiteId));

            User user = _cashOrderingValidation.GetLoggedUser(User.Identity.Name);
            ViewData.Add("Merchants", new SelectList(_lookup.GetMerchantsByTeller(user).ToDropDownModel(), "Id", "Name"));

            DateTime deliveryDate = _cashOrderingValidation.GetNextDeliveryDate();
            TimeSpan difference = deliveryDate.AddDays(7) - DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            if (cashOrder.CashOrderTypeId > 0)
            {
                if (_lookup.IsEft(cashOrder.CashOrderTypeId))
                {
                    // get attachments if EFT
                    ViewBag.Attachments = _cashOrderingValidation.GetFiles(cashOrder.CashOrderId);
                }
            }
            ViewBag.DeliveryDate = deliveryDate.ToShortDateString();

            ViewBag.CashOrderNumberOfDays = (int) difference.TotalDays;

            ViewBag.NumberOfMonthsToShowOnCalendar = (daysInMonth - DateTime.Now.Day <= 9) ? 2 : 1;
        }

        #endregion

        #region JSonResult functions

        /// <summary>
        ///     Get Merchant Site
        /// </summary>
        /// <param name="merchantId">id of the Merchant</param>
        /// <returns></returns>
        public JsonResult GetMerchantSites(int merchantId)
        {
            IEnumerable<DropDownModel> sites = _lookup.GetSites(merchantId).ToSitesDropDownModel();
            return Json(sites, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ApprovalEmails
  

        /// <summary>
        /// Approval Email to be sent through through to the Approver and the Client
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientName"></param>
        /// <param name="amount"></param>
        /// <param name="citCode"></param>
        /// <param name="taskRefNumber"></param>
        /// <returns></returns>
        private string ApprovalScreenMailNotification(string message, string clientName, 
                                            string citCode, string amount, string taskRefNumber)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.Table);

            html.RenderBeginTag(HtmlTextWriterTag.Strong);
            html.WriteEncodedText("mySBV.deposit EFT Cash Order");
            html.RenderEndTag();
            html.WriteBreak();
            html.WriteBreak();
            html.WriteBreak();

            html.WriteEncodedText(message);
            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText("Task Reference Number : " + taskRefNumber);
            html.WriteBreak();
            html.WriteEncodedText("Client Name: " + clientName);
            html.WriteBreak();
            html.WriteEncodedText("CIT Code: " + citCode);
            html.WriteBreak();
            html.WriteEncodedText("Amount: " + amount);

            html.WriteBreak();
            html.WriteBreak();
            
            html.RenderEndTag(); // End Table

            string htmlString = writer.ToString();
            return htmlString;
        }


        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="message">Message in the email</param>
        /// <param name="cashOrder"> The order</param>
        /// <param name="user">user</param>
        private void SendApprovalEmails(MailMessage message, CashOrderDto cashOrder, User user)
        {
            string subject = "CashOrder Approved - RefNo_" + cashOrder.ReferenceNumber;
            var site = _lookup.GetSite(cashOrder.SiteId);

            var emailBody = string.Empty;

            // NOTE: 
            // Email sent to the Finance Reviewer
            var financeReviewersEmailAddresses = _lookup.GetFinanceReviewerEmail();

            if (financeReviewersEmailAddresses.Any())
            {
                //Generate Email Body
                emailBody = ApprovalScreenMailNotification("The following EFT Cash Order has been approved:", site.Name,
                                        site.CitCode, cashOrder.CashOrderAmount.ToString(),
                                        cashOrder.ReferenceNumber);

                SendEmailsToMany(financeReviewersEmailAddresses, message, subject, emailBody);
            }


            // NOTE: 
            // Email sent to the Cash Center
            var emailAddresses = _lookup.GetEmailAddresses(cashOrder.SiteId, user);

            if (emailAddresses.Any())
            {
                emailBody = ApprovalScreenMailNotification("The following EFT Cash Order has been approved:", site.Name,
                                        site.CitCode, cashOrder.CashOrderAmount.ToString(),
                                        cashOrder.ReferenceNumber);

                SendEmailsToMany(emailAddresses, message, subject, emailBody);
            }


            //NOTE:
            //Email sent to Current user
           if (user.EmailAddress.Any())
            {
                emailBody = ApprovalScreenMailNotification("You have approved the following EFT Cash Order:", site.Name,
                                        site.CitCode, cashOrder.CashOrderAmount.ToString(),
                                        cashOrder.ReferenceNumber);

                SendEmailNotification(user.EmailAddress, new MailMessage(), subject, emailBody);
            }


        }


        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="message">Message in the email</param>
        /// <param name="cashOrder"> The order</param>
        /// <param name="user">user</param>
        private void SendDeclineEmails(MailMessage message, CashOrderDto cashOrder, User user)
        {
            string subject = "CashOrder Rejected - RefNo_" + cashOrder.ReferenceNumber;
            var site = _lookup.GetSite(cashOrder.SiteId);
            
            var client = _lookup.GetUserById(cashOrder.CreatedById);

            // NOTE: 
            // Email sent to the Finance Reviewer
            var financeReviewersEmailAddresses = _lookup.GetFinanceReviewerEmail();

            if (financeReviewersEmailAddresses.Any())
            {
                //Generate Email Body
                var writer = new StringWriter();
                var html = new HtmlTextWriter(writer);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.WriteEncodedText("mySBV.deposit EFT Cash Order");
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Hr);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.WriteEncodedText("The following EFT Cash Order has been rejected:");
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Transaction Reference Number : {0}", cashOrder.ReferenceNumber));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                html.WriteBreak();
                html.RenderEndTag();
                html.Flush();

                var htmlString = writer.ToString();
                SendEmailsToMany(financeReviewersEmailAddresses, message, subject, htmlString);
            }


            // NOTE: 
            // Email sent to the Cash Center
            var emailAddresses = _lookup.GetEmailAddresses(cashOrder.SiteId, user);

            if (emailAddresses.Any())
            {
                var writer = new StringWriter();
                var html = new HtmlTextWriter(writer);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.WriteEncodedText("mySBV.deposit EFT Cash Order");
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Hr);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.WriteEncodedText("The following EFT Cash Order has been rejected:");
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Transaction Reference Number : {0}", cashOrder.ReferenceNumber));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                html.WriteBreak();
                html.RenderEndTag();
                html.Flush();

                var htmlString = writer.ToString();
                SendEmailsToMany(emailAddresses, message, subject, htmlString);
            }


            //NOTE:
            //Email sent to Current user
            if (user.EmailAddress.Any())
            {
                var writer = new StringWriter();
                var html = new HtmlTextWriter(writer);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.WriteEncodedText("mySBV.deposit EFT Cash Order");
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Hr);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.WriteEncodedText("You have rejected the follwoing EFT Cash Order:");
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Transaction Reference Number : {0}", cashOrder.ReferenceNumber));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                html.WriteBreak();
                html.RenderEndTag();
                html.Flush();

                var htmlString = writer.ToString();
                SendEmailNotification(user.EmailAddress, message, subject, htmlString);
            }


            var clientUser = new List<string> {client.EmailAddress, site.ContactPersonEmailAddress1};

            // NOTE: 
            // Email sent to the Client
            if (clientUser.Any())
            {
                var writer = new StringWriter();
                var html = new HtmlTextWriter(writer);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.WriteEncodedText("mySBV.deposit EFT Cash Order");
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Hr);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.WriteEncodedText("The following EFT Cash Order has been rejected:");
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Transaction Reference Number : {0}", cashOrder.ReferenceNumber));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Client name : {0}", site.Name));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("CIT code : {0}", site.CitCode));
                html.WriteBreak();
                html.WriteEncodedText(string.Format("Amount : {0}", cashOrder.CashOrderAmount));
                html.WriteBreak();
                html.WriteBreak();
                html.WriteEncodedText("Please review your Cash Order request and resubmit via the following link:");
                html.RenderEndTag();

                string hostUrl = _lookup.GetServerAddress();
                hostUrl += "/Account/Login/";

                html.RenderBeginTag(HtmlTextWriterTag.A);
                html.WriteEncodedText(hostUrl);
                html.WriteBreak();
                html.RenderEndTag();

                html.Flush();

                var htmlString = writer.ToString();
                SendEmailsToMany(clientUser, message, subject, htmlString);
            }

        }


        /// <summary>
        /// Send email for Deletion
        /// </summary>
        /// <param name="message">Message in the email</param>
        /// <param name="cashOrder"> The order</param>
        /// <param name="user">user</param>
        /// <param name="taskRef">user</param>
        private void SendDeletionEmails(MailMessage message, CashOrderDto cashOrder, User user, string taskRef)
        {
            string subject = "CashOrder Deletion - RefNo_" + cashOrder.ReferenceNumber;
            var site = _lookup.GetSite(cashOrder.SiteId);

            var emailBody = string.Empty;

            // NOTE: 
            // Email sent to the Finance Reviewer
            var financeReviewersEmailAddresses = _lookup.GetFinanceReviewerEmail();

            if (financeReviewersEmailAddresses.Any())
            {
                //Generate Email Body
                emailBody = ApprovalScreenMailNotification("The following EFT Cash Order has been deleted:", site.Name,
                                        site.CitCode, cashOrder.CashOrderAmount.ToString(),
                                        taskRef);

                SendEmailsToMany(financeReviewersEmailAddresses, message, subject, emailBody);
            }


            // NOTE: 
            // Email sent to the Cash Center
            var emailAddresses = _lookup.GetEmailAddresses(cashOrder.SiteId, user);

            if (emailAddresses.Any())
            {
                emailBody = ApprovalScreenMailNotification("The following EFT Cash Order has been deleted:", site.Name,
                                        site.CitCode, cashOrder.CashOrderAmount.ToString(),
                                        cashOrder.ReferenceNumber);

                SendEmailsToMany(emailAddresses, message, subject, emailBody);
            }


            //NOTE:
            //Email sent to Current user
            if (user.EmailAddress.Any())
            {
                emailBody = ApprovalScreenMailNotification("You have deleted the following EFT Cash Order:", site.Name,
                                        site.CitCode, cashOrder.CashOrderAmount.ToString(),
                                        taskRef);

                SendEmailNotification(user.EmailAddress, new MailMessage(), subject, emailBody);
            }


        }



        #endregion
        
    }
}