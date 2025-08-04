using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Hec.Entities;
using System.Threading.Tasks;
using Hec.Jobs;
using Hangfire;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class EmailQueueController : BaseController
    {
        private IBackgroundJobClient backgroundJob;

        public EmailQueueController(HecContext db, IBackgroundJobClient backgroundJob) : base(db)
        {
            this.backgroundJob = backgroundJob;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Email Queue";
            return View();
        }

        public ActionResult SendNow()
        {
            backgroundJob.Enqueue<EmailDispatchJob>(x => x.Run());
            return RedirectToAction("Index");
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = db.EmailQueues.ToDataSourceResult(request);
            return Json(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
