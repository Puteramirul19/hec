using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class Role : Entity
    {
        public Role()
        {
            this.SerializedAccessRights = "";
        }

        [Required]
        public string Name { get; set; }
        public string SerializedAccessRights { get; set; }
        public bool NeedZone { get; set; }
        public bool NeedSubZone { get; set; }
        public bool NeedUnit { get; set; }
        
        //-------------------------------------------------------------------------------- 
                
        private string GetAccessRightString(AccessRights accessRight)
        {
            return String.Concat("{", accessRight, "}");
        }

        public Role AddAccessRights(params AccessRights[] accessRights)
        {
            foreach (var accessRight in accessRights)
                this.AddAccessRight(accessRight);

            return this;
        }

        //-------------------------------------------------------------------------------- 
        
        public void AddAccessRight(AccessRights accessRight)
        {
            var str = GetAccessRightString(accessRight);
            if (this.SerializedAccessRights.Contains(str))
                return;

            this.SerializedAccessRights += str;
        }

        public void RemoveAccessRight(AccessRights accessRight)
        {
            this.SerializedAccessRights = this.SerializedAccessRights.Replace(GetAccessRightString(accessRight), "");
        }

        public bool HaveAccessRight(AccessRights accessRight)
        {
            return this.SerializedAccessRights.Contains(GetAccessRightString(accessRight));
        }

        public IEnumerable<AccessRights> GetAccessRightList()
        {
            return this.SerializedAccessRights
                    .Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Enum.Parse(typeof(AccessRights), x))
                    .Cast<AccessRights>();
        }
    }

    public class RoleNames
    {
        public static string Administrator = "Administrator";
        public static string Public = "Public";
    }

    public enum AccessRights
    {
        SuperUser,
        UseTools
    }
}
