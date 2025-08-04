using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Http.ModelBinding;
using Hec.Entities;
using Hec.Auth;

namespace Hec.Web.Controllers
{
    [Authorize]
    public class DirectoryUserController : BaseController
    {
        private IDirectoryService directory;

        public DirectoryUserController(HecContext db, IDirectoryService directory)
            : base(db)
        {
            this.directory = directory;
        }
        
        public ActionResult Read(string id)
        {
            var user = directory.GetUserByStaffNo(id);
            if (user == null)
            {
                return Status(HttpStatusCode.NotFound);
            }

            user.IsRegistered = db.Users.Any(x => x.Id == id);
            return Json(user);
        }
    }
}
