﻿using System.Web.Mvc;
using Kendo.Mvc.Examples.Models;

namespace Kendo.Mvc.Examples.Controllers
{
    public partial class GridController : Controller
    {
        public ActionResult ServerBinding()
        {
            return View(new NorthwindDataContext().Products);
        }
    }
}
