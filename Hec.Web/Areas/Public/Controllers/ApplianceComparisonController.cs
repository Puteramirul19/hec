using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Public.Controllers
{
    public class ApplianceComparisonController : Controller
    {
        // GET: ApplianceComparison
        public ActionResult Index()
        {
            return View();
        }

        // GET: SingleAppliance
        public ActionResult SingleAppliance()
        {
            return View();
        }

        // GET: CompareAppliance
        public ActionResult CompareAppliance()
        {
            return View();
        }
    }
}