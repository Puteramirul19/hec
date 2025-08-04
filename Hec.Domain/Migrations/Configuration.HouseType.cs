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

        private void SeedHouseType(HecContext db)
        {
            db.HouseCategories.AddOrUpdate(
                x => x.Id,
                new HouseCategory { Id = new Guid("D79F114C-7705-44AC-AFA7-C01A20822AC2"), HouseCategoryName = "Terrace", HouseCategoryDesc = "Terrace", Sequence = 1 },
                new HouseCategory { Id = new Guid("8FAD2F83-1715-4263-8337-792D0BDE837F"), HouseCategoryName = "HighRiseResidential", HouseCategoryDesc = "High-Rise Residential", Sequence = 2 },
                new HouseCategory { Id = new Guid("A65B7566-F36C-4D87-80FF-872FD0D135FD"), HouseCategoryName = "SemiD", HouseCategoryDesc = "Semi D", Sequence = 3 },
                new HouseCategory { Id = new Guid("292DEAAE-BC33-43E3-9038-76DE5B1EEE8F"), HouseCategoryName = "Bungalow", HouseCategoryDesc = "Bungalow", Sequence = 4 },
                new HouseCategory { Id = new Guid("B72E7E2A-49C3-4D25-AB79-54B034A940F2"), HouseCategoryName = "Kampung", HouseCategoryDesc = "Kampung", Sequence = 5 }

            );

            db.SaveChanges();

            //db.HouseTypes.AddOrUpdate(
            //    x => x.Id,
            //    // Terrace
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612345"), HouseTypeCode = "Terrace_1", HouseTypeName = "One Floor", PremiseCode = "DOM03", PremiseType = "Link house (1 storey)", Average = "349", HouseCategoryId = new Guid("D79F114C-7705-44AC-AFA7-C01A20822AC2"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612346"), HouseTypeCode = "Terrace_2", HouseTypeName = "Two Floors", PremiseCode = "DOM04", PremiseType = "Link house (> 1 storey)", Average = "396", HouseCategoryId = new Guid("D79F114C-7705-44AC-AFA7-C01A20822AC2"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612347"), HouseTypeCode = "Terrace_N", HouseTypeName = "More than Two Floors", PremiseCode = "DOM04", PremiseType = "Link house (> 1 storey)", Average = "396", HouseCategoryId = new Guid("D79F114C-7705-44AC-AFA7-C01A20822AC2"), IsActive = true },

            //    // High-Residence
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612348"), HouseTypeCode = "Apartment_N", HouseTypeName = "Apartment", PremiseCode = "DOM12", PremiseType = "Low-cost Apt ( >3 rooms)", Average = "330", HouseCategoryId = new Guid("8FAD2F83-1715-4263-8337-792D0BDE837F"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612349"), HouseTypeCode = "Condominium_N", HouseTypeName = "Condominium", PremiseCode = "DOM14", PremiseType = "Condominium", Average = "334", HouseCategoryId = new Guid("8FAD2F83-1715-4263-8337-792D0BDE837F"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612350"), HouseTypeCode = "Flat_N", HouseTypeName = "Flat", PremiseCode = "DOM10", PremiseType = "Flat", Average = "209", HouseCategoryId = new Guid("8FAD2F83-1715-4263-8337-792D0BDE837F"), IsActive = true },

            //    // Bungalow
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612351"), HouseTypeCode = "Bungalow_1", HouseTypeName = "One Floor", PremiseCode = "DOM17", PremiseType = "Bungalow (1 storey)", Average = "669", HouseCategoryId = new Guid("292DEAAE-BC33-43E3-9038-76DE5B1EEE8F"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612352"), HouseTypeCode = "Bungalow_2", HouseTypeName = "Two Floors", PremiseCode = "DOM18", PremiseType = "Bungalow (> 1 storey)", Average = "956", HouseCategoryId = new Guid("292DEAAE-BC33-43E3-9038-76DE5B1EEE8F"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612353"), HouseTypeCode = "Bungalow_3", HouseTypeName = "More than Two Floors", PremiseCode = "DOM18", PremiseType = "Bungalow (> 1 storey)", Average = "956", HouseCategoryId = new Guid("292DEAAE-BC33-43E3-9038-76DE5B1EEE8F"), IsActive = true },

            //    // Semi D
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612354"), HouseTypeCode = "SemiD_1", HouseTypeName = "One Floor", PremiseCode = "DOM01", PremiseType = "Semi-D(Incl.Cluster -1storey)", Average = "364", HouseCategoryId = new Guid("A65B7566-F36C-4D87-80FF-872FD0D135FD"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612355"), HouseTypeCode = "SemiD_2", HouseTypeName = "Two Floors", PremiseCode = "DOM02", PremiseType = "Semi-D(Incl.Cluster >1storey)", Average = "552", HouseCategoryId = new Guid("A65B7566-F36C-4D87-80FF-872FD0D135FD"), IsActive = true },
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612356"), HouseTypeCode = "SemiD_N", HouseTypeName = "More than Two Floors", PremiseCode = "DOM02", PremiseType = "Semi-D(Incl.Cluster >1storey)", Average = "552", HouseCategoryId = new Guid("A65B7566-F36C-4D87-80FF-872FD0D135FD"), IsActive = true },

            //    // Kampung
            //    new HouseType { Id = new Guid("32D89576-54BE-4CA6-91C4-89B296612357"), HouseTypeCode = "Kampung_1", HouseTypeName = "One Floor", PremiseCode = "DOM19", PremiseType = "Kampung House", Average = "319", HouseCategoryId = new Guid("B72E7E2A-49C3-4D25-AB79-54B034A940F2"), IsActive = true }
            //);

            //db.SaveChanges();

        }
    }
}
