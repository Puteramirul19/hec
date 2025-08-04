using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Hec.Integrations;

namespace Hec.Entities
{
    public class User : IdentityUser, IUser
    {
        public User()
        {
            this.SecurityStamp = Guid.NewGuid().ToString();
            this.IsActive = true;
        }

        public static string DEFAULT_LANGUAGE = "en";

        public LoginType LoginType { get; set; }

        public DateTime LastLogin { get; set; }
        public string FullName { get; set; }
        public string Nric { get; set; }
        public string Photo { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string OfficeNumber { get;  set;}
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int LoginCount { get; set; }

        //-------------------------------------------------------------------------------- 

        public string SspUserId { get; set; }
        public string SspUserToken { get; set; }

        //-------------------------------------------------------------------------------- 

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        //-------------------------------------------------------------------------------- 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //-------------------------------------------------------------------------------- 

        public bool HaveAccessRight(params AccessRights[] accessRights)
        {
            if (Role.HaveAccessRight(AccessRights.SuperUser))
                return true;

            foreach (var accessRight in accessRights)
            {
                if (Role.HaveAccessRight(accessRight))
                    return true;
            }

            return false;
        }

        //-------------------------------------------------------------------------------- 

        public ContractAccount GetDefaultAccount(HecContext db)
        {
            return db.ContractAccounts
                    .Include(x => x.User)
                    .FirstOrDefault(x => x.UserId == this.Id && x.IsDefault == true);
        }

        //-------------------------------------------------------------------------------- 

        public decimal? BaseConsumption { get; set; } // For ranking calculation

        public void SetBaseAmount(HecContext db, IBcrmService bcrmService)
        {
            // Average 6 latest month
            var account = this.GetDefaultAccount(db);
            var bills = account.GetBills(db, bcrmService);
            var latests = bills.OrderByDescending(x => x.MonthYear).Take(6);
            this.BaseConsumption = (latests.Count() > 0)
                                    ? latests.Average(x => x.Consumption)
                                    : 0m;
        }

    }

    public enum LoginType
    {
        Internal,
        ActiveDirectory,
        Ssp
    }
}
