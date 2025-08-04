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
        private void SeedState(HecContext db)
        {
            db.States.AddOrUpdate(
                x => x.Id,
                new State { Id = new Guid("DF534E06-50BC-4E24-A74A-77012CFFD5DA"), Code = "SEL", EhrmsCode = "ZS", Name = "Selangor", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 6},
                new State { Id = new Guid("077632C9-1AE5-4ADF-831F-8DAB30AE01DD"), Code = "KLP", EhrmsCode = "ZW", Name = "WP Kuala Lumpur", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 10},
                new State { Id = new Guid("9E35D88F-DAEF-4E76-8706-FD195419FB64"), Code = "JOH", EhrmsCode = "ZJ", Name = "Johor", Weekend1 = DayOfWeek.Friday, Weekend2 = DayOfWeek.Saturday, IntegrationId = 12},
                new State { Id = new Guid("7AC9BE4B-58B7-481C-8693-4F521568C9A3"), Code = "KED", EhrmsCode = "ZK", Name = "Kedah", Weekend1 = DayOfWeek.Friday, Weekend2 = DayOfWeek.Saturday, IntegrationId = 3},
                new State { Id = new Guid("45BDCDA1-9133-4A39-B445-9D62988DDD09"), Code = "KEL", EhrmsCode = "ZD", Name = "Kelantan", Weekend1 = DayOfWeek.Friday, Weekend2 = DayOfWeek.Saturday, IntegrationId = 13},
                new State { Id = new Guid("09C1529A-644E-4ABE-B4D2-091B17FB97B5"), Code = "MEL", EhrmsCode = "ZM", Name = "Melaka", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 7},
                new State { Id = new Guid("A0E29010-2C65-4759-9D04-CEF5AA00CB3E"), Code = "NSB", EhrmsCode = "ZN", Name = "Negeri Sembilan", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 11},
                new State { Id = new Guid("3BA4F148-C759-4AFA-A1E8-89DC2299177D"), Code = "PAH", EhrmsCode = "ZC", Name = "Pahang", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 8},
                new State { Id = new Guid("3FFF2EFD-75CD-45C1-9025-2427427BB566"), Code = "PER", EhrmsCode = "ZA", Name = "Perak", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 5},
                new State { Id = new Guid("C480718C-DBD2-4CE6-BA0B-62F656A819F9"), Code = "PLS", EhrmsCode = "ZR", Name = "Perlis", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 2},
                new State { Id = new Guid("DF2C7520-159C-4B7D-8AFF-18212DC04CFE"), Code = "PEN", EhrmsCode = "ZP", Name = "Pulau Pinang", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 4},
                new State { Id = new Guid("8015D07C-95B0-4F18-B993-9669A21D74F6"), Code = "PUT", EhrmsCode = "ZW", Name = "Putrajaya & Cyberjaya", Weekend1 = DayOfWeek.Saturday, Weekend2 = DayOfWeek.Sunday, IntegrationId = 1},
                new State { Id = new Guid("F4174B23-76CB-489A-A0CC-B7FCD2C7E305"), Code = "TER", EhrmsCode = "ZT", Name = "Terengganu", Weekend1 = DayOfWeek.Friday, Weekend2 = DayOfWeek.Saturday, IntegrationId =9 }
            );                             

            db.SaveChanges();
        }
    }
}
