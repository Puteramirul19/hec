using NLog;
using Hec.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hec.Jobs
{
    /// <summary>
    /// Task to do send emails in the EmailQueue.
    /// </summary>
    public class EmailDispatchJob
    {
        private HecContext db;
        private DirectEmailSender emailSender;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EmailDispatchJob(HecContext db, DirectEmailSender emailSender)
        {
            this.db = db;
            this.emailSender = emailSender;
        }

        private const int BATCH_SIZE = 250;

        public void Run()
        {
            var enabled = ConfigurationManager.AppSettings["EmailDispatchEnabled"] == "true";
            if (!enabled)
            {
                logger.Debug("Trying to send email, but EmailDispatchEnable != true");
                return;
            }

            logger.Debug("Sending emails in email queue...");

            var emails = db.EmailQueues
                            .Where(x => x.ProcessedOn == null && x.ScheduledOn < DateTime.Now)
                            .OrderBy(x => x.CreatedOn)
                            .Take(BATCH_SIZE)
                            .ToList();

            var totalCount = emails.Count();
            if (totalCount == 0)
            {
                logger.Debug("No queued emails to be sent.");
            }

            // Update as being processed
            foreach (var email in emails)
            {
                email.ProcessedOn = DateTime.Now;
                db.Entry(email).State = EntityState.Modified;
            }
            db.SaveChanges();

            var successCount = 0;
            var exceptions = "";
            foreach (var email in emails)
            {
                try
                {
                    string[] attachmentFileIds = null;
                    if (email.Attachments != null)
                        attachmentFileIds = email.Attachments.Split(QueuedEmailSender.ATTACHMENT_SEPARATOR);

                    AsyncHelper.RunSync(() => emailSender.SendEmail(email.ToAddress, email.ToName, email.Subject, email.Content, attachmentFileIds, DateTime.Now));

                    successCount++;

                    email.SentOn = DateTime.Now;
                }
                catch (Exception e)
                {
                    exceptions += "<div>" + e.ToString() + "</div>";
                    email.ProcessedOn = null;
                }
                db.Entry(email).State = EntityState.Modified;
            }

            db.SaveChanges();

            var msg = (successCount > 0)
                        ? "DONE. " + successCount + " emails successfully sent out of " + totalCount + " (BATCH_SIZE=" + BATCH_SIZE + ")."
                        : "DONE. Unable to send queued emails. (" + totalCount + " emails). Exceptions: " + exceptions;
            logger.Debug(msg);
        }
    }
}