using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class UserRegistration : Entity
    {
        public string UserName { get; set; } 
        public string FullName { get; set; }
        public string AccountNo { get; set; }
        public UserRegistrationStatus Status { get; set; }
    }

    public enum UserRegistrationStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
