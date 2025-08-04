
using Hec.Entities;
using Hec.IdGeneration;
using Hec.Workflows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Hec.Migrations
{
    public partial class Configuration
    {

        private void SeedTariff(HecContext db)
        {
            // From file Appliances - New Default Values - v2.xlsx
            db.Tariffs.AddOrUpdate(
                x => x.Id,
                new Tariff
                {
                    Description = "1 - 200 kWh",
                    TariffPerKWh = 0.218,
                    BoundryTier = 200,
                    CummulativeKWh = 200,
                    Sequence = 1
                },
                new Tariff
                {
                    Description = "201 - 300 kWh",
                    TariffPerKWh = 0.334,
                    BoundryTier = 100,
                    CummulativeKWh = 300,
                    Sequence = 2
                },
                new Tariff
                {
                    Description = "301 - 600 kWh",
                    TariffPerKWh = 0.516,
                    BoundryTier = 300,
                    CummulativeKWh = 600,
                    Sequence = 3
                },
                new Tariff
                {
                    Description = "601 - 900 kWh",
                    TariffPerKWh = 0.546,
                    BoundryTier = 300,
                    CummulativeKWh = 900,
                    Sequence = 4
                },
                new Tariff
                {
                    Description = "901 kWh onwards",
                    TariffPerKWh = 0.571,
                    BoundryTier = 0,
                    CummulativeKWh = 0,
                    Sequence = 5
                }
            );

            db.SaveChanges();
        }
    }
}
