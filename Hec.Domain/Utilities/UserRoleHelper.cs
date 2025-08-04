//using Hec.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.Entity;

//namespace Hec.Workflows
//{
//    public class UserRoleHelper
//    {
//        private HecContext db;

//        public UserRoleHelper(HecContext db)
//        {
//            this.db = db;
//        }

//        public virtual IEnumerable<User> GetAuthorizedUsers(AccessRights accessRight)
//        {
//            var roles = db.Roles.ToList().Where(x => x.HaveAccessRight(accessRight));

//            var users = new List<User>();
//            foreach (var role in roles)
//            {
//                users.AddRange(db.UserRoles.Where(x => x.RoleId == role.Id && x.User.IsActive == true).Select(x => x.User));
//            }

//            return users.Distinct();
//        }

//        public IEnumerable<User> GetAuthorizedUsersBySubZone(AccessRights accessRight, Guid subZoneId)
//        {
//            var roles = db.Roles.ToList().Where(x => x.HaveAccessRight(accessRight));

//            var users = new List<User>();
//            foreach (var role in roles)
//            {
//                users.AddRange(db.UserRoles
//                    .Where(x => x.RoleId == role.Id && x.SubZoneId.HasValue == true && x.SubZoneId.Value == subZoneId && x.User.IsActive == true)
//                    .Select(x => x.User));
//            }

//            return users.Distinct();
//        }

//        public IEnumerable<User> GetAuthorizedUsersByZone(AccessRights accessRight, Guid zoneId)
//        {
//            var roles = db.Roles.ToList().Where(x => x.HaveAccessRight(accessRight));

//            var users = new List<User>();
//            foreach (var role in roles)
//            {
//                users.AddRange(db.UserRoles
//                    .Where(x => x.RoleId == role.Id && x.ZoneId.HasValue == true && x.ZoneId.Value == zoneId && x.User.IsActive == true)
//                    .Select(x => x.User));
//            }

//            return users.Distinct();
//        }
//    }
//}
