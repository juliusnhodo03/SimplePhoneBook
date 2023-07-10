using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Application.Dto.Product;
using Application.Modules.Common;
using Application.Modules.Maintanance.Product;
using Application.Modules.Maintanance.Site;
using Domain.Data.Model;
using Domain.Repository;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
    public class ProductController : BaseController
    {
        private readonly ILookup _lookup;
        private readonly IRepository _repository;
        private readonly IProductValidation _productValidation;
        private readonly ISiteValidation _siteValidation;

        public ProductController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _productValidation = LocalUnityResolver.Retrieve<IProductValidation>();
            _repository = LocalUnityResolver.Retrieve<IRepository>();
            _siteValidation = LocalUnityResolver.Retrieve<ISiteValidation>();
        }



        #region Product

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Index(int load = 0)
        {
            if (VerifyAuthentication())
            {
                var sites = _productValidation.All();
                //if (load == 1)
                //{
                //    ShowMessage("Product saved successfully", MessageType.success, "Save Product");
                //    return View(sites);
                //}
                //else if (load == 2)
                //{
                //    ShowMessage("Product updated successfully", MessageType.success, "Update Product");
                //    return View(sites);
                //}
                return View(sites);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Create(int siteId = 0)
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();
                SiteProductDto siteProductDto;

                if (siteId > 0)
                {
                    siteProductDto = new SiteProductDto
                    {
                        SiteId = siteId,
                        CitCode = _siteValidation.Find(siteId).EntityResult.CitCode
                    };
                }
                else
                {
                    siteProductDto = new SiteProductDto
                    {
                        SiteId = siteId,
                        CitCode = ""
                    };
                }

                return View(siteProductDto);
            }
            return RedirectToAction("Login", "Account");
        }

       [HttpPost]
        public ActionResult Create(ProductsHolderDto productsHolder)
        {
           if (VerifyAuthentication())
           {
               ModelState.Clear();

               string modelErrorMessage = string.Empty;

               //Check For duplicates
               foreach (var productDto in productsHolder.Products)
               {
                   //Check that the Serial Number already Exists before Inserting
                   if (_productValidation.IsSerialNumberInUse(productDto.SerialNumber))
                   {
                       modelErrorMessage += string.Format("Serial Number [{0}] already exist in the System.", productDto.SerialNumber) + " <br/> ";
                       ModelState.AddModelError(productDto.SerialNumber, modelErrorMessage);
                   }


                   if (productDto.FeeCodes != null)
                   {
                       var result = IsFeeCodeDuplicated(productDto);
                       foreach (var feeCode in result)
                       {
                           modelErrorMessage += string.Format("Fee Code [{0}] is duplicated.", feeCode) + " <br/> ";
                           ModelState.AddModelError(feeCode, modelErrorMessage);
                       }
                   }
               }

               if (ModelState.IsValid)
               {
                   foreach (var product in productsHolder.Products) // Loop through List with foreach
                   {
                       if (!string.IsNullOrWhiteSpace(product.TerminationDateString))
                       {
                           if (product.TerminationDateString.Substring(0, 10) != "01-01-0001")
                           {
                               product.TerminationDate = Convert.ToDateTime(product.TerminationDateString);
                           }
                       }

                       product.StatusId = _lookup.GetStatusId("ACTIVE");
                       var result = _productValidation.Add(product, User.Identity.Name);
                       if (result.Status == MethodStatus.Successful)
                       {
                          ShowMessage("Product saved successfully", MessageType.success, "Save Product");
                          return Json(new { Url = Url.Action("Index"), Message = "" }, JsonRequestBehavior.AllowGet);
                       }
                   }
               }

               ShowMessage(modelErrorMessage, MessageType.error, "Add Product" );

               PrepareDropDowns();
               return Json(new { Url = "", Message = modelErrorMessage }, JsonRequestBehavior.AllowGet);
               
           }
           return RedirectToAction("Login", "Account");
        }

        public ActionResult Edit(int id)
        {
            if (id > 0)
            {
               PrepareDropDowns();
               
               ProductDto productDto;
               var product = _repository.Find<Product>(id);
               var site = _repository.Find<Site>(product.SiteId);
                

                //GET FEE VALUES
                var monthlyCitFeeCode = GetFeeValues(id, "CIT_FEE");
                var monthlyRiskFeeFixed = GetFeeValues(id, "MONTHLY_RISK_FEE_FIXED_FEE");
                var monthlyRiskFeeVariable = GetFeeValues(id, "CIT_RISK_FEE_VARIABLE");
                var citAdHocFee = GetFeeValues(id, "CIT_ADHOC_FEE");
                var maxCashDailyValue = GetFeeValues(id, "MAX_CASH_DAILY_VALUE_FEE");
                var allInclusiveProductFeeCode = GetFeeValues(id, "ALL_INCLUSIVE_FEE_VARIABLE");
                var cashProcessingFee = GetFeeValues(id, "CASH_PROCESSING_FEE");
                var cashDeviceRentalFee = GetFeeValues(id, "CASH_DEVICE_RENTAL_FEE");
                var cashDeviceRiskCover = GetFeeValues(id, "CASH_DEVICE_RISK_COVER");
                var cashInDeviceRiskCover = GetFeeValues(id, "CASH_IN_DEVICE");
                var deviceTrainingFee = GetFeeValues(id, "DEVICE_TRAINING_FEE");
                var cashInstallationFee = GetFeeValues(id, "DEVICE_INSTALLATION_FEE");
                var deviceMovementFee = GetFeeValues(id, "DEVICE_MOVEMENT_FEE");
                var cashOrderFee = GetFeeValues(id, "CASH_ORDER_FEE");
                var monthlySubscriptionFee = GetFeeValues(id, "MONTHLY_SUBSCRIPTION_FEE");
                var minMonthlyFee = GetFeeValues(id, "MINIMUM_MONTHLY_FEE");
                var trainingFee = GetFeeValues(id, "TRAINING_FEE");
                var penaltyFee = GetFeeValues(id, "PENALTY_FEE");
                var volumeBreakerValueMin = GetFeeValues(id, "VOLUME_BREAKER_VALUE_MIN");
                var volumeBreakerValueMax = GetFeeValues(id, "VOLUME_BREAKER_VALUE_MAX");
                var volumeBreakerFeeBelow = GetFeeValues(id, "VOLUME_BREAKER_FEE_ABOVE");
                var volumeBreakerFeeAbove = GetFeeValues(id, "VOULME_BREAKER_FEE_BELOW");
                var vaultCashProcessingFee = GetFeeValues(id, " VAULT_CASH_PROCESSING_FEE");

                productDto = new ProductDto
               {
                   ProductId = product.ProductId,
                   ProductTypeId = product.ProductTypeId,
                   SiteId = product.SiteId,
                   ServiceTypeId = product.ServiceTypeId,
                   SettlementTypeId = product.SettlementTypeId,
                   PublicHolidayInclInFeeFlag = product.PublicHolidayInclInFeeFlag,
                   ImplementationDateString = product.ImplementationDate.ToString("dd MMMM yyyy"),
                   CitCode = site.CitCode,
                   
                   
                   //FEES AND CODES
                   //CIT
                   MonthlyCitFeeCode = monthlyCitFeeCode.Code,
                   MonthlyCitFee = monthlyCitFeeCode.Value,
                   MonthlyRiskFeeFixedCode = monthlyRiskFeeFixed.Code,
                   MonthlyRiskFeeFixedFee = monthlyRiskFeeFixed.Value,
                   MonthlyRiskFeeVariable = monthlyRiskFeeVariable.Code,
                   MonthlyRiskFeeVariableFee = monthlyRiskFeeVariable.Value,
                   CitAdHocFeeCode = citAdHocFee.Code,
                   CitAdHocFee = citAdHocFee.Value,
                   MaxCashDailyValueCode = maxCashDailyValue.Code,
                   MaxCashDailyValueFee = maxCashDailyValue.Value,
                   AllInclusiveProductFeeCode = allInclusiveProductFeeCode.Code,
                   AllInclusiveProductFee = allInclusiveProductFeeCode.Value,
                   
                   ////VAULT
                   VaultCashProcessingFeeCode = vaultCashProcessingFee.Code,
                   VaultCashProcessingFee = vaultCashProcessingFee.Value,
                   CashDeviceRentalFeeCode = cashDeviceRentalFee.Code,
                   CashDeviceRentalFee = cashDeviceRentalFee.Value,
                   CashDeviceRiskCoverCode = cashDeviceRiskCover.Code,
                   CashDeviceRiskCoverFee = cashDeviceRiskCover.Value,
                   CashInDeviceRiskCoverCode = cashInDeviceRiskCover.Code,
                   CashInDeviceRiskCoverFee = cashInDeviceRiskCover.Value,
                   DeviceTrainingFeeCode = deviceTrainingFee.Code,
                   DeviceTrainingFee = deviceTrainingFee.Value,
                   CashInstallationFeeCode = cashInstallationFee.Code,
                   CashInstallationFee = cashInstallationFee.Value,
                   DeviceMovementFeeCode = deviceMovementFee.Code,
                   DeviceMovementFee = deviceMovementFee.Value,
                   
                   //DEPOSIT
                   CashProcessingFeeCode = cashProcessingFee.Code,
                   CashProcessingFee = cashProcessingFee.Value,
                   CashOrderFeeCode = cashOrderFee.Code,
                   CashOrderFee = cashOrderFee.Value,
                   MonthlySubscriptionFeeCode = monthlySubscriptionFee.Code,
                   MonthlySubscriptionFee = monthlySubscriptionFee.Value,
                   MinMonthlyFeeCode = minMonthlyFee.Code,
                   MinMonthlyFee = minMonthlyFee.Value,
                   TrainingFeeCode = trainingFee.Code,
                   TrainingFee = trainingFee.Value,
                   PenaltyFeeCode = penaltyFee.Code,
                   PenaltyFee = penaltyFee.Value,
                   VolumeBreakerValueMinCode = volumeBreakerValueMin.Code,
                   VolumeBreakerValueMinFee = volumeBreakerValueMin.Value,
                   VolumeBreakerValueMaxCode = volumeBreakerValueMax.Code,
                   VolumeBreakerValueMaxFee = volumeBreakerValueMax.Value,
                   VolumeBreakerFeeBelowCode = volumeBreakerFeeBelow.Code,
                   VolumeBreakerFeeBelowFee = volumeBreakerFeeBelow.Value,
                   VolumeBreakerFeeAboveCode = volumeBreakerFeeAbove.Code ,
                   VolumeBreakerFeeAboveFee = volumeBreakerFeeAbove.Value,
               };

               
                if (product.TerminationDate != null)
                    productDto.TerminationDateString = product.TerminationDate.Value.ToString("dd MMMM yyyy");
                
                if (product.ProductTypeId == 2)
                {
                    if (product.DeviceTypeId != null && product.DeviceId != null)
                    {
                        var deviceType = _repository.Find<DeviceType>(product.DeviceTypeId.Value);
                        var device = _repository.Find<Device>(product.DeviceId.Value);
                        productDto.DeviceSupplierId = deviceType.SupplierId;

                        productDto.DeviceId = product.DeviceId;
                        productDto.DeviceTypeId = deviceType.DeviceTypeId;
                        productDto.SerialNumber = device.SerialNumber;
                    }
                    
                }


               return View(productDto);
            }
            ShowMessage("Product record not found", MessageType.error, "Edit Product");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(ProductsHolderDto productsHolder)
        {
            if (VerifyAuthentication())
            {
                ModelState.Clear();

                if (ModelState.IsValid)
                {
                    foreach (var product in productsHolder.Products) // Loop through List with foreach
                    {
                        product.StatusId = _lookup.GetStatusId("ACTIVE");

                        if (!string.IsNullOrWhiteSpace(product.TerminationDateString))
                        {
                            if (product.TerminationDateString.Substring(0, 10) != "01-01-0001")
                            {
                                product.TerminationDate = Convert.ToDateTime(product.TerminationDateString);
                            }
                        }

                        //Check that the Serial Number already exists before inserting
                        if (product.DeviceId != null && _productValidation.SerialNumberUsedByAnotherDevice(product.SerialNumber, product.DeviceId.Value))
                        {
                            ShowMessage(string.Format("Serial Number [{0}] already in use by another Product.", product.SerialNumber), MessageType.error, "Error");
                            return Json(new { Url = Url.Action("Index"), Message = "" }, JsonRequestBehavior.AllowGet);
                        }

                        //Check for Duplicate Product Codes
                        if (product.FeeCodes != null)
                        {
                            var result = IsFeeCodeDuplicated(product);
                            foreach (var feeCode in result)
                            {
                                ShowMessage(string.Format("Fee Code [{0}] is duplicated.", feeCode) + " <br/> ", MessageType.error, "Error");
                                return Json(new { Url = Url.Action("Index"), Message = "" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        
                        var results = _productValidation.Edit(product, User.Identity.Name);
                        if (results.Status == MethodStatus.Successful)
                        {
                            ShowMessage("Product saved successfully", MessageType.success, "Save Product Account");
                            return Json(new { Url = Url.Action("Index"), Message = "" }, JsonRequestBehavior.AllowGet);
                        }
                        
                    }
                }

                PrepareDropDowns();
                ShowMessage("Product not saved successfully", MessageType.error, "Error");
                return Json(new { Url = Url.Action("Index"), Message = "" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
        public ActionResult View(int id)
        {
            if (id > 0)
            {
                PrepareDropDowns();

                ProductDto productDto;
                var product = _repository.Find<Product>(id);
                var site = _repository.Find<Site>(product.SiteId);

                //GET FEE VALUES
                var monthlyCitFeeCode = GetFeeValues(id, "CIT_FEE");
                var monthlyRiskFeeFixed = GetFeeValues(id, "MONTHLY_RISK_FEE_FIXED_FEE");
                var monthlyRiskFeeVariable = GetFeeValues(id, "CIT_RISK_FEE_VARIABLE");
                var citAdHocFee = GetFeeValues(id, "CIT_ADHOC_FEE");
                var maxCashDailyValue = GetFeeValues(id, "MAX_CASH_DAILY_VALUE_FEE");
                var allInclusiveProductFeeCode = GetFeeValues(id, "ALL_INCLUSIVE_FEE_VARIABLE");
                var cashProcessingFee = GetFeeValues(id, "CASH_PROCESSING_FEE");
                var cashDeviceRentalFee = GetFeeValues(id, "CASH_DEVICE_RENTAL_FEE");
                var cashDeviceRiskCover = GetFeeValues(id, "CASH_DEVICE_RISK_COVER");
                var cashInDeviceRiskCover = GetFeeValues(id, "CASH_IN_DEVICE");
                var deviceTrainingFee = GetFeeValues(id, "DEVICE_TRAINING_FEE");
                var cashInstallationFee = GetFeeValues(id, "DEVICE_INSTALLATION_FEE");
                var deviceMovementFee = GetFeeValues(id, "DEVICE_MOVEMENT_FEE");
                var cashOrderFee = GetFeeValues(id, "CASH_ORDER_FEE");
                var monthlySubscriptionFee = GetFeeValues(id, "MONTHLY_SUBSCRIPTION_FEE");
                var minMonthlyFee = GetFeeValues(id, "MINIMUM_MONTHLY_FEE");
                var trainingFee = GetFeeValues(id, "TRAINING_FEE");
                var penaltyFee = GetFeeValues(id, "PENALTY_FEE");
                var volumeBreakerValueMin = GetFeeValues(id, "VOLUME_BREAKER_VALUE_MIN");
                var volumeBreakerValueMax = GetFeeValues(id, "VOLUME_BREAKER_VALUE_MAX");
                var volumeBreakerFeeBelow = GetFeeValues(id, "VOLUME_BREAKER_FEE_ABOVE");
                var volumeBreakerFeeAbove = GetFeeValues(id, "VOULME_BREAKER_FEE_BELOW");

                productDto = new ProductDto
                {
                    ProductId = product.ProductId,
                    ProductTypeId = product.ProductTypeId,
                    SiteId = product.SiteId,
                    ServiceTypeId = product.ServiceTypeId,
                    SettlementTypeId = product.SettlementTypeId,
                    PublicHolidayInclInFeeFlag = product.PublicHolidayInclInFeeFlag,
                    ImplementationDateString = product.ImplementationDate.ToString("dd MMMM yyyy"),
                    CitCode = site.CitCode,


                    //FEES AND CODES
                    //CIT
                    MonthlyCitFeeCode = monthlyCitFeeCode.Code,
                    MonthlyCitFee = monthlyCitFeeCode.Value,
                    MonthlyRiskFeeFixedCode = monthlyRiskFeeFixed.Code,
                    MonthlyRiskFeeFixedFee = monthlyRiskFeeFixed.Value,
                    MonthlyRiskFeeVariable = monthlyRiskFeeVariable.Code,
                    MonthlyRiskFeeVariableFee = monthlyRiskFeeVariable.Value,
                    CitAdHocFeeCode = citAdHocFee.Code,
                    CitAdHocFee = citAdHocFee.Value,
                    MaxCashDailyValueCode = maxCashDailyValue.Code,
                    MaxCashDailyValueFee = maxCashDailyValue.Value,
                    AllInclusiveProductFeeCode = allInclusiveProductFeeCode.Code,
                    AllInclusiveProductFee = allInclusiveProductFeeCode.Value,

                    ////VAULT
                    VaultCashProcessingFeeCode = cashProcessingFee.Code,
                    VaultCashProcessingFee = cashProcessingFee.Value,
                    CashDeviceRentalFeeCode = cashDeviceRentalFee.Code,
                    CashDeviceRentalFee = cashDeviceRentalFee.Value,
                    CashDeviceRiskCoverCode = cashDeviceRiskCover.Code,
                    CashDeviceRiskCoverFee = cashDeviceRiskCover.Value,
                    CashInDeviceRiskCoverCode = cashInDeviceRiskCover.Code,
                    CashInDeviceRiskCoverFee = cashInDeviceRiskCover.Value,
                    DeviceTrainingFeeCode = deviceTrainingFee.Code,
                    DeviceTrainingFee = deviceTrainingFee.Value,
                    CashInstallationFeeCode = cashInstallationFee.Code,
                    CashInstallationFee = cashInstallationFee.Value,
                    DeviceMovementFeeCode = deviceMovementFee.Code,
                    DeviceMovementFee = deviceMovementFee.Value,


                    //DEPOSIT
                    CashProcessingFeeCode = cashProcessingFee.Code,
                    CashProcessingFee = cashProcessingFee.Value,
                    CashOrderFeeCode = cashOrderFee.Code,
                    CashOrderFee = cashOrderFee.Value,
                    MonthlySubscriptionFeeCode = monthlySubscriptionFee.Code,
                    MonthlySubscriptionFee = monthlySubscriptionFee.Value,
                    MinMonthlyFeeCode = minMonthlyFee.Code,
                    MinMonthlyFee = minMonthlyFee.Value,
                    TrainingFeeCode = trainingFee.Code,
                    TrainingFee = trainingFee.Value,
                    PenaltyFeeCode = penaltyFee.Code,
                    PenaltyFee = penaltyFee.Value,
                    VolumeBreakerValueMinCode = volumeBreakerValueMin.Code,
                    VolumeBreakerValueMinFee = volumeBreakerValueMin.Value,
                    VolumeBreakerValueMaxCode = volumeBreakerValueMax.Code,
                    VolumeBreakerValueMaxFee = volumeBreakerValueMax.Value,
                    VolumeBreakerFeeBelowCode = volumeBreakerFeeBelow.Code,
                    VolumeBreakerFeeBelowFee = volumeBreakerFeeBelow.Value,
                    VolumeBreakerFeeAboveCode = volumeBreakerFeeAbove.Code,
                    VolumeBreakerFeeAboveFee = volumeBreakerFeeAbove.Value,
                };


                if (product.TerminationDate != null)
                    productDto.TerminationDateString = product.TerminationDate.Value.ToString("dd MMMM yyyy");

                if (product.ProductTypeId == 2)
                {
                    if (product.DeviceTypeId != null && product.DeviceId != null)
                    {
                        var deviceType = _repository.Find<DeviceType>(product.DeviceTypeId.Value);
                        var device = _repository.Find<Device>(product.DeviceId.Value);
                        productDto.DeviceSupplierId = deviceType.SupplierId;

                        productDto.DeviceId = product.DeviceId;
                        productDto.DeviceTypeId = deviceType.DeviceTypeId;
                        productDto.SerialNumber = device.SerialNumber;
                    }

                }
                
                return View(productDto);
            }
            ShowMessage("Product record not found", MessageType.error, "View Product");
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (VerifyAuthentication())
            {
                var deleteProductInfo = _productValidation.Delete(id, User.Identity.Name);
                if (deleteProductInfo.Status == MethodStatus.Successful)
                    ShowMessage("Product deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage(deleteProductInfo.Message, MessageType.error, "Delete Product");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        private void PrepareDropDowns()
        {
            ViewData.Add("ProductTypes", new SelectList(_lookup.ProductTypes().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Sites", new SelectList(_lookup.Sites().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("ServiceTypes", new SelectList(_lookup.ServiceTypes().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("SettlementTypes", new SelectList(_lookup.SettlementTypes().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Fees", _lookup.Fees().Take(5).ToFeeModel());
            ViewData.Add("Suppliers", new SelectList(_lookup.Suppliers().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("devices", new SelectList(_lookup.DeviceTypes().ToDropDownModel(), "Id", "Name"));
        }

        public ActionResult VaultFeePartial()
        {
            PrepareDropDowns();
           
            return View(new ProductDto());
        }

        public ActionResult DepositFeePartial()
        {
            PrepareDropDowns();
            return View(new ProductDto());
        }

        public ActionResult CitFeePartial()
        {
            PrepareDropDowns();
            return View(new ProductDto());
        }

        public ActionResult ProductSelectionPartial(SiteProductDto siteProductDto)
        {
            PrepareDropDowns();
            var product = new ProductDto();
            return View(product);
        }

        public ActionResult GetDevices(string supplierId)
        {
            var convertedSupplierId = Convert.ToInt32(supplierId);
            var deviceTypes = _lookup.DeviceTypes().Where(a => a.SupplierId == convertedSupplierId).ToList().Select(a => new 
            {
                text = a.Name,
                value = a.DeviceTypeId.ToString(CultureInfo.InvariantCulture),
            });

            return Json(deviceTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeesValue(string code)
        {
            var fee = _lookup.Fees().Where(a => a.Code == code).Select(a => new
            {
                id = a.FeeId.ToString(CultureInfo.InvariantCulture),
                value = a.Value.ToString(CultureInfo.InvariantCulture)
            });
            return Json(fee.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CitCode(int siteId)
        {
            Site site = _lookup.Sites().FirstOrDefault(a => a.SiteId == siteId);
            return Json(new
            {
                site.CitCode
            }, JsonRequestBehavior.AllowGet);
        }

        public string GetFees()
        {
            var json = new JavaScriptSerializer().Serialize(_lookup.Fees().ToFeeModel());
            return json;
        }

        public string GetFeeCode(int productId, string lookUpKey)
        {
            return (from pf in _repository.All<ProductFee>()
                    join f in _repository.All<Fee>() on pf.FeeId equals f.FeeId
                    where pf.ProductId == productId && f.LookUpKey == lookUpKey
                    select f.Code).FirstOrDefault();
        }

        public int GetFeeId(string code)
        {
            return _lookup.GetFee(code).FeeId;
        }

        public ProductFeeValues GetFeeValues(int productId, string lookUpKey)
        {
            ProductFeeValues productFeeValues = new ProductFeeValues();

            var fees = (from pf in _repository.All<ProductFee>()
                     join f in _repository.All<Fee>() on pf.FeeId equals f.FeeId
                     where pf.ProductId == productId && f.LookUpKey == lookUpKey && pf.IsActive == true
                     select f).FirstOrDefault();

            if (fees != null)
            {
                productFeeValues.Code = fees.Code;
                productFeeValues.Value = fees.Value;
            }
            return productFeeValues;
        }

        #endregion
        
        #region ListView Actions

        public ActionResult ProductColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Product Type", Tag = "ProductTypeName"},
                new DropDownModel {Id = 2, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 3, Name = "Service Type", Tag = "ServiceTypeName"},
                new DropDownModel {Id = 4, Name = "Settlement Type", Tag = "SettlementTypeName"},
                new DropDownModel {Id = 5, Name = "Status", Tag = "StatusName"},
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteProductByColumn(string columName, string searchData)
        {
            var products = _productValidation.All().ToList();

            //return Json(merchants, JsonRequestBehavior.AllowGet);

            var items = new ArrayList();

            switch (columName)
            {
                case "ProductTypeName":
                    {
                        foreach (
                            var product in products.Where(e => string.IsNullOrEmpty(e.ProductTypeName) == false))
                        {
                            if (product.ProductTypeName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(product.ProductTypeName);
                            }
                        }
                        break;
                    }
                case "SiteName":
                    {
                        foreach (
                            var product in products.Where(e => string.IsNullOrEmpty(e.SiteName) == false))
                        {
                            if (product.SiteName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(product.SiteName);
                            }
                        }
                        break;
                    }
                case "ServiceTypeName":
                    {
                        foreach (
                            var product in products.Where(e => string.IsNullOrEmpty(e.ServiceTypeName) == false))
                        {
                            if (product.ServiceTypeName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(product.ServiceTypeName);
                            }
                        }
                        break;
                    }
                case "SettlementTypeName":
                    {
                        foreach (
                            var product in products.Where(e => string.IsNullOrEmpty(e.SettlementTypeName) == false))
                        {
                            if (product.SettlementTypeName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(product.SettlementTypeName);
                            }
                        }
                        break;
                    }
                case "StatusName":
                    {
                        foreach (
                            var product in products.Where(e => string.IsNullOrEmpty(e.StatusName) == false))
                        {
                            if (product.StatusName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(product.StatusName);
                            }
                        }
                        break;
                    }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        public ActionResult Error()
        {
            return View("Error");
        }

        private List<string> IsFeeCodeDuplicated(ProductDto productDto)
        {
            var repeatedFeeCodes = new List<string>();

            foreach (var result in productDto.FeeCodes.GroupBy(feecode => feecode).Select(feeCode => new
            {
                FeeCode = feeCode,
                Count = feeCode.Count()
            }))
            {
                if (result.Count > 1)
                    repeatedFeeCodes.Add(result.FeeCode.Key);
            }

            return repeatedFeeCodes;
        }
    }
}
