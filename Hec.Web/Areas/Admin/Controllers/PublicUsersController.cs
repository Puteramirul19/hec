using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Hec.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Hec.Web.Controllers;
//using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class PublicUserGridItem
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime LastLogin { get; set; }
        public int LoginCount { get; set; }
    }

    [Authorize]
    public class PublicUsersController : BaseController
    {
        public PublicUsersController(HecContext db) : base(db)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<User> users = db.Users.Include(x => x.Role).Where(x => x.Role.Name == RoleNames.Public);
            DataSourceResult result = users.Select(x => new PublicUserGridItem
                                        {
                                            Id = x.Id,
                                            UserName = x.UserName,
                                            LastLogin = x.LastLogin,
                                            FullName = x.FullName,
                                            LoginCount = x.LoginCount
                                        })
                                        .ToDataSourceResult(request);

            return Json(result);
        }

        //public async Task<ActionResult> Detail(string id)
        //{
        //    var entity = await db.Users
        //                //.Include(x => x.Role)
        //                .FirstOrDefaultAsync(x => x.Id == id);

        //    if (entity == null)
        //        throw new IdNotFoundException<User>(id);

        //    return Json(new UserDetail
        //    {
        //        Id = entity.Id,
        //        UserName = entity.UserName,
        //        FullName = entity.FullName,
        //        Email = entity.Email,
        //        PhoneNumber = entity.PhoneNumber,
        //        Photo = entity.Photo,
        //        LoginType = entity.LoginType,
        //        LastLogin = entity.LastLogin,
        //        IsActive = entity.IsActive,
        //        Designation = entity.Designation,
        //        Department = entity.Department,
        //        RoleId = entity.RoleId
        //    });
        //}
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
