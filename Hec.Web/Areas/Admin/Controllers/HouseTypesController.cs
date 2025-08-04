using Hec.Entities;
using Hec.FileStorage;
using Hec.Web.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class HouseTypesListModel
    {
        public string Title { get; set; }
        public List<HouseCategory> HouseCategoryList { get; set; }

        public Guid Id { get; set; }
        public string HouseTypeCode { get; set; }
        public string HouseTypeName { get; set; }
        public string PremiseCode { get; set; }
        public string PremiseType { get; set; }
        public string Average { get; set; }
        public Guid HouseCategoryId { get; set; }

        public bool IsActive { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }

        public string FileHeaderId { get; set; }
        public string FileHeaderName { get; set; }
        public string FileHeaderExtension { get; set; }
        public int? FileHeaderSize { get; set; }

        public HouseTypesListModel()
        {
        }

        public HouseTypesListModel(HouseType entity)
        {
            this.Id = entity.Id;
            this.HouseTypeCode = entity.HouseTypeCode;
            this.HouseTypeName = entity.HouseTypeName;
            this.PremiseCode = entity.PremiseCode;
            this.PremiseType = entity.PremiseType;
            this.Average = entity.Average;
            this.IsActive = entity.IsActive;
            this.HouseCategoryId = entity.HouseCategoryId;

            this.FileId = entity.FileId;
            this.FileName = entity.FileName;
            this.FileExtension = entity.FileExtension;
            this.FileSize = entity.FileSize;

            this.FileHeaderId = entity.FileHeaderId;
            this.FileHeaderName = entity.FileHeaderName;
            this.FileHeaderExtension = entity.FileHeaderExtension;
            this.FileHeaderSize = entity.FileHeaderSize;
        }
    }

    [Authorize]
    public class HouseTypesController : BaseController
    {
        private IFileStore fileStore;

        public HouseTypesController(HecContext db, IFileStore fileStore) : base(db)
        {
            this.fileStore = fileStore;
        }

        // GET: Admin/HouseType
        public ActionResult Index()
        {

            var model = new HouseTypesListModel
            {
                Title = "House-Category",
                HouseCategoryList = db.HouseCategories.ToList()
            };

            return View(model);
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<HouseType> houses = db.HouseTypes;

            DataSourceResult result = houses
               .Include(p => p.HouseCategories)
               .ToDataSourceResult(request, house => new {
                   Id = house.Id,
                   HouseTypeCode = house.HouseTypeCode,
                   HouseTypeName = house.HouseTypeName,
                   PremiseCode = house.PremiseCode,
                   PremiseType = house.PremiseType,
                   Average = house.Average,
                   IsActive = house.IsActive,
                   HouseCategoryId = house.HouseCategoryId,

                   FileId = house.FileId,
                   FileName = house.FileName,
                   FileExtension = house.FileExtension,
                   FileSize = house.FileSize,

                   FileHeaderId = house.FileHeaderId,
                   FileHeaderName = house.FileHeaderName,
                   FileHeaderExtension = house.FileHeaderExtension,
                   FileHeaderSize = house.FileHeaderSize

               });
            //DataSourceResult result = house.ToDataSourceResult(request);

            return Json(result);
        }

        public ActionResult Create()
        {
            return View(new HouseTypesListModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "Id, CreatedOn, UpdatedOn")] HouseType houseType)
        {
            if (ModelState.IsValid)
            {
                var entity = new HouseType
                {
                    Id = houseType.Id,
                    HouseTypeCode = houseType.HouseTypeCode,
                    HouseTypeName = houseType.HouseTypeName,
                    PremiseCode = houseType.PremiseCode,
                    PremiseType = houseType.PremiseType,
                    Average = houseType.Average,
                    IsActive = houseType.IsActive,
                    HouseCategoryId = houseType.HouseCategoryId,

                    FileId = houseType.FileId,
                    FileName = houseType.FileName,
                    FileExtension = houseType.FileExtension,
                    FileSize = houseType.FileSize,

                    FileHeaderId = houseType.FileHeaderId,
                    FileHeaderName = houseType.FileHeaderName,
                    FileHeaderExtension = houseType.FileHeaderExtension,
                    FileHeaderSize = houseType.FileHeaderSize
                };

                db.HouseTypes.Add(entity);
                db.SaveChangesAsync();

                houseType.Id = entity.Id;
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

            HouseType entity = await db.HouseTypes.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            return View(new HouseTypesListModel(entity));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Edit(Guid id, HouseTypesListModel model)
        {
            if (ModelState.IsValid)
            {

                var entity = await db.HouseTypes.FindAsync(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }

                entity.Id = model.Id;
                entity.HouseTypeCode = model.HouseTypeCode;
                entity.HouseTypeName = model.HouseTypeName;
                entity.PremiseCode = model.PremiseCode;
                entity.PremiseType = model.PremiseType;
                entity.Average = model.Average;
                entity.IsActive = model.IsActive;
                entity.HouseCategoryId = model.HouseCategoryId;

                entity.FileId = model.FileId;
                entity.FileName = model.FileName;
                entity.FileExtension = model.FileExtension;
                entity.FileSize = model.FileSize;

                entity.FileHeaderId = model.FileHeaderId;
                entity.FileHeaderName = model.FileHeaderName;
                entity.FileHeaderExtension = model.FileHeaderExtension;
                entity.FileHeaderSize = model.FileHeaderSize;

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
                var entity = await db.HouseTypes.FindAsync(id);
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
