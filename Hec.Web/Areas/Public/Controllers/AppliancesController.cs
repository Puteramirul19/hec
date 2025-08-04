﻿using System;
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

namespace Hec.Web.Areas.Public.Controllers
{
    //[Authorize]
    public class AppliancesController : BaseController
    {
        public AppliancesController(HecContext db) : base(db)
        {       
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Appliance> appliances = db.Appliances;
            DataSourceResult result = appliances.ToDataSourceResult(request);

            return Json(result);
        }

        //private static readonly Object createLock = new Object();
        //private static readonly Object updateLock = new Object();
        
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create([DataSourceRequest]DataSourceRequest request, Appliance appliance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        lock (createLock)
        //        {
        //            if (db.Appliances.Any(x => x.Name == appliance.Name))
        //                ModelState.AddModelError("Name", "Appliance with name '" + appliance.Name + "' already existed.");
        //            else
        //            {
        //                var entity = new Appliance
        //                {
        //                    Name = appliance.Name,
        //                    DefaultHoursPerDay = appliance.DefaultHoursPerDay,
        //                    DefaultDaysPerMonth = appliance.DefaultDaysPerMonth,
        //                    DefaultWatts = appliance.DefaultWatts,
        //                    ForLivingRoom = appliance.ForLivingRoom,
        //                    ForMasterBedroom = appliance.ForMasterBedroom,
        //                    ForBedroom = appliance.ForBedroom,
        //                    ForBathroom = appliance.ForBathroom,
        //                    ForDiningRoom = appliance.ForDiningRoom,
        //                    ForKitchen = appliance.ForKitchen
        //                };

        //                db.Appliances.Add(entity);
        //                db.SaveChanges();
        //            }
        //        }
        //    }

        //    return Json(new[] { appliance }.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Update([DataSourceRequest]DataSourceRequest request, Appliance appliance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        lock (updateLock)
        //        {
        //            if (db.Appliances.Any(x => x.Name == appliance.Name && x.Id != appliance.Id))
        //                ModelState.AddModelError("Name", "Appliance with name '" + appliance.Name + "' already existed.");
        //            else
        //            {
        //                var entity = new Appliance
        //                {
        //                    Id = appliance.Id,
        //                    Name = appliance.Name,
        //                    DefaultHoursPerDay = appliance.DefaultHoursPerDay,
        //                    DefaultDaysPerMonth = appliance.DefaultDaysPerMonth,
        //                    DefaultWatts = appliance.DefaultWatts,
        //                    ForLivingRoom = appliance.ForLivingRoom,
        //                    ForMasterBedroom = appliance.ForMasterBedroom,
        //                    ForBedroom = appliance.ForBedroom,
        //                    ForBathroom = appliance.ForBathroom,
        //                    ForDiningRoom = appliance.ForDiningRoom,
        //                    ForKitchen = appliance.ForKitchen
        //                };

        //                db.Appliances.Attach(entity);
        //                db.Entry(entity).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //        }
        //    }

        //    return Json(new[] { appliance }.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Delete([DataSourceRequest]DataSourceRequest request, Appliance appliance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var found = db.Appliances.Find(appliance.Id);
        //        if (found != null)
        //        {
        //            db.Appliances.Remove(found);
        //            db.SaveChanges();
        //        }
        //    }

        //    return Json(new[] { appliance }.ToDataSourceResult(request, ModelState));
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
