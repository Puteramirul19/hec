using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public interface ISortable
    {
        Guid Id { get; set; }
        int Sequence { get; set; }
    }
}
