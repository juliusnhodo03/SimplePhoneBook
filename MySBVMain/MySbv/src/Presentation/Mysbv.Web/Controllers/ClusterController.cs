using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.Cluster;
using Application.Modules.Common;
using Application.Modules.Maintanance.Cluster;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Cluster related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class ClusterController : BaseController
    {
        private readonly IClusterValidation _clusterValidation;
        private readonly ILookup _staticTypes;

        public ClusterController()
        {
            _staticTypes = LocalUnityResolver.Retrieve<ILookup>();
            _clusterValidation = LocalUnityResolver.Retrieve<IClusterValidation>();
        }



        #region Controller Actions

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<ClusterDto> clusters = _clusterValidation.All();
                return View(clusters);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <param name="clusterDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ClusterDto clusterDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(clusterDto.Name))
                {
                    ModelState.AddModelError("Cluster Name", "Please enter cluster name");
                    return View(clusterDto);
                }

                if (_clusterValidation.IsClusterNameInUse(clusterDto.Name))
                {
                    ModelState.AddModelError("ClusterName", "Cluster Name already exists on the system");
                }

                if (ModelState.IsValid)
                {
                    if (_clusterValidation.Add(clusterDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Cluster added successfully", MessageType.success, "Save");
                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Cluster not saved successfully", MessageType.error, "Error");
                return View(clusterDto);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Edit
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                MethodResult<ClusterDto> result = _clusterValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Cluster");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Edit
        /// </summary>
        /// <param name="clusterDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ClusterDto clusterDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(clusterDto.Name))
                {
                    ModelState.AddModelError("Cluster Name", "Please enter cluster name");
                    return View(clusterDto);
                }

                if (_clusterValidation.NameUsedByAnotherCluster(clusterDto.Name, clusterDto.ClusterId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a name of another existing cluster!", MessageType.error,
                        "Update Failed");
                }

                if (ModelState.IsValid)
                {
                    if (_clusterValidation.Edit(clusterDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Cluster Details updated successfully", MessageType.success, "Saved");

                        return RedirectToAction("Index");
                    }
                }
                return View(clusterDto);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     View
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                MethodResult<ClusterDto> result = _clusterValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Cluster");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                MethodResult<bool> results = _clusterValidation.Delete(id, User.Identity.Name);
                if (results.Status == MethodStatus.Successful)
                    ShowMessage("Cluster deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage(results.Message, MessageType.error, "Delete Cluster");
                }
                return Json(new {url = Url.Action("Index")});
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region ListView Action

        /// <summary>
        ///     Cluster Column Listing
        /// </summary>
        /// <returns></returns>
        public ActionResult ClusterColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Cluster Name", Tag = "Name"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     AutoComplete Cluster By Column
        /// </summary>
        /// <param name="columName">Column name</param>
        /// <param name="searchData"> Search String</param>
        /// <returns></returns>
        public JsonResult AutoCompleteClusterByColumn(string columName, string searchData)
        {
            List<ClusterDto> clusters = _clusterValidation.All().ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                {
                    foreach (ClusterDto cluster in clusters.Where(e => string.IsNullOrEmpty(e.Name) == false))
                    {
                        if (cluster.Name.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(cluster.Name);
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