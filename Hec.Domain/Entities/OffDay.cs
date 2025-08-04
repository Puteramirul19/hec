using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hec.Entities
{
    public class OffDay : Entity
    {
        public int Year { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public Guid StateId { get; set; }
        public virtual State State { get; set; }
    }
}
