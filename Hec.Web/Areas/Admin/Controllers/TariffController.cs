using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hec.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Linq;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class TariffController : BaseController
    {
        public TariffController(HecContext db)
            : base(db)
        {
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Tariff";

            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = db.Tariffs.ToDataSourceResult(request);
            return Json(result);
        }

        private async Task<Tariff> TryFind(Guid id)
        {
            var entity = await db.Tariffs.FindAsync(id);
            if (entity == null)
                throw new IdNotFoundException<Tariff>(id);
            return entity;
        }

        private static readonly Object createLock = new Object();
        private static readonly Object updateLock = new Object();


        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Update([DataSourceRequest]DataSourceRequest request, Guid id, [Bind(Exclude = "CreatedOn, UpdatedOn")] Tariff model)
        {
            var entity = await TryFind(model.Id);

            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    return Status(HttpStatusCode.BadRequest);
                }

                var existingName = await db.Tariffs.AnyAsync(x => x.Description == model.Description && x.Id != model.Id);

                lock (updateLock)
                    {
                    if (existingName)
                            ModelState.AddModelError("Description", "Description '" + model.Description + "' already existed.");
                        else
                        {
                            entity.Description = model.Description;
                            entity.TariffPerKWh = model.TariffPerKWh;
                            entity.BoundryTier = model.BoundryTier;
                            entity.CummulativeKWh = model.CummulativeKWh;
                        }
                    }

                db.SetModified(entity);
                await db.SaveChangesAsync();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "CreatedOn, UpdatedOn")] Tariff model)
        {
            if (ModelState.IsValid)
            {
                lock (createLock)
                {
                    if (db.Tariffs.Any(x => x.Description == model.Description))
                        ModelState.AddModelError("Description", "Description '" + model.Description + "' already existed.");
                    else
                    {
                        var entity = new Tariff
                        {
                            Description = model.Description,
                            TariffPerKWh = model.TariffPerKWh,
                            BoundryTier = model.BoundryTier,
                            CummulativeKWh = model.CummulativeKWh
                        };

                        db.Tariffs.Add(entity);
                        db.SaveChanges();

                        model.Id = entity.Id;
                    }
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var entity = await TryFind(id);

            try
            {
                db.SetDeleted(entity);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Cannot delete item. It might already be used in the system.", ex);
            }
            
            return Ok();
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
