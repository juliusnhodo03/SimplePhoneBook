using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.ProductType;
using Application.Modules.Maintanance.ProductType;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class ProductTypeController : BaseController
    {
        private readonly IProductTypeValidation _productValidation;

        public ProductTypeController()
        {
            _productValidation = LocalUnityResolver.Retrieve<IProductTypeValidation>();
        }



        #region Controller Actions

        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                var products = _productValidation.All();
                return View(products);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
            return View();
        }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Create(ProductTypeDto productTypeDto)
        {
            if (VerifyAuthentication())
            {
            if (string.IsNullOrWhiteSpace(productTypeDto.Name))
            {
                ModelState.AddModelError("Product Name", "Please enter Product Type Name");
                return View(productTypeDto);
            }

            if (_productValidation.IsProductTypeNameInUse(productTypeDto.Name))
            {
                ModelState.AddModelError("ProductName", "Product Name already exists on the system");
            }

            if (ModelState.IsValid)
            {
                if (_productValidation.Add(productTypeDto, User.Identity.Name).Status == MethodStatus.Successful)
                {
                    ShowMessage("Product Type added successfully", MessageType.success, "Save");

                    return RedirectToAction("Index");
                }
            }
            ShowMessage("Product Type not saved successfully", MessageType.error, "Error");
            return View(productTypeDto);
        }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
            var result = _productValidation.Find(id);
            if (result.Status == MethodStatus.Successful)
            {
                return View(result.EntityResult);
            }
            ShowMessage(result.Message, MessageType.error, "Edit Product Type");
            return RedirectToAction("Index");
        }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(ProductTypeDto productTypeDto)
        {
            if (VerifyAuthentication())
            {
            if (string.IsNullOrWhiteSpace(productTypeDto.Name))
            {
                ModelState.AddModelError("Product Name", "Please enter Product Type Name");
                return View(productTypeDto);
            }
            
            if (_productValidation.NameUsedByAnother(productTypeDto.Name, productTypeDto.ProductTypeId))
            {
                ModelState.AddModelError("", "");
                ShowMessage("You cannot give a name of another existing Product!", MessageType.error, "Update Failed");
            }

            if (ModelState.IsValid)
            {
                if (_productValidation.Edit(productTypeDto, User.Identity.Name).Status == MethodStatus.Successful)
                {
                    ShowMessage("Product Details updated successfully", MessageType.success, "Saved");

                    return RedirectToAction("Index");
                }
            }
            return View(productTypeDto);
        }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
            var result = _productValidation.Find(id);
            if (result.Status == MethodStatus.Successful)
            {
                return View(result.EntityResult);
            }
            ShowMessage(result.Message, MessageType.error, "View Product Type");
            return RedirectToAction("Index");
        }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Delete(int id = 0)
        {
           if (VerifyAuthentication())
            {
                var results = _productValidation.Delete(id, User.Identity.Name);
            if (results.Status == MethodStatus.Successful)
                ShowMessage("Product Type deleted successfully", MessageType.success, "Deleted");
            else
            {
                ShowMessage(results.Message, MessageType.error, "Delete Product Type");
            }
            return Json(new { url = Url.Action("Index") });
        }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region ListView Actions

        public ActionResult ProductTypeColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Product Name", Tag = "Name"},
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteProductByColumn(string columName, string searchData)
        {
           var products = _productValidation.All().ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                    {
                        foreach (ProductTypeDto product in products.Where(e => string.IsNullOrEmpty(e.Name) == false)
                            )
                        {
                            if (product.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(product.Name);
                            }
                        }
                        break;
                    }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion  
    }
}