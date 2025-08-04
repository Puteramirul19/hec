using Hec.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Hec.Web.Controllers
{
    public class BaseController : Controller
    {
        protected HecContext db { get; set; }

        public BaseController(HecContext db)
        {
            this.db = db;
        }

        protected new internal JsonResult Json(object data)
        {
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }

        public HttpStatusCodeResult Ok()
        {
            return Status(System.Net.HttpStatusCode.OK);
        }

        public ActionResult Error(string msg)
        {
            Response.StatusCode = 500;
            return Content(msg);
        }

        public HttpStatusCodeResult NotFound()
        {
            return Status(System.Net.HttpStatusCode.NotFound);
        }

        public HttpStatusCodeResult Status(HttpStatusCode statusCode)
        {
            return new HttpStatusCodeResult(statusCode);
        }

        protected User GetCurrentUser()
        {
            var user = db.Users
                        .FirstOrDefault(x => x.Id == User.Identity.Name);
            if (user == null)
                throw new AuthorizationException();

            return user;
        }

        protected void CheckAccess(params AccessRights[] accessRights)
        {
            CheckAccess(null, accessRights);
        }

        protected void CheckAccess(User user, params AccessRights[] accessRights)
        {
            if (user == null)
                user = GetCurrentUser();

            if (user.HaveAccessRight(accessRights))
                return;

            throw new AuthorizationException(accessRights);
        }
    }
}