using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Settings
{
    public class TariffSettings : ISettings
    {
        // NOTE: Tariff taken from https://www.tnb.com.my/residential/pricing-tariffs/
        // Tariff A - Domestic Tariff
        // The minimum monthly charge is RM3.00

        /// <summary>
        /// For the first 200 kWh(1 - 200 kWh) per month   sen/kWh	21.80
        /// </summary>
        public decimal Tier1 { get; set; }

        /// <summary>
        /// For the next 100 kWh(201 - 300 kWh) per month  sen/kWh	33.40
        /// </summary>
        public decimal Tier2 { get; set; }

        /// <summary>
        /// For the next 300 kWh(301 - 600 kWh) per month  sen/kWh	51.60
        /// </summary>
        public decimal Tier3 { get; set; }

        /// <summary>
        /// For the next 300 kWh(601 - 900 kWh) per month  sen/kWh	54.60
        /// </summary>
        public decimal Tier4 { get; set; }

        /// <summary>
        /// For the next kWh(901 kWh onwards) per month    sen/kWh	57.10
        /// </summary>
        public decimal Tier5 { get; set; }

        public TariffSettings()
        {
            this.Tier1 = 0.218m;
            this.Tier2 = 0.334m;
            this.Tier3 = 0.516m;
            this.Tier4 = 0.546m;
            this.Tier5 = 0.571m;
        }
    }
}
