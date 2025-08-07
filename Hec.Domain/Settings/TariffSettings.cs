using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Settings
{
    public class TariffSettings : ISettings
    {
        // NOTE: OLD Tariff taken from https://www.tnb.com.my/residential/pricing-tariffs/
        // OLD Tariff A - Domestic Tariff - SUPERSEDED BY NEW STRUCTURE JULY 2025
        // The minimum monthly charge was RM3.00

        /// <summary>
        /// OLD: For the first 200 kWh(1 - 200 kWh) per month   sen/kWh	21.80 - SUPERSEDED JULY 2025
        /// </summary>
        public decimal Tier1 { get; set; }

        /// <summary>
        /// OLD: For the next 100 kWh(201 - 300 kWh) per month  sen/kWh	33.40 - SUPERSEDED JULY 2025
        /// </summary>
        public decimal Tier2 { get; set; }

        /// <summary>
        /// OLD: For the next 300 kWh(301 - 600 kWh) per month  sen/kWh	51.60 - SUPERSEDED JULY 2025
        /// </summary>
        public decimal Tier3 { get; set; }

        /// <summary>
        /// OLD: For the next 300 kWh(601 - 900 kWh) per month  sen/kWh	54.60 - SUPERSEDED JULY 2025
        /// </summary>
        public decimal Tier4 { get; set; }

        /// <summary>
        /// OLD: For the next kWh(901 kWh onwards) per month    sen/kWh	57.10 - SUPERSEDED JULY 2025
        /// </summary>
        public decimal Tier5 { get; set; }

        // NEW TARIFF STRUCTURE EFFECTIVE JULY 1, 2025
        // Based on component charges instead of tier system

        /// <summary>
        /// NEW: Generation charge for usage under 1,500 kWh per month - 27.03 sen/kWh
        /// </summary>
        public decimal GenerationChargeUnder1500 { get; set; }

        /// <summary>
        /// NEW: Generation charge for usage over 1,500 kWh per month - 37.03 sen/kWh
        /// </summary>
        public decimal GenerationChargeOver1500 { get; set; }

        /// <summary>
        /// NEW: Capacity charge - 4.55 sen/kWh (for all usage)
        /// </summary>
        public decimal CapacityCharge { get; set; }

        /// <summary>
        /// NEW: Network charge - 12.85 sen/kWh (for all usage)
        /// </summary>
        public decimal NetworkCharge { get; set; }

        /// <summary>
        /// NEW: Retail charge - RM10 per month (exempted for usage under 600 kWh)
        /// </summary>
        public decimal RetailCharge { get; set; }

        /// <summary>
        /// NEW: Threshold for retail charge exemption - 600 kWh
        /// </summary>
        public decimal RetailChargeExemptionThreshold { get; set; }

        /// <summary>
        /// NEW: Threshold for different generation charge - 1,500 kWh
        /// </summary>
        public decimal GenerationChargeThreshold { get; set; }

        /// <summary>
        /// NEW: Energy Efficiency Incentive threshold - 1,000 kWh
        /// </summary>
        public decimal EnergyEfficiencyThreshold { get; set; }

        /// <summary>
        /// NEW: Maximum Energy Efficiency Incentive - 25 sen/kWh
        /// </summary>
        public decimal MaxEnergyEfficiencyIncentive { get; set; }

        /// <summary>
        /// NEW: Average cost per kWh for simplified calculations (Generation + Capacity + Network for under 1500kWh)
        /// 27.03 + 4.55 + 12.85 = 44.43 sen/kWh
        /// </summary>
        public decimal AverageCostPerKwhUnder1500 { get; set; }

        /// <summary>
        /// NEW: Average cost per kWh for over 1500kWh usage
        /// 37.03 + 4.55 + 12.85 = 54.43 sen/kWh
        /// </summary>
        public decimal AverageCostPerKwhOver1500 { get; set; }

        public TariffSettings()
        {
            // OLD TIER SYSTEM - KEPT FOR BACKWARD COMPATIBILITY
            this.Tier1 = 0.218m;
            this.Tier2 = 0.334m;
            this.Tier3 = 0.516m;
            this.Tier4 = 0.546m;
            this.Tier5 = 0.571m;

            // NEW TARIFF STRUCTURE EFFECTIVE JULY 1, 2025
            this.GenerationChargeUnder1500 = 0.2703m;
            this.GenerationChargeOver1500 = 0.3703m;
            this.CapacityCharge = 0.0455m;
            this.NetworkCharge = 0.1285m;
            this.RetailCharge = 10.00m;
            this.RetailChargeExemptionThreshold = 600m;
            this.GenerationChargeThreshold = 1500m;
            this.EnergyEfficiencyThreshold = 1000m;
            this.MaxEnergyEfficiencyIncentive = 0.25m;
            this.AverageCostPerKwhUnder1500 = 0.4443m; // 44.43 sen/kWh
            this.AverageCostPerKwhOver1500 = 0.5443m;  // 54.43 sen/kWh
        }
    }
}
