namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActiveForTips : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tips", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tips", "IsActive");
        }
    }
}
