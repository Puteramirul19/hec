using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hec;
using Hec.Entities;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class RoleList
    {
        public IEnumerable<string> AllAccessRights { get; set; }
        public IEnumerable<RoleListItem> Roles { get; set; }
    }

    public class RoleListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoleListItemAccessRight> AccessRights { get; set; }
    }

    public class RoleListItemAccessRight
    {
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }

    public class UpdateAccessRightModel
    {
        public Guid RoleId { get; set; }
        public AccessRights AccessRight { get; set; }
        public bool Assigned { get; set; }
    }

    [Authorize]
    public class RolesController : BaseController
    {
        public RolesController(HecContext db) : base(db)
        {
        }

        public ActionResult Index()
        {
            var allAccessRights = Enum.GetValues(typeof(AccessRights)).Cast<AccessRights>();

            var roleList = new RoleList()
            {
                AllAccessRights = allAccessRights.Select(x => x.ToString()),
                Roles = new List<RoleListItem>()
            };

            db.Roles.OrderBy(x => x.Name).ToList().ForEach(x =>
            {
                var role = new RoleListItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    AccessRights = new List<RoleListItemAccessRight>()
                };

                foreach (var accessRight in allAccessRights)
                {
                    ((List<RoleListItemAccessRight>) role.AccessRights).Add(
                        new RoleListItemAccessRight
                        {
                            Name = accessRight.ToString(),
                            Assigned = x.HaveAccessRight(accessRight)
                        }
                    );
                }

                ((List<RoleListItem>) roleList.Roles).Add(role);
            });

            return View(roleList);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateAccessRight(UpdateAccessRightModel model)
        {
            if (!ModelState.IsValid)
                return Status(HttpStatusCode.BadRequest);

            var role = db.Roles.Find(model.RoleId);
            if (role == null)
                throw new IdNotFoundException<Role>(model.RoleId);

            if (model.Assigned)
                role.AddAccessRight(model.AccessRight);
            else
                role.RemoveAccessRight(model.AccessRight);

            db.Entry(role).State = EntityState.Modified;

            await db.SaveChangesAsync();

            //System.Threading.Thread.Sleep(1000);
            return Ok();
        }

        //// GET: Roles/Details/5
        //public async Task<ActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Role role = await db.Roles.FindAsync(id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude= "Id")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.Id = Guid.NewGuid();
                db.Roles.Add(role);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Role role = await db.Roles.FindAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Roles.FindAsync(role.Id);
                if (entity == null)
                    return NotFound();

                entity.Name = role.Name;
                entity.NeedZone = role.NeedZone;
                entity.NeedSubZone = role.NeedSubZone;
                entity.NeedUnit = role.NeedUnit;

                //db.Entry(role).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        //// GET: Roles/Delete/5
        //public async Task<ActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Role role = await db.Roles.FindAsync(id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        //// POST: Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(Guid id)
        //{
        //    Role role = await db.Roles.FindAsync(id);
        //    db.Roles.Remove(role);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
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
