namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfficeNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "OfficeNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "OfficeNumber");
        }
    }
}
