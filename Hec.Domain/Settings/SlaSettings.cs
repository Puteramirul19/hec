using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Settings
{
    /*
    +-----------------------+----------------------+--------------------+----------------------+------------------+
    | Stage                 |  SLA                 | Type               | Receiver             |  Interval        |
    |                       |                      |                    |                      |  Duration        |
    +=======================+======================+====================+======================+==================+
    | New Notice Creation   | Immediately          | Acknowledgement/   | Sub Zone Author,     |  One-time        |
    |                       |                      | Notification       | Sub Zone Reviewer    |                  |
    +-----------------------+----------------------+--------------------+----------------------+------------------+
    | Review & Approval     | < 4 days             | Reminder Notice    | Sub Zone Reviewer    |  Every 24 hrs    |
    | of Notice             |                      |                    |                      |  until completed |
    +-----------------------+----------------------+--------------------+----------------------+------------------+
    | Approval Decision     | Immediately          | Acknowledgement/   | Sub Zone Author,     |  One-time        |
    |                       |                      | Notification       | Sub Zone Reviewer    |                  |
    +-----------------------+----------------------+--------------------+----------------------+------------------+
    | Withdraw Notice       | Immediately          | Acknowledgement/   | Sub Zone Author,     |  One-time        |
    |                       |                      | Notification       | Sub Zone Reviewer    |                  |
    +-----------------------+----------------------+--------------------+----------------------+------------------+     
     */

    public class SlaSettings : ISettings
    {
        public SlaItem ReviewReminder { get; set; }
        public SlaItem ReviewAlert { get; set; }

        public SlaSettings()
        {
            this.ReviewReminder = new SlaItem
            {
                Due = 0,
                ReminderFrequencyType = IntervalType.Hours,
                ReminderFrequencyPeriod = 24
            };

            this.ReviewAlert = new SlaItem
            {
                Due = 4, // days before shutdown date
                ReminderFrequencyType = IntervalType.Hours,
                ReminderFrequencyPeriod = 24
            };
        }
    }

    public enum IntervalType
    {
        None,
        Hours,
        Days,
        Weeks
    }

    public class SlaItem
    {
        public int Due { get; set; } // In days, for example: must do this in less than 3 days.
        public IntervalType ReminderFrequencyType { get; set; }
        public int ReminderFrequencyPeriod { get; set; }
    }
}
