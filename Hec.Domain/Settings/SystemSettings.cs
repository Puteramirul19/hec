using Hec.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Settings
{
    public class SystemSettings : ISettings
    {
        public int FinancialYearStartDay { get; set; }
        public int FinancialYearStartMonth { get; set; }

        public SystemSettings()
        {
            this.FinancialYearStartDay = 1;
            this.FinancialYearStartMonth = 9;
        }
    }
}
