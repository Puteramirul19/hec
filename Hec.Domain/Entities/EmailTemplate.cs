using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class EmailTemplate // Entity
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
        public string SubjectTemplate { get; set; }
        public string ContentTemplate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public EmailTemplate()
        {
            this.SubjectTemplate = "";
            this.ContentTemplate = "";
        }

        public EmailTemplate(string id, string description, string subjectTemplate, string contentTemplate)
        {
            this.Id = id;
            this.Description = description;
            this.SubjectTemplate = subjectTemplate;
            this.ContentTemplate = contentTemplate;
        }
        
        //public static string UserRegistrationSubmitted_Guest = "UserRegistrationSubmitted_Guest";
        //public static string UserRegistrationSubmitted_Admin = "UserRegistrationSubmitted_Admin";
        //public static string UserRegistrationRejected_Guest = "UserRegistrationRejected_Guest";
        //public static string UserRegistrationApproved_Guest = "UserRegistrationApproved_Guest";
        //public static string PasswordReset_User = "PasswordReset_User";
        public static string FriendInviteSubmitted_Invitee = "FriendInviteSubmitted_Invitee";
        public static string FriendInviteAccepted_Inviter = "FriendInviteAccepted_Inviter";
    }

    public interface IEmailTemplateSource
    {
        IDictionary<string, string> GetVariables();
    }
}
