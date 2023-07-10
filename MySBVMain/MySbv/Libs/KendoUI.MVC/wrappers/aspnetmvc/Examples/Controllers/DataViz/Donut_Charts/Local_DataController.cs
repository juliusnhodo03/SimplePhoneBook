﻿using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Examples.Models;

namespace Kendo.Mvc.Examples.Controllers
{
    public partial class Donut_ChartsController : Controller
    {
        public ActionResult Local_Data()
        {
            return View(ChartDataRepository.SpainElectricityBreakdown());
        }
    }
}