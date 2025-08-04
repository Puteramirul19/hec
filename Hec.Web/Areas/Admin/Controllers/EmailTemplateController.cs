using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Hec.Entities;
using Hec.Settings;
using System.Web.Mvc;
using Hec.Notifications;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class EmailTemplateController : BaseController
    {
        public EmailTemplateController(HecContext db)
            : base(db)
        {
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Email Templates";
            return View();
        }

        public async Task<ActionResult> Read(string id)
        {
            var template = await db.EmailTemplates.FindAsync(id);
            if (template == null)
                throw new IdNotFoundException<EmailTemplate>(id);

            return Json(template);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public async Task<ActionResult> Update(string id, EmailTemplate model)
        {
            CheckAccess(AccessRights.SuperUser);

            db.SetModified(model);
            await db.SaveChangesAsync();

            return Json(model);
        }

        public class TemplatePreview
        {
            public string SubjectTemplate { get; set; }
            public string ContentTemplate { get; set; }

            public string Subject { get; set; }
            public string Content { get; set; }
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Preview(TemplatePreview model)
        {
            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            var now = DateTime.Now;
            var dummy = new Dictionary<string, string>
            {
                { "Id", "SAMPLE Id" },
                { "InviterId", "SAMPLE InviterId" },
                { "InviterName", "SAMPLE InviterName" },
                { "InviteeId", "SAMPLE InviteeId" },
                { "InviteeName", "SAMPLE InviteeName" },
                { "Title", "SAMPLE Title" },
                { "BaseUrl", System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]}
            };

            var parsed = ParsedTemplate.Parse(
                new EmailTemplate
                {
                    SubjectTemplate = model.SubjectTemplate,
                    ContentTemplate = model.ContentTemplate
                }, dummy);

            // Remove template to minimize output, we only need parsed strings
            model.SubjectTemplate = null;
            model.ContentTemplate = null;
            model.Subject = parsed.Subject;
            model.Content = parsed.Content;

            return Json(model);
        }
    }
}
