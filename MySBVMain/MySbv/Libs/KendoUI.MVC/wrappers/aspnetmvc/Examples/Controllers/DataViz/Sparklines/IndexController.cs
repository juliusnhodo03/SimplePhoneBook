﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Kendo.Mvc.Examples.Controllers
{
    public partial class SparklinesController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.PressureData = new double[] {
                936, 968, 1025, 999, 998, 1014, 1017, 1010, 1010, 1007,
                1004, 988, 990, 988, 987, 995, 946, 954, 991, 984,
                974, 956, 986, 936, 955, 1021, 1013, 1005, 958, 953,
                952, 940, 937, 980, 966, 965, 928, 916, 910, 980
            };

            ViewBag.TemperatureData = new double[] {
                16, 17, 18, 19, 20, 21, 21, 22, 23, 22,
                20, 18, 17, 17, 16, 16, 17, 18, 19, 20,
                21, 22, 23, 25, 24, 24, 22, 22, 23, 22,
                22, 21, 16, 15, 15, 16, 19, 20, 20, 21
            };

            ViewBag.HumidityData = new double[] {
                71, 70, 69, 68, 65, 60, 55, 55, 50, 52,
                73, 72, 72, 71, 68, 63, 57, 58, 53, 55,
                63, 59, 61, 64, 58, 53, 48, 48, 45, 45,
                63, 64, 63, 67, 58, 56, 53, 59, 51, 54
            };

            ViewBag.TemperatureRange = new double[] { 21, 23 };

            ViewBag.ACStats = new Dictionary<string, int[]>
            {
                {"mon", new int[] {14, 10}},
                {"tue", new int[] {8, 16}},
                {"wed", new int[] {8, 16}},
                {"thu", new int[] {12, 12}},
                {"fri", new int[] {6, 18}},
                {"sat", new int[] {1, 23}},
                {"sun", new int[] {5, 19}}
            };

            return View();
        }
    }
}