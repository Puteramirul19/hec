using Hec.Entities;
using Hec.Web.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class HouseCategoriesController : BaseController
    {
        public HouseCategoriesController(HecContext db) : base(db)
        {
        }

        // GET: Admin/HouseType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<HouseCategory> house = db.HouseCategories;
            DataSourceResult result = house.ToDataSourceResult(request);

            return Json(result);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "Id, CreatedOn, UpdatedOn")] HouseCategory HouseCategory)
        {
            if (ModelState.IsValid)
            {

                HouseCategory.Id = Guid.NewGuid();
                db.HouseCategories.Add(HouseCategory);

                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return Json(new[] { HouseCategory }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "CreatedOn, UpdatedOn")] HouseCategory model)
        {
            if (ModelState.IsValid)
            {
     
                var entity = new HouseCategory
                {
                    Id = model.Id,
                    HouseCategoryName = model.HouseCategoryName,
                    HouseCategoryDesc = model.HouseCategoryDesc
       
                };

                db.HouseCategories.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                    
                
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete([DataSourceRequest]DataSourceRequest request, [Bind(Exclude = "CreatedOn, UpdatedOn")] HouseCategory HouseCategory)
        {
            if (ModelState.IsValid)
            {
                HouseCategory house = await db.HouseCategories.FindAsync(HouseCategory.Id);
                db.HouseCategories.Remove(house);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return Json(new[] { HouseCategory }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
