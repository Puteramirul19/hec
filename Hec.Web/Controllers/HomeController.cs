using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlTypes;

namespace Hec.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(HecContext db) : base(db)
        {
        }
        
        public ActionResult Index()
        {
            return View();

            //var lang = Entities.User.DEFAULT_LANGUAGE;
            //var user = db.Users.Where(x => x.Id == User.Identity.Name).Select(x => new { Language = x.Language, Role = x.Role }).FirstOrDefault();
            //if (user != null && !string.IsNullOrEmpty(user.Language))
            //{
            //    lang = user.Language;
            //    if (lang != "ms" && lang != "en")
            //        lang = Entities.User.DEFAULT_LANGUAGE;
            //}
            //ViewBag.Language = lang;


            //DateTime date = DateTime.UtcNow;
            //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //if (dateFrom == null)
            //    ViewBag.DateFrom = firstDayOfMonth;
            //else
            //    ViewBag.DateFrom = dateFrom;

            //if (dateTo == null)
            //    ViewBag.DateTo = lastDayOfMonth;
            //else
            //    ViewBag.DateTo = lastDayOfMonth;

            ////return View();
            //if (user.Role.Name.ToString() == "Public")
            //{
            //    return RedirectToAction("Index", "Public/BillInfo");
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Admin/Home");
            //}
        }

        public ActionResult Error(int? id)
        {
            var errmsg = "This application encountered a system error. Contact System Administrator for help.";

            if (!id.HasValue)
                ViewBag.Message = errmsg;

            switch (id)
            {
                case 404:
                    ViewBag.Message = "Page not found.";
                    break;

                default:
                    ViewBag.Message = errmsg;
                    break;
            }

            ViewBag.Exception = Server.GetLastError();

            return View();
        }
        
        public ActionResult Help()
        {
            return View(db.HelpFiles.OrderBy(x => x.Sequence));
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var user = GetCurrentUser();
            if (user == null)
                user = new Entities.User();
            return PartialView("_MainMenu", user);
        }

        public ActionResult Sitemap()
        {
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }

        public ActionResult TestPage()
        {
            return View("~/Views/Home/Index2.cshtml");
        }
    }
}