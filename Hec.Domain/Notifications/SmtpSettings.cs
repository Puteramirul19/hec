using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hec.Notifications
{
    public class SmtpSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }

        public SmtpSettings(string smtpHost, int smtpPort, string userName, string password, bool enableSsl, string fromAddress, string fromName)
        {
            this.SmtpHost = smtpHost;
            this.SmtpPort = smtpPort;
            this.UserName = userName;
            this.Password = password;
            this.EnableSsl = enableSsl;
            this.FromAddress = fromAddress;
            this.FromName = fromName;
        }
    }
}
