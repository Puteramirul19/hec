namespace Hec.Migrations
{
    using Entities;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public partial class Configuration : DbMigrationsConfiguration<Hec.HecContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Hec.HecContext db)
        {
            //
            // Initialize running number database
            //
            var runningNumber = new Hec.IdGeneration.DbRunningNumber("HecContext");
            runningNumber.InitializeStorage();

            //
            // Initialize file store database
            //
            var dbFileStore = new Hec.FileStorage.DbFileStore("HecContext");
            dbFileStore.Initialize();

            //SeedState(db);

            //SeedOffDay(db);

            SeedUserRole(db);

            SeedEmailTemplate(db);

            //SeedTipsAndCategory(db);

            //SeedAppliance(db);

            SeedTariff(db);

            SeedHouseType(db);

            SeedSampleData(db);

            db.SaveChanges();

            RunSql(db);
        }

        public void RunSeed(Hec.HecContext db)
        {
            Seed(db);
        }
    }
}
