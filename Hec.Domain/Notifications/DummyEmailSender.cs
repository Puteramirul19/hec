using NLog;
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
    public class DummyEmailSender : IEmailSender
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Task SendEmail(string toAddress, string toName, string subject, string htmlContent, IEnumerable<string> attachmentFileIds, DateTime scheduledOn, string objectId = null)
        {
            var msg = String.Concat(
                "SEND EMAIL: ",
                " To=", toAddress,
                " (", toName, ")",
                " Subject=", subject,
                " Content=", htmlContent);

            System.Diagnostics.Debug.WriteLine(msg);
            logger.Trace(msg);

            return Task.FromResult(0);
        }
    }
}
