namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSequenceForHouse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseTypes", "Sequence", c => c.Int(nullable: false));
            AddColumn("dbo.HouseCategories", "Sequence", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HouseCategories", "Sequence");
            DropColumn("dbo.HouseTypes", "Sequence");
        }
    }
}
