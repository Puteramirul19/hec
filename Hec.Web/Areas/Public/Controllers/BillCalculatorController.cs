using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Public.Controllers
{
    [Authorize]
    public class BillCalculatorController : Controller
    {
        // GET: BillCalculator
        public ActionResult Index()
        {
            return View();
        }
    }
}