using Hec.Entities;
using Hec.IdGeneration;
using Hec.Workflows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Hec.Migrations
{
    public partial class Configuration
    {
        private void SeedUserRole(HecContext db)
        {
            db.Roles.AddOrUpdate(
                x => x.Name,
                new Role { Id = new Guid("207BC1CD-D12E-47E6-A998-47C2F1DC304C"), Name = RoleNames.Administrator }.AddAccessRights(
                    AccessRights.SuperUser
                ),
                new Role { Id = new Guid("207BC1CD-D12E-47E6-A998-47C2F1DC304D"), Name = RoleNames.Public }.AddAccessRights(
                    AccessRights.UseTools
                )
            );

            db.SaveChanges();

            var roles = db.Roles.ToDictionary(x => x.Name);
            var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("q");

            Func<string, Action<User>, User> createUserAdmin = (username, func) =>
            {
                var u = new User
                {
                    Id = username,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = passwordHash,
                    UserName = username,
                    FullName = "Administrator",
                    Email = "akaunkeduaku@gmail.com",
                    PhoneNumber = "0135324324",
                    LoginType = LoginType.Internal,
                    IsActive = true,
                    RoleId = roles[RoleNames.Administrator].Id
                };
                
                if (func != null)
                    func(u);

                return u;
            };

            Func<string, Action<User>, User> createUserPublic = (username, func) =>
            {
                var id = username + "@hec.tnb.com.my";
                var u = new User
                {
                    Id = id,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = passwordHash,
                    UserName = id,
                    FullName = "Public User " + username,
                    Email = id, //"akaunkeduaku@gmail.com",
                    PhoneNumber = "0135324324",
                    LoginType = LoginType.Ssp,
                    IsActive = true,
                    RoleId = roles[RoleNames.Public].Id
                };

                if (func != null)
                    func(u);

                return u;
            };

            db.Users.AddOrUpdate(
                x => x.UserName,
                createUserAdmin("admin",  null),
                createUserPublic("public1", (u) => u.FullName = "Public User One"),
                createUserPublic("public2", (u) => u.FullName = "Public User Two"),
                createUserPublic("public3", (u) => u.FullName = "Public User Three"),
                createUserPublic("public4", (u) => u.FullName = "Public User Four"),
                createUserPublic("public5", (u) => u.FullName = "Public User Five"),
                createUserPublic("public6", (u) => u.FullName = "Public User Six"),
                createUserPublic("public7", (u) => u.FullName = "Public User Seven"),
                createUserPublic("public8", (u) => u.FullName = "Public User Eight"),
                createUserPublic("public9", (u) => u.FullName = "Public User Nine")
            );

            db.SaveChanges();
        }
    }
}
