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
using Hec.Web.Controllers;
using System.Threading.Tasks;
using Hec.FileStorage;
using System.IO;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class TipModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        [AllowHtml] public string Description { get; set; }
        public bool IsActive { get; set; }
        public Guid TipCategoryId { get; set; }
        public string TipCategoryName { get; set; }

        public List<TipCategory> TipsCategory { get; set; }

        public string FileThumbId { get; set; }
        public string FileThumbName { get; set; }
        public string FileThumbExtension { get; set; }
        public int? FileThumbSize { get; set; }

        public string FilePopupId { get; set; }
        public string FilePopupName { get; set; }
        public string FilePopupExtension { get; set; }
        public int? FilePopupSize { get; set; }



        public TipModel()
        {
        }

        public TipModel(Tip entity)
        {
            this.Id = entity.Id;
            this.Title = entity.Title;
            this.Description = entity.Description;
            this.IsActive = entity.IsActive;
            this.TipCategoryId = entity.TipCategoryId;
            this.TipCategoryName = entity.TipCategory.Name;

            this.FileThumbId = entity.FileThumbId;
            this.FileThumbName = entity.FileThumbName;
            this.FileThumbExtension = entity.FileThumbExtension;
            this.FileThumbSize = entity.FileThumbSize;

            this.FilePopupId = entity.FilePopupId;
            this.FilePopupName = entity.FilePopupName;
            this.FilePopupExtension = entity.FilePopupExtension;
            this.FilePopupSize = entity.FilePopupSize;

        }
    }

    [Authorize]
    public class TipsController : BaseController
    {
        private IFileStore fileStore;

        public TipsController(HecContext db, IFileStore fileStore) : base(db)
        {
            this.fileStore = fileStore;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Tips";
            ViewBag.TipsCategories = db.TipCategories.ToList();
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Tip> announcements = db.Tips;
            DataSourceResult result = announcements
                .Include(p => p.TipCategory)
                .ToDataSourceResult(request, entity => new {
                Id = entity.Id,
                Title = entity.Title,
                IsActive = entity.IsActive,
                Description = entity.Description,
                TipCategoryId = entity.TipCategoryId,
                TipCategoryName = entity.TipCategory.Name,

                FileThumbnId = entity.FileThumbId,
                FileThumbName = entity.FileThumbName,
                FileThumbExtension = entity.FileThumbExtension,
                FileThumbSize = entity.FileThumbSize,

                FilePopupId = entity.FilePopupId,
                FilePopupName = entity.FilePopupName,
                FilePopupExtension = entity.FilePopupExtension,
                FilePopupSize = entity.FilePopupSize

                });

            return Json(result);
        }

        public ActionResult Create()
        {
            return View(new TipModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "Id,CreatedOn, UpdatedOn")] TipModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Tip
                {
                    Title = model.Title,
                    IsActive = model.IsActive,
                    Description = model.Description,
                    TipCategoryId = model.TipCategoryId,
                    FileThumbId = model.FileThumbId,
                    FileThumbName = model.FileThumbName,
                    FileThumbExtension = model.FileThumbExtension,
                    FileThumbSize = model.FileThumbSize,
                    FilePopupId = model.FilePopupId,
                    FilePopupName = model.FilePopupName,
                    FilePopupExtension = model.FilePopupExtension,
                    FilePopupSize = model.FilePopupSize
          
                };


                //if (!String.IsNullOrEmpty(model.FileThumbId))
                //{
                //    var stream = fileStore.GetStream(model.FileThumbId);
                //    fileStore.Delete(model.FileThumbId);

                //    var bytes = FileHelper.ReadBytes(stream);


                //    var name = Path.GetFileNameWithoutExtension(model.FileThumbName);
                //    var fileStream = new FileStream(BuildFileName(model.FileThumbName), FileMode.Create);
                //    fileStream.Write(bytes, 0, bytes.Length);
                //    fileStream.Flush();
                //    fileStream.Close();

                //    fileStore.Delete(model.FileThumbId);
                //}

                //if (!String.IsNullOrEmpty(model.FilePopupId))
                //{
                //    var stream = fileStore.GetStream(model.FilePopupId);
                //    fileStore.Delete(model.FilePopupId);

                //    var bytes = FileHelper.ReadBytes(stream);

                //    var fileStream = new FileStream(BuildFileName(model.FilePopupName), FileMode.Create);
                //    fileStream.Write(bytes, 0, bytes.Length);
                //    fileStream.Flush();
                //    fileStream.Close();

                //    fileStore.Delete(model.FilePopupId);
                //}

                db.Tips.Add(entity);
                db.SaveChangesAsync();

                model.Id = entity.Id;
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tip entity = await db.Tips.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            return View(new TipModel(entity));
        }

      

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Guid id, TipModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Tips.FindAsync(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }


                //if (!String.IsNullOrEmpty(model.FileId))
                //{
                //    var stream = fileStore.GetStream(model.FileId);
                //    fileStore.Delete(model.FileId);

                //    var bytes = FileHelper.ReadBytes(stream);

                //    var fileStream = new FileStream(BuildFileName(model.FileName), FileMode.Create);
                //    fileStream.Write(bytes, 0, bytes.Length);
                //    fileStream.Flush();
                //    fileStream.Close();

                //    fileStore.Delete(model.FileId);
                //}

                //if (!String.IsNullOrEmpty(model.FileThumbId))
                //{
                //    var stream = fileStore.GetStream(model.FileThumbId);
                //    fileStore.Delete(model.FileThumbId);

                //    var bytes = FileHelper.ReadBytes(stream);

                //    var fileStream = new FileStream(BuildFileName(model.FileThumbName), FileMode.Create);
                //    fileStream.Write(bytes, 0, bytes.Length);
                //    fileStream.Flush();
                //    fileStream.Close();

                //    fileStore.Delete(model.FileThumbId);
                //}


                entity.IsActive = model.IsActive;
                entity.Title = model.Title;
                entity.Description = model.Description;
                entity.TipCategoryId = model.TipCategoryId;
                entity.FileThumbId = model.FileThumbId;
                entity.FileThumbName = model.FileThumbName;
                entity.FileThumbExtension = model.FileThumbExtension;
                entity.FileThumbSize = model.FileThumbSize;
                entity.FilePopupId = model.FilePopupId;
                entity.FilePopupName = model.FilePopupName;
                entity.FilePopupExtension = model.FilePopupExtension;
                entity.FilePopupSize = model.FilePopupSize;

                db.SetModified(entity);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Tips.FindAsync(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }

                db.SetDeleted(entity);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
