using Hec.Entities;
using Hec.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace Hec.Notifications
{
    public class ParsedTemplate
    {
        public string Subject { get; set; }
        public string Content { get; set; }

        public static ParsedTemplate Parse(EmailTemplate template, IDictionary<string, string> variables)
        {
            var parsed = new ParsedTemplate();
            parsed.Subject = template.SubjectTemplate ?? "";
            parsed.Content = template.ContentTemplate ?? "";

            variables["BaseUrl"] = ConfigurationManager.AppSettings["BaseUrl"];

            foreach (var variable in variables)
            {
                parsed.Subject = parsed.Subject.Replace("{{" + variable.Key + "}}", variable.Value ?? "");
                parsed.Content = parsed.Content.Replace("{{" + variable.Key + "}}", variable.Value ?? "");
            }

            return parsed;
        }

        public static ParsedTemplate Parse(EmailTemplate template, IEmailTemplateSource source)
        {
            var variables = source.GetVariables();
            return Parse(template, variables);
        }
    }
}
