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
    public class UserDetail
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string OfficeNumber { get;  set;}
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public LoginType LoginType { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }

    public class UserGridItem
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public LoginType LoginType { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
    }

    [Authorize]
    public class UsersController : BaseController
    {
        private UserManager<User> userManager;

        public UsersController(HecContext db, UserManager<User> userManager) : base(db)
        {
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<User> users = db.Users.Include(x => x.Role).Where(x => x.Role.Name == RoleNames.Administrator); // Admin only!
            DataSourceResult result = users.Select(x => new UserGridItem
                                        {
                                            Id = x.Id,
                                            UserName = x.UserName,
                                            LoginType = x.LoginType,
                                            LastLogin = x.LastLogin,
                                            FullName = x.FullName,
                                            IsActive = x.IsActive,
                                            RoleName = x.Role.Name

                                        })
                                        .ToDataSourceResult(request);

            return Json(result);
        }

        public async Task<ActionResult> Detail(string id)
        {
            var entity = await db.Users
                        //.Include(x => x.Role)
                        .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw new IdNotFoundException<User>(id);

            return Json(new UserDetail
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FullName = entity.FullName,
                Email = entity.Email,
                OfficeNumber = entity.OfficeNumber,
                PhoneNumber = entity.PhoneNumber,
                Photo = entity.Photo,
                LoginType = entity.LoginType,
                LastLogin = entity.LastLogin,
                IsActive = entity.IsActive,
                Designation = entity.Designation,
                Department = entity.Department,
                RoleId = entity.RoleId
            });
        }

        public ActionResult Edit(string id)
        {
            CheckAccess(AccessRights.SuperUser);

            return View((object)id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Update(string id, UserDetail model)
        {
            CheckAccess(AccessRights.SuperUser);

            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            if (id != model.Id)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            var entity = await db.Users
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw new IdNotFoundException<User>(id);

            entity.FullName = model.FullName;
            entity.Email = model.Email;
            entity.OfficeNumber = model.OfficeNumber;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Photo = model.Photo;
            entity.LoginType = model.LoginType;
            entity.LastLogin = model.LastLogin;
            entity.Designation = model.Designation;
            entity.Department = model.Department;
            entity.IsActive = model.IsActive;
            entity.RoleId = model.RoleId;

            db.SetModified(entity);
            await db.SaveChangesAsync();

            return Json(model);
        }

        public ActionResult Create()
        {
            CheckAccess(AccessRights.SuperUser);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Create(UserDetail model)
        {
            CheckAccess(AccessRights.SuperUser);

            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            var existing = await db.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (existing != null)
                throw new Exception("Username already exists.");

            var entity = new User();
            entity.Id = entity.UserName = model.UserName;
            entity.FullName = model.FullName;
            entity.Email = model.Email;
            entity.OfficeNumber = model.OfficeNumber;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Photo = model.Photo;
            entity.LoginType = model.LoginType;
            entity.Designation = model.Designation;
            entity.Department = model.Department;
            entity.RoleId = model.RoleId;

            if (model.LoginType == LoginType.Internal)
            {
                entity.PasswordHash = userManager.PasswordHasher.HashPassword(model.Password);
            }

            db.Users.Add(entity);
            await db.SaveChangesAsync();

            model.Id = entity.Id;
            return Json(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Destroy(Guid key)
        {
            CheckAccess(AccessRights.SuperUser);

            var entity = db.Users.Find(key);
            if (entity == null)
            {
                return Status(HttpStatusCode.NotFound);
            }

            db.SetDeleted(entity);
            await db.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ChangePassword(string id)
        {
            CheckAccess(AccessRights.SuperUser);
            return View((object)id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> ChangePassword(string userId, string password)
        {
            CheckAccess(AccessRights.SuperUser);

            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(userId);
            if (user == null)
                throw new IdNotFoundException<User>(userId);

            user.PasswordHash = userManager.PasswordHasher.HashPassword(password);
            db.SetModified(user);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
