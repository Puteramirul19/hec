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
    public class AppliancesModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DefaultNumbersOfApp { get; set; }
        public decimal DefaultHoursPerDay { get; set; }
        public decimal DefaultDaysPerMonth { get; set; }
        public decimal DefaultWatts { get; set; }
        public bool ForLivingRoom { get; set; }
        public bool ForMasterBedroom { get; set; }
        public bool ForBedroom { get; set; }
        public bool ForBathroom { get; set; }
        public bool ForDiningRoom { get; set; }
        public bool ForKitchen { get; set; }
        public Guid TipCategoryId { get; set; }
        public string TipCategoryName { get; set; }
        public bool IsActive { get; set; }

        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }

        public List<TipCategory> TipsCategory { get; set; }


        public AppliancesModel()
        {
        }

        public AppliancesModel(Appliance entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.DefaultNumbersOfApp = entity.DefaultNumbersOfApp;
            this.DefaultHoursPerDay = entity.DefaultHoursPerDay;
            this.DefaultDaysPerMonth = entity.DefaultDaysPerMonth;
            this.DefaultWatts = entity.DefaultWatts;
            this.ForLivingRoom = entity.ForLivingRoom;
            this.ForMasterBedroom = entity.ForMasterBedroom;
            this.ForBedroom = entity.ForBedroom;
            this.ForBathroom = entity.ForBathroom;
            this.ForDiningRoom = entity.ForDiningRoom;
            this.ForKitchen = entity.ForKitchen;
            this.TipCategoryId = entity.TipCategoryId;
            this.TipCategoryName = entity.TipCategory.Name;
            this.IsActive = entity.IsActive;

            this.FileId = entity.FileId;
            this.FileName = entity.FileName;
            this.FileExtension = entity.FileExtension;
            this.FileSize = entity.FileSize;
        }
    }

    [Authorize]
    public class AppliancesController : BaseController
    {
        private IFileStore fileStore;

        public AppliancesController(HecContext db, IFileStore fileStore) : base(db)
        {
            this.fileStore = fileStore;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Appliances";
            ViewBag.TipsCategories = db.TipCategories.ToList();

            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Appliance> appliances = db.Appliances;
            DataSourceResult result = appliances
                .Include(p => p.TipCategory)
                .ToDataSourceResult(request, appliance => new
                {
                    Id = appliance.Id,
                    Name = appliance.Name,
                    DefaultHoursPerDay = appliance.DefaultHoursPerDay,
                    DefaultDaysPerMonth = appliance.DefaultDaysPerMonth,
                    DefaultWatts = appliance.DefaultWatts,
                    ForLivingRoom = appliance.ForLivingRoom,
                    ForMasterBedroom = appliance.ForMasterBedroom,
                    ForBedroom = appliance.ForBedroom,
                    ForBathroom = appliance.ForBathroom,
                    ForDiningRoom = appliance.ForDiningRoom,
                    ForKitchen = appliance.ForKitchen,
                    TipCategoryId = appliance.TipCategoryId,
                    TipCategoryName = appliance.TipCategory.Name,
                    IsActive = appliance.IsActive,
                    FileId = appliance.FileId,
                    FileName = appliance.FileName,
                    FileExtension = appliance.FileExtension,
                    FileSize = appliance.FileSize
                });

            return Json(result);
        }

        public ActionResult Create()
        {
            return View(new AppliancesModel());
        }


        private static readonly Object createLock = new Object();
        private static readonly Object updateLock = new Object();

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "CreatedOn, UpdatedOn")] Appliance appliance)
        {
            if (ModelState.IsValid)
            {
                lock (createLock)
                {
                    if (db.Appliances.Any(x => x.Name == appliance.Name))
                        ModelState.AddModelError("Name", "Appliance with name '" + appliance.Name + "' already existed.");
                    else
                    {

                        var entity = new Appliance
                        {
                            Name = appliance.Name,
                            DefaultHoursPerDay = appliance.DefaultHoursPerDay,
                            DefaultDaysPerMonth = appliance.DefaultDaysPerMonth,
                            DefaultWatts = appliance.DefaultWatts,
                            ForLivingRoom = appliance.ForLivingRoom,
                            ForMasterBedroom = appliance.ForMasterBedroom,
                            ForBedroom = appliance.ForBedroom,
                            ForBathroom = appliance.ForBathroom,
                            ForDiningRoom = appliance.ForDiningRoom,
                            ForKitchen = appliance.ForKitchen,
                            TipCategoryId = appliance.TipCategoryId,
                            IsActive = appliance.IsActive,
                            FileId = appliance.FileId,
                            FileName = appliance.FileName,
                            FileExtension = appliance.FileExtension,
                            FileSize = appliance.FileSize

                        };

                        if (!String.IsNullOrEmpty(appliance.FileId))
                        {
                            var stream = fileStore.GetStream(appliance.FileId);
                            fileStore.Delete(appliance.FileId);

                            var bytes = FileHelper.ReadBytes(stream);

                            var fileStream = new FileStream(BuildFileName(appliance.Name), FileMode.Create);
                            fileStream.Write(bytes, 0, bytes.Length);
                            fileStream.Flush();
                            fileStream.Close();

                            fileStore.Delete(appliance.FileId);
                        }

                        db.Appliances.Add(entity);
                        db.SaveChanges();

                        appliance.Id = entity.Id;
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

            Appliance entity = await db.Appliances.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            return View(new AppliancesModel(entity));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Guid id, Appliance model)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Appliances.FindAsync(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }
            
                entity.Name = model.Name;
                entity.DefaultHoursPerDay = model.DefaultHoursPerDay;
                entity.DefaultDaysPerMonth = model.DefaultDaysPerMonth;
                entity.DefaultWatts = model.DefaultWatts;
                entity.ForLivingRoom = model.ForLivingRoom;
                entity.ForMasterBedroom = model.ForMasterBedroom;
                entity.ForBedroom = model.ForBedroom;
                entity.ForBathroom = model.ForBathroom;
                entity.ForDiningRoom = model.ForDiningRoom;
                entity.ForKitchen = model.ForKitchen;
                entity.TipCategoryId = model.TipCategoryId;
                entity.IsActive = model.IsActive;
                entity.FileId = model.FileId;
                entity.FileName = model.FileName;
                entity.FileExtension = model.FileExtension;
                entity.FileSize = model.FileSize;

                db.SetModified(entity);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private string BuildFileName(string name)
        {
            return Server.MapPath("~/Uploads/Appliances/" + name + ".png");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Appliances.FindAsync(id);
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
