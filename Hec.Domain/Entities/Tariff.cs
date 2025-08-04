using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class Tariff : Entity
    {
        public string Description { get; set; }
        public double TariffPerKWh { get; set; }
        public int BoundryTier { get; set; }
        public int CummulativeKWh { get; set; }
        public int Sequence { get; set; }
    }
}
