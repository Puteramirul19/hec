using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class Friendship : Entity, IEmailTemplateSource
    {
        public string InviterId { get; set; }
        public virtual User Inviter { get; set; }
        public string InviteeId { get; set; }
        public virtual User Invitee { get; set; }
        public bool IsAccepted { get; set; }

        public IDictionary<string, string> GetVariables()
        {
            return new Dictionary<string, string>
            {
                { "Id", Id.ToString() },
                { "InviterId", InviterId },
                { "InviterName", Inviter.FullName },
                { "InviteeId", InviteeId },
                { "InviteeName", Invitee.FullName }
            };
        }
    }
}
