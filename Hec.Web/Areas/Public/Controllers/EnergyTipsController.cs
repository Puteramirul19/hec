using Hec.Entities;
using Hec.Web.Controllers;
using Kendo.Mvc.Extensions;
using System.Data;
using System.Data.Entity;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Public.Controllers
{

    public class TipModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid TipCategoryId { get; set; }
        public int DoneCount { get; set; }
        public UserTipStatus? Status { get; set; }

        public string FileThumbId { get; set; }
        public string FileThumbName { get; set; }
        public string FileThumbExtension { get; set; }
        public int? FileThumbSize { get; set; }

        public string FilePopupId { get; set; }
        public string FilePopupName { get; set; }
        public string FilePopupExtension { get; set; }
        public int? FilePopupSize { get; set; }

    }

    public class EnergyTipsController : BaseController
    {
        public EnergyTipsController(HecContext db) : base(db)
        {
        }

        // GET: EnergyTips
        public ActionResult Index()
        {
            var allCategories = db.TipCategories.Where(x=>x.IsActive == true).OrderBy(x => x.Name).ToList();
            var reorder = new List<TipCategory>(allCategories.Where(x => x.Name == "General"));
            reorder.AddRange(allCategories.Where(x => x.Name != "General"));
            ViewBag.TipCategories = reorder;

            if (User.Identity.IsAuthenticated)
            {
                var user = GetCurrentUser();
                var userTips = db.UserTips.Where(x => x.UserId == user.Id).ToList();
                ViewBag.Doit = userTips.Where(x => x.Status == UserTipStatus.Doing).Count();
                ViewBag.Done = userTips.Where(x => x.Status == UserTipStatus.Done).Count();
            }

            return View();
        }


        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = GetCurrentUser();

                var tips = from t in db.Tips
                           join ut in db.UserTips on new { TipId = t.Id, UserId = user.Id } equals new { TipId = ut.TipId, UserId = ut.UserId }
                               into utt
                           from ut in utt.DefaultIfEmpty()
                           where t.IsActive == true
                           select new TipModel
                           {
                               Id = t.Id,
                               Title = t.Title,
                               Description = t.Description,
                               TipCategoryId = t.TipCategoryId,
                               DoneCount = t.DoneCount,
                               Status = (ut == null) ? (UserTipStatus?)null : ut.Status,

                               FileThumbId = t.FileThumbId,
                               FileThumbName = t.FileThumbName,
                               FileThumbExtension = t.FileThumbExtension,
                               FileThumbSize = t.FileThumbSize,
                               FilePopupId = t.FilePopupId,
                               FilePopupName = t.FilePopupName,
                               FilePopupExtension = t.FilePopupExtension,
                               FilePopupSize = t.FilePopupSize

                           };

                DataSourceResult result = tips.ToDataSourceResult(request);
                return Json(result);
            }
            else
            {
                var tips = from t in db.Tips
                           where t.IsActive == true
                           select new TipModel
                           {
                               Id = t.Id,
                               Title = t.Title,
                               Description = t.Description,
                               TipCategoryId = t.TipCategoryId,
                               DoneCount = t.DoneCount,
                               Status = (UserTipStatus?)null,

                               FileThumbId = t.FileThumbId,
                               FileThumbName = t.FileThumbName,
                               FileThumbExtension = t.FileThumbExtension,
                               FileThumbSize = t.FileThumbSize,
                               FilePopupId = t.FilePopupId,
                               FilePopupName = t.FilePopupName,
                               FilePopupExtension = t.FilePopupExtension,
                               FilePopupSize = t.FilePopupSize

                           };

                DataSourceResult result = tips.ToDataSourceResult(request);
                return Json(result);
            }

        }

        public ActionResult UpdateUserTipStatus(Guid id, UserTipStatus status)
        {
            var user = GetCurrentUser();

            var existing = db.UserTips.FirstOrDefault(x => x.UserId == user.Id && x.TipId == id);
            if (existing == null)
            {
                db.UserTips.Add(new UserTip
                {
                    UserId = user.Id,
                    TipId = id,
                    Status = status
                });
            }
            else
            {
                existing.Status = status;
                db.SetModified(existing);
            }

            db.SaveChanges();

            // Update count
            db.Database.ExecuteSqlCommand("update Tips set DoneCount=(select count(1) from UserTips where TipId={0} and Status=1) where Id={0}", id);
            var tip = db.Tips.Select(x => new { x.Id, x.DoneCount }).FirstOrDefault(x => x.Id == id);

            var userTips = db.UserTips.Where(x => x.UserId == user.Id).ToList();
            var doitCount = userTips.Where(x => x.Status == UserTipStatus.Doing).Count();
            var doneCount = userTips.Where(x => x.Status == UserTipStatus.Done).Count();

            return Json(new { tip = tip, doitCount = doitCount, doneCount = doneCount });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}