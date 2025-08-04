namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHeaderHouseImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HouseTypes", "FileHeaderId", c => c.String());
            AddColumn("dbo.HouseTypes", "FileHeaderName", c => c.String());
            AddColumn("dbo.HouseTypes", "FileHeaderExtension", c => c.String());
            AddColumn("dbo.HouseTypes", "FileHeaderSize", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HouseTypes", "FileHeaderSize");
            DropColumn("dbo.HouseTypes", "FileHeaderExtension");
            DropColumn("dbo.HouseTypes", "FileHeaderName");
            DropColumn("dbo.HouseTypes", "FileHeaderId");
        }
    }
}
