using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec
{
    public static class ExtensionMethods
    {
        public static string Dump(this object me)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(me, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
