using Hec.Entities;
using Hec.Settings;
using Hec.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Notifications
{
    /// <summary>
    /// An email sender that puts email in DB queue. A scheduled task will later on pick it up and dispatch it.
    /// </summary>
    public class QueuedEmailSender : IEmailSender
    {
        private HecContext db;

        public static char ATTACHMENT_SEPARATOR = '§';

        public QueuedEmailSender(HecContext db)
        {
            this.db = db;
        }

        public async Task SendEmail(string toAddress, string toName, string subject, string htmlContent, IEnumerable<string> attachmentFileIds, DateTime scheduledOn, string objectId)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(toAddress, toName));
            msg.Subject = subject;

            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(EmailConfig.TextContent, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlContent, null, MediaTypeNames.Text.Html));

            //Add mail into queue
            db.EmailQueues.Add(new EmailQueue
            {
                Id = Guid.NewGuid(),
                Subject = subject,
                ToAddress = toAddress,
                ToName = toName,
                Content = htmlContent,
                Attachments = (attachmentFileIds != null && attachmentFileIds.Count() > 0) ? String.Join(ATTACHMENT_SEPARATOR.ToString(), attachmentFileIds.ToArray()) : "",
                CreatedOn = DateTime.Now,
                ScheduledOn = scheduledOn,
                SentOn = null,
                ObjectId = objectId
            });

            await db.SaveChangesAsync();
        }
    }
}
