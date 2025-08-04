using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Controllers
{
    [Authorize]
    public class ReportingsController : BaseController
    {
        public ReportingsController(HecContext db) : base(db)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ReportName</param>
        /// <returns></returns>
        public ActionResult Index(ReportName id)
        {
            var model = new ReportingModel
            {
                ReportName = id
            };

            switch (id)
            {
                case ReportName.NoticeByZone:
                    model.Title = "Overall Zone Report";
                    break;

                case ReportName.NoticeBySubZone:
                    model.Title = "Overall Sub-Zone Report";
                    break;

                case ReportName.NoticeList:
                    break;
            }

            return View(model);
        }
    }

    public enum ReportName
    {
        NoticeByZone,
        NoticeBySubZone,
        NoticeList
    }

    public class ReportingModel
    {
        public string Title { get; set; }
        public ReportName ReportName { get; set; }
        public string ZoneId { get; set; }
        public string SubZoneId { get; set; }
        public string CacheBust { get; set; }

        public ReportingModel()
        {
            CacheBust = new Guid().ToString();
            ZoneId = "0";
            SubZoneId = "0";
        }
    }
}