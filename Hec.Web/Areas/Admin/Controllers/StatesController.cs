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
    public class StatesController : BaseController
    {
        public StatesController(HecContext db)
            : base(db)
        {
        }

        public ActionResult Index()
        {
            ViewBag.Title = "States";

            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = db.States.ToDataSourceResult(request);
            return Json(result);
        }

        private async Task<State> TryFind(Guid id)
        {
            var entity = await db.States.FindAsync(id);
            if (entity == null)
                throw new IdNotFoundException<State>(id);
            return entity;
        }

        private static readonly Object createLock = new Object();
        private static readonly Object updateLock = new Object();


        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Update([DataSourceRequest]DataSourceRequest request, Guid id, State model)
        {
            var entity = await TryFind(model.Id);

            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    return Status(HttpStatusCode.BadRequest);
                }

                var existingName = await db.States.AnyAsync(x => x.Name == model.Name && x.Id != model.Id);

                lock (updateLock)
                    {
                    if (existingName)
                            ModelState.AddModelError("Name", "State name '" + model.Name + "' already existed.");
                        else
                        {
                            entity.Code = model.Code;
                            entity.Name = model.Name;
                            entity.Weekend1 = model.Weekend1;
                            entity.Weekend2 = model.Weekend2;
                            entity.EhrmsCode = model.EhrmsCode;
                            entity.IntegrationId = model.IntegrationId;
                        }
                    }

                db.SetModified(entity);
                await db.SaveChangesAsync();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "Id")] State model)
        {
            if (ModelState.IsValid)
            {
                lock (createLock)
                {
                    if (db.States.Any(x => x.Name == model.Name))
                        ModelState.AddModelError("Name", "State with name '" + model.Name + "' already existed.");
                    else
                    {
                        var entity = new State
                        {
                            Code = model.Code,
                            Name = model.Name,
                            Weekend1 = model.Weekend1,
                            Weekend2 = model.Weekend2,
                            EhrmsCode = model.EhrmsCode,
                            IntegrationId = model.IntegrationId
                        };

                        db.States.Add(entity);
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
