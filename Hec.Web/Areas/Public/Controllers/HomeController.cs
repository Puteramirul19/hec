using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Public.Controllers
{
    public class HomeController : Web.Controllers.BaseController
    {
        public HomeController(HecContext db) : base(db)
        {
        }

        // GET: Public/Home
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "BillInfo");
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            //return View();
            //return RedirectToAction("Index", "BillInfo");
        }

        [HttpGet]
        public ActionResult ChangePhoto()
        {
            return View();
        }

        public ActionResult ProfilePhoto()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("");

            var user = GetCurrentUser();
            if (String.IsNullOrEmpty(user.Photo))
                return Content("");

            return Content($"<img src='{user.Photo}' />");
        }

        public ActionResult ChangePhoto(HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.ErrorMessage = "Please select a file.";
                return View();
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg")
            {
                ViewBag.ErrorMessage = "Only JPG file is allowed.";
                return View();
            }

            if (file.ContentLength > 500000)
            {
                ViewBag.ErrorMessage = "File must be less than 500kB in size.";
                return View();
            }
            
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.Name);
                if (user != null)
                {
                    var bytes = FileStorage.FileHelper.ReadBytes(file.InputStream);
                    user.Photo = "data:image/png;base64," + Convert.ToBase64String(bytes);
                    db.SetModified(user);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}