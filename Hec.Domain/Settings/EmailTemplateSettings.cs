//using Hec.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Hec.Settings
//{
//    public class EmailTemplateSettings : ISettings
//    {
//        public EmailTemplate[] Templates { get; set; }

//        public EmailTemplateSettings()
//        {
//            this.Templates = new[]
//            {
//                new EmailTemplate(
//                    EmailTemplateType.BudgetAllocated,
//                    "Dihantar kepada....",
//                    "[TNB-SNMS] New Budget Allocation",
//                    "BudgetAllocated. <br><br><a href='{{BaseUrl}}'>{{BaseUrl}}</a>"
//                ),
//            };
//        }

//        //-------------------------------------------------------------------------------- 

//        public EmailTemplate GetTemplate(EmailTemplateType templateType)
//        {
//            return this.Templates.First(x => x.Type == templateType);
//        }

//        public void SetTemplate(EmailTemplateType templateType, EmailTemplate model)
//        {
//            var tpl = GetTemplate(templateType);
//            tpl.SubjectTemplate = model.SubjectTemplate;
//            tpl.ContentTemplate = model.ContentTemplate;
//        }

//        public ParsedTemplate ParseTemplate(EmailTemplateType templateType, IEmailTemplateSource obj)
//        {
//            var template = this.GetTemplate(templateType);
//            var parsed = new ParsedTemplate(template, obj);
//            return parsed;
//        }

//        public ParsedTemplate ParseTemplate(EmailTemplateType templateType, IDictionary<string, string> variables)
//        {
//            var template = this.GetTemplate(templateType);
//            var parsed = new ParsedTemplate(template, variables);
//            return parsed;
//        }
//    }

//    public interface IEmailTemplateSource
//    {
//        IDictionary<string, string> GetVariables();
//    }

//    public class ParsedTemplate
//    {
//        public string Subject { get; set; }
//        public string Content { get; set; }

//        public ParsedTemplate(EmailTemplate template, IEmailTemplateSource x)
//            : this(template, x.GetVariables())
//        {
//        }

//        public ParsedTemplate(EmailTemplate template, IDictionary<string, string> variables)
//        {
//            this.Subject = template.SubjectTemplate ?? "";
//            this.Content = template.ContentTemplate ?? "";

//            foreach (var variable in variables)
//            {
//                this.Subject = this.Subject.Replace("{{" + variable.Key + "}}", variable.Value ?? "");
//                this.Content = this.Content.Replace("{{" + variable.Key + "}}", variable.Value ?? "");
//            }
//        }
//    }

//    public class EmailTemplate
//    {
//        public EmailTemplateType Type { get; set; }
//        public string Description { get; set; }
//        public string SubjectTemplate { get; set; }
//        public string ContentTemplate { get; set; }

//        public EmailTemplate()
//        {
//            this.SubjectTemplate = "";
//            this.ContentTemplate = "";
//        }

//        public EmailTemplate(EmailTemplateType type, string description, string subjectTemplate, string contentTemplate)
//        {
//            this.Type = type;
//            this.Description = description;
//            this.SubjectTemplate = subjectTemplate;
//            this.ContentTemplate = contentTemplate;
//        }
//    }

//    public enum EmailTemplateType
//    {
//        BudgetAllocated,
//        NewRequest,
//        RequestNeedApproval,
//        RequestApproved,
//        RequestRejected,
//        NewReturn,
//        NewRequestAdditional,
//        EmergencyNeedApproval,
//        BudgetAllocatedToState
//    }
//}
