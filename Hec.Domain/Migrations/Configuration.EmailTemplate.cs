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
        public object EmailTempalate { get; private set; }

        private void SeedEmailTemplate(HecContext db)
        {
            db.EmailTemplates.AddOrUpdate(
                x => x.Id,
                //new EmailTemplate(EmailTemplate.UserRegistrationSubmitted_Guest,
                //    "Sent to Guest when user registration is submitted.",
                //    "[TNB-HEC] User Registration Submitted",
                //    @"Dear {{ReceiverName}},
                //    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                //    <br><br>
                //    Thank you."
                //),
                //new EmailTemplate(EmailTemplate.UserRegistrationSubmitted_Admin,
                //    "Sent to Admin when user registration is submitted.",
                //    "[TNB-HEC] User Registration Submitted",
                //    @"Dear {{ReceiverName}},
                //    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                //    <br><br>
                //    Thank you."
                //),
                //new EmailTemplate(EmailTemplate.UserRegistrationRejected_Guest,
                //    "Sent to Guest when user registration is rejected.",
                //    "[TNB-HEC] User Registration Rejected",
                //    @"Dear {{ReceiverName}},
                //    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                //    <br><br>
                //    Thank you."
                //),
                //new EmailTemplate(EmailTemplate.UserRegistrationApproved_Guest,
                //    "Sent to Guest when user registration is approved.",
                //    "[TNB-HEC] User Registration Approved",
                //    @"Dear {{ReceiverName}},
                //    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                //    <br><br>
                //    Thank you."
                //),
                //new EmailTemplate(EmailTemplate.PasswordReset_User,
                //    "Sent to User when user request a password reset.",
                //    "[TNB-HEC] Password Reset",
                //    @"Dear {{ReceiverName}},
                //    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                //    <br><br>
                //    Thank you."
                //),
                new EmailTemplate(EmailTemplate.FriendInviteSubmitted_Invitee,
                    "Sent to the Invitee when friend invitation is requested",
                    "[TNB-HEC] Friend Invitation Request",
                    @"Dear {{InviteeName}},
                    <br>You have an invitation from {{InviterName}}. Please login to accept the invitation.
                    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                    <br><br>
                    Thank you."
                ),
                new EmailTemplate(EmailTemplate.FriendInviteAccepted_Inviter,
                    "Sent to the Inviter when friend invitation is accepted",
                    "[TNB-HEC] Friend Invitation Accepted",
                    @"Dear {{InviterName}},
                    <br>{{InviteeName}} has accepted your invitation. Please login to continue.
                    <br><a href='{{BaseUrl}}'>Go to TNB HEC.</a>
                    <br><br>
                    Thank you."
                )
            );                             

            db.SaveChanges();
        }
    }
}
