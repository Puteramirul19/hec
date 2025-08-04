namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActiveForAppliances : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appliances", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appliances", "IsActive");
        }
    }
}
