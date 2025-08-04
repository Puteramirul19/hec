using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Auth
{
    public class DirectoryUser
    {
        [Key]
        public string StaffNo { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string OfficeNumber { get; set; }

        public string Photo { get; set; }

        /// <summary>
        /// If user is already registered
        /// </summary>
        public bool IsRegistered { get; set; }
    }
}
