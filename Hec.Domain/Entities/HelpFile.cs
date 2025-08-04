using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hec.Entities
{
    public class HelpFile : Entity, ISortable
    {
        public string Name { get; set; }
        public string FileId { get; set; }
        public int Sequence { get; set; }
    }
}
