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
using System.IO;
using Hec.FileStorage;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class HelpFileController : BaseController
    {        
        public HelpFileController(HecContext db) : base(db)
        {
        }

        private async Task<HelpFile> TryFind(Guid id)
        {
            var entity = await db.HelpFiles.FindAsync(id);
            if (entity == null)
                throw new IdNotFoundException<HelpFile>(id);

            return entity;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = db.HelpFiles.ToDataSourceResult(request);
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Create([DataSourceRequest]DataSourceRequest request, HelpFile model)
        {
            if (ModelState.IsValid)
            {
                var max = db.HelpFiles.Select(x => x.Sequence).DefaultIfEmpty(0).Max();

                var entity = new HelpFile
                {
                    Name = model.Name,
                    Sequence = max + 1
                };
                
                db.HelpFiles.Add(entity);
                await db.SaveChangesAsync();

                model.Id = entity.Id;
                model.Sequence = entity.Sequence;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Update([DataSourceRequest]DataSourceRequest request, HelpFile model)
        {
            if (ModelState.IsValid)
            {
                var entity = await TryFind(model.Id);
                entity.Name = model.Name;

                db.SetModified(entity);
                await db.SaveChangesAsync();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Destroy([DataSourceRequest]DataSourceRequest request, HelpFile model)
        {
            if (ModelState.IsValid)
            {
                var entity = await TryFind(model.Id);

                // Delete physical file
                if (!String.IsNullOrEmpty(model.FileId))
                {
                    var fi = new FileInfo(Server.MapPath("~/Help/" + model.FileId));
                    if (fi != null)
                        fi.Delete();
                }

                // Delete DB record
                db.SetDeleted(entity);
                await db.SaveChangesAsync();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            // Read stream
            byte[] bytes = FileHelper.ReadBytes(file.InputStream);

            if (bytes != null && bytes.Length > 0)
            {
                var s = new FileStream(Server.MapPath("~/Help/" + fileName), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                s.Write(bytes, 0, bytes.Length);
                s.Close();
            }

            return Content("");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UploadDone(Guid id, string fileId)
        {
            var helpFile = db.HelpFiles.Find(id);
            if (helpFile != null)
            {
                helpFile.FileId = fileId;
                db.SetModified(helpFile);
                await db.SaveChangesAsync();
            }

            return Content("");
        }
    }
}
