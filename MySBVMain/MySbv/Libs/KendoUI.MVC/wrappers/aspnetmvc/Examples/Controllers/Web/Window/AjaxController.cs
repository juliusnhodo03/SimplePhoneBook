﻿using System.Web.Mvc;

namespace Kendo.Mvc.Examples.Controllers
{
    public partial class WindowController : Controller
    {
        public ActionResult Ajax()
        {
            return View();
        }

        public ActionResult AjaxContent()
        {
            return View();
        }

        public ActionResult AjaxContent1()
        {
            return PartialView();
        }
    }
}