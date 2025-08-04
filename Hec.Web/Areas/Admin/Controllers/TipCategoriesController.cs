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
    public class TipCategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }

        public TipCategoryModel()
        {
        }

        public TipCategoryModel(TipCategory entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.IsActive = entity.IsActive;
            this.FileId = entity.FileId;
            this.FileName = entity.FileName;
            this.FileExtension = entity.FileExtension;
            this.FileSize = entity.FileSize;
        }
    }

    [Authorize]
    public class TipCategoryController : BaseController
    {
        private IFileStore fileStore;

        public TipCategoryController(HecContext db, IFileStore fileStore) : base(db)
        {
            this.fileStore = fileStore;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Tips Categories";
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<TipCategory> announcements = db.TipCategories;
            DataSourceResult result = announcements.ToDataSourceResult(request, entity => new {
                Id = entity.Id,
                Name = entity.Name,
                IsActive = entity.IsActive,
                FileId = entity.FileId,
                FileName = entity.FileName,
                FileExtension = entity.FileExtension,
                FileSize = entity.FileSize
            });

            return Json(result);
        }

        public ActionResult Create()
        {
            return View(new TipCategoryModel());
        }

        private static readonly Object createLock = new Object();
        private static readonly Object updateLock = new Object();

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "CreatedOn, UpdatedOn")] TipCategory tipCategory)
        {
            if (ModelState.IsValid)
            {
                lock (createLock)
                {
                    if (db.TipCategories.Any(x => x.Name == tipCategory.Name))
                        ModelState.AddModelError("Name", "TipCategory with name '" + tipCategory.Name + "' already existed.");
                    else
                    {
                        var entity = new TipCategory
                        {
                            Id = tipCategory.Id,
                            Name = tipCategory.Name,
                            IsActive = tipCategory.IsActive
                        };

                        if (!String.IsNullOrEmpty(tipCategory.FileId))
                        {
                            var stream = fileStore.GetStream(tipCategory.FileId);
                            fileStore.Delete(tipCategory.FileId);

                            var bytes = FileHelper.ReadBytes(stream);

                            var fileStream = new FileStream(BuildFileName(tipCategory.Name), FileMode.Create);
                            fileStream.Write(bytes, 0, bytes.Length);
                            fileStream.Flush();
                            fileStream.Close();

                            fileStore.Delete(tipCategory.FileId);
                        }

                        db.TipCategories.Add(entity);
                        db.SaveChanges();

                        tipCategory.Id = entity.Id;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipCategory entity = await db.TipCategories.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            return View(new TipCategoryModel(entity));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Guid id, TipCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.TipCategories.FindAsync(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }
                
                if (!String.IsNullOrEmpty(model.FileId))
                {
                    var stream = fileStore.GetStream(model.FileId);
                    var bytes = FileHelper.ReadBytes(stream);

                    var fileStream = new FileStream(BuildFileName(entity.Name), FileMode.Create);
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    fileStore.Delete(model.FileId);
                }

                if (entity.Name != model.Name)
                {
                    var fi = new FileInfo(BuildFileName(entity.Name));
                    if (fi.Exists)
                    {
                        fi.MoveTo(BuildFileName(model.Name));
                    }
                }

                entity.Name = model.Name;
                entity.IsActive = model.IsActive;

                db.SetModified(entity);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private string BuildFileName(string name)
        {
            return Server.MapPath("~/Uploads/TipCategory/" + name + ".png");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.TipCategories.FindAsync(id);
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

        //public ActionResult GetImage(Guid id)
        //{

        //    var stream = fileStore.GetStream(id);
        //    var fileName = System.IO.Path.GetFileName(fileId);

        //    return File(stream, System.Web.MimeMapping.GetMimeMapping(fileName), fileName);


        //    TipCategory obj = mms.GetMember(id);
        //    byte[] imageData = obj.FileSize;
        //    return File(imageData, "image/jpg");


        //    var imgquery = (from img in db.TipCategories
        //                    where img.Id == id

        //                    select new
        //                    {
        //                        Image = img.FileId
        //                    }).FirstOrDefault();
        //    {
        //        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
        //        byte[] Image = imgquery.Image;
        //        return File(Image, "Image/jpg");
        //    }
        //}


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
