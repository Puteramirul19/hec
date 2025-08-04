using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec
{
    public class IdNotFoundException<T> : Exception
    {
        public IdNotFoundException(object id) : base("No '" + typeof(T) + "' found with ID='" + id + "'.")
        {
        }
    }
}
