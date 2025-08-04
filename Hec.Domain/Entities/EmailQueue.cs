using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hec.Entities
{
    public class EmailQueue : Entity
    {
        public string Subject { get; set; }
        public string ToAddress { get; set; }
        public string ToName { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
        //public DateTime CreatedOn { get; set; }
        public DateTime ScheduledOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public DateTime? SentOn { get; set; }
        public string ObjectId { get; set; }
    }
}
