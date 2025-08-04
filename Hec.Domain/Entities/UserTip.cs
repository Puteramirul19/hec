using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class UserTip : Entity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public Guid TipId { get; set; }
        public virtual Tip Tip { get; set; }
        public UserTipStatus Status { get; set; }
    }

    public enum UserTipStatus
    {
        Doing,
        Done,
        NoThanks
    }
}
