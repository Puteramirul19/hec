using Hec.Entities;
using Hec.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class SlaSettingsController : BaseController
    {
        private SettingsStore store;

        public SlaSettingsController(HecContext db, SettingsStore store)
            : base(db)
        {
            this.store = store;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Read()
        {
            return Json(await store.Get<SlaSettings>());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Update(SlaSettings model)
        {
            CheckAccess(AccessRights.SuperUser);

            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            await store.Save(model);
            return Json(model);
        }
    }
}
