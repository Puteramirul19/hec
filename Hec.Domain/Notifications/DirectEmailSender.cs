using Hec.Entities;
using Hec.FileStorage;
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
    /// An email sender that will send the email directly.
    /// </summary>
    public class DirectEmailSender : IEmailSender
    {
        private SmtpSettings smtpSettings;
        private IFileStore fileStore;

        public DirectEmailSender(SmtpSettings smtpSettings, IFileStore fileStore)
        {
            this.smtpSettings = smtpSettings;
            this.fileStore = fileStore;
        }

        public async Task SendEmail(string toAddress, string toName, string subject, string htmlContent, IEnumerable<string> attachmentFileIds, DateTime scheduledOn, string objectId = null)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(smtpSettings.FromAddress, smtpSettings.FromName);
            msg.To.Add(new MailAddress(toAddress, toName));
            msg.Subject = subject;

            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(EmailConfig.TextContent, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlContent, null, MediaTypeNames.Text.Html));

            if (attachmentFileIds != null && attachmentFileIds.Count() > 0)
            {
                foreach (var fileId in attachmentFileIds)
                {
                    if (!String.IsNullOrEmpty(fileId))
                        msg.Attachments.Add(new Attachment(fileStore.GetStream(fileId), fileId, System.Web.MimeMapping.GetMimeMapping(fileId)));
                }
            }

            SmtpClient smtpClient = new SmtpClient(smtpSettings.SmtpHost, smtpSettings.SmtpPort);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
            smtpClient.EnableSsl = smtpSettings.EnableSsl;
            
            await smtpClient.SendMailAsync(msg);
        }
    }
}
