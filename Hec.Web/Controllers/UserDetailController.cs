//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Hec.Entities;
//using Hec.FileStorage;
//using Microsoft.AspNet.Identity;
//using System.Web.Mvc;
//using Kendo.Mvc.UI;

//namespace Hec.Web.Controllers
//{
//    public class UserDetail
//    {
//        public string Id { get; set; }
//        public string Password { get; set; }
//        public string UserName { get; set; }
//        public string FullName { get; set; }
//        public string Email { get; set; }
//        public string PhoneNumber { get; set; }
//        public string Photo { get; set; }
//        public LoginType LoginType { get; set; }
//        public DateTime LastLogin { get; set; }
//        public bool IsActive { get; set; }
//        public Guid RoleId { get; set; }
//        public string RoleName { get; set; }
//        public string Designation { get; set; }
//        public string Department { get; set; }
//        //public Guid? RegionId { get; set; }
//        //public string RegionName { get; set; }
//        //public Guid? ZoneId { get; set; }
//        //public string ZoneName { get; set; }
//        //public Guid? StateId { get; set; }
//        //public string StateName { get; set; }
//        //public Guid? DistrictId { get; set; }
//        //public string DistrictName { get; set; }

//        public IEnumerable<UserDetailFundLocation> FundLocations { get; set; }
//        public IEnumerable<UserDetailBusinessArea> BusinessAreas { get; set; }
//    }

//    public class UserDetailFundLocation
//    {
//        public Guid FundLocationId { get; set; }
//        public string FundLocationName { get; set; }
//    }

//    public class UserDetailBusinessArea
//    {
//        public Guid BusinessAreaId { get; set; }
//        public string BusinessAreaName { get; set; }
//    }

//    [Authorize]
//    public class UserDetailController : BaseController
//    {
//        private UserManager<User> userManager;

//        public UserDetailController(HecContext db, UserManager<User> userManager)
//            : base(db)
//        {
//            this.userManager = userManager;
//        }

//        public async Task<ActionResult> Read(string id)
//        {
//            var entity = await db.Users
//                        .Include(x => x.Role)
//                        .FirstOrDefaultAsync(x => x.Id == id);

//            if (entity == null)
//                throw new IdNotFoundException<User>(id);

//            return Json(new UserDetail
//            {
//                Id = entity.Id,
//                UserName = entity.UserName,
//                FullName = entity.FullName,
//                Email = entity.Email,
//                PhoneNumber = entity.PhoneNumber,
//                Photo = entity.Photo,
//                LoginType = entity.LoginType,
//                LastLogin = entity.LastLogin,
//                IsActive = entity.IsActive,
//                RoleId = entity.RoleId,
//                RoleName = (entity.Role != null) ? entity.Role.Name : "",
//                Designation = entity.Designation,
//                Department = entity.Department,
//                //RegionId = entity.RegionId,
//                //RegionName = (entity.Region != null) ? entity.Region.Name : "",
//                //ZoneId = entity.ZoneId,
//                //ZoneName = (entity.Zone != null) ? entity.Zone.Name : "",
//                //StateId = entity.StateId,
//                //StateName = (entity.State != null) ? entity.State.Name : "",
//                //DistrictId = entity.DistrictId,
//                //DistrictName = (entity.District != null) ? entity.District.Name : ""
//            });
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public async Task<ActionResult> Update(string id, UserDetail model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return Status(HttpStatusCode.BadRequest);
//            }

//            if (id != model.Id)
//            {
//                return Status(HttpStatusCode.BadRequest);
//            }

//            var entity = await db.Users
//                    .FirstOrDefaultAsync(x => x.Id == id);

//            if (entity == null)
//                throw new IdNotFoundException<User>(id);

//            entity.FullName = model.FullName;
//            entity.Email = model.Email;
//            entity.PhoneNumber = model.PhoneNumber;
//            entity.Photo = model.Photo;
//            entity.LoginType = model.LoginType;
//            entity.LastLogin = model.LastLogin;
//            entity.RoleId = model.RoleId;
//            entity.Designation = model.Designation;
//            entity.Department = model.Department;
//            //entity.RegionId = model.RegionId;
//            //entity.ZoneId = model.ZoneId;
//            //entity.StateId = model.StateId;
//            //entity.DistrictId = model.DistrictId;

//            db.SetModified(entity);
//            await db.SaveChangesAsync();

//            return Json(model);
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public async Task<ActionResult> Create(UserDetail model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return Status(HttpStatusCode.BadRequest);
//            }

//            var existing = await db.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
//            if (existing != null)
//                throw new Exception("Username already exists.");

//            var entity = new User();
//            entity.Id = entity.UserName = model.UserName;
//            entity.FullName = model.FullName;
//            entity.Email = model.Email;
//            entity.PhoneNumber = model.PhoneNumber;
//            entity.Photo = model.Photo;
//            entity.LoginType = model.LoginType;
//            entity.Designation = model.Designation;
//            entity.Department = model.Department;
//            entity.RoleId = model.RoleId;
//            //entity.RegionId = model.RegionId;
//            //entity.ZoneId = model.ZoneId;
//            //entity.StateId = model.StateId;
//            //entity.DistrictId = model.DistrictId;
                        
//            if (model.LoginType == LoginType.Internal)
//            {
//                entity.PasswordHash = userManager.PasswordHasher.HashPassword(model.Password);
//            }

//            db.Users.Add(entity);
//            await db.SaveChangesAsync();

//            model.Id = entity.Id;
//            return Json(model);
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public async Task<ActionResult> Destroy(Guid key)
//        {
//            var entity = db.Users.Find(key);
//            if (entity == null)
//            {
//                return Status(HttpStatusCode.NotFound);
//            }

//            db.SetDeleted(entity);
//            await db.SaveChangesAsync();

//            return Ok();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public async Task<ActionResult> ChangePassword(string userId, string password)
//        {
//            //CheckAccess(AccessRights.ManageUser);

//            if (!ModelState.IsValid)
//            {
//                return Status(HttpStatusCode.BadRequest);
//            }

//            var user = db.Users.Find(userId);
//            if (user == null)
//                throw new IdNotFoundException<User>(userId);

//            user.PasswordHash = userManager.PasswordHasher.HashPassword(password);
//            db.SetModified(user);
//            await db.SaveChangesAsync();
            
//            return Ok();
//        }
//    }
//}
