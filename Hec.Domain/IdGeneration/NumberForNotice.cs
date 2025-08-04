using Hec.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.IdGeneration
{
    public  class NumberForNotice : INumberSpecification
    {
        private string prefix;

        public NumberForNotice(string funcLocation)
        {
            if (String.IsNullOrEmpty(funcLocation))
                throw new Exception("Cannot create reference number from empty functional location.");

            this.prefix = (funcLocation.Length < 4) ? funcLocation : funcLocation.Substring(0, 4);
        }

        public string GetKey()
        {
            return "NID"; // Running number scope is global.
        }

        public string Format(int number)
        {
            return prefix + "-" + number.ToString("0000000");
        }
    }
}
