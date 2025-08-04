using System;
using System.Data;
using System.Linq;
using Hec.Entities;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class RoleGridItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool NeedZone { get; set; }
        public bool NeedSubZone { get; set; }
        public bool NeedUnit { get; set; }
    }

    [Authorize]
    public class RoleGridItemController : BaseController
    {
        public RoleGridItemController(HecContext db)
            : base(db)
        {
        }
        
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(db.Roles
                .Select(x => new RoleGridItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    NeedZone = x.NeedZone,
                    NeedSubZone = x.NeedSubZone,
                    NeedUnit = x.NeedUnit
                })
                .ToDataSourceResult(request)
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
