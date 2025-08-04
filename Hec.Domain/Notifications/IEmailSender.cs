using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Notifications
{
    public interface IEmailSender
    {
        Task SendEmail(string toAddress, string toName, string subject, string htmlContent, IEnumerable<string> attachmentFileIds, DateTime scheduledOn, string objectId = null);
    }
}
