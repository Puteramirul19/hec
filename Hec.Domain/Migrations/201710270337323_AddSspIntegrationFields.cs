namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSspIntegrationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SspUserId", c => c.String());
            AddColumn("dbo.AspNetUsers", "SspUserToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SspUserToken");
            DropColumn("dbo.AspNetUsers", "SspUserId");
        }
    }
}
