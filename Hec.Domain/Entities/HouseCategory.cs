using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class HouseCategory : Entity
    {
        public string HouseCategoryName { get; set; }
        public string HouseCategoryDesc { get; set; }
        public int Sequence { get; set; }
    }
}
