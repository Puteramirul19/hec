namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmendAddTipsImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tips", "FilePopupId", c => c.String());
            AddColumn("dbo.Tips", "FilePopupName", c => c.String());
            AddColumn("dbo.Tips", "FilePopupExtension", c => c.String());
            AddColumn("dbo.Tips", "FilePopupSize", c => c.Int());
            DropColumn("dbo.Tips", "FileId");
            DropColumn("dbo.Tips", "FileName");
            DropColumn("dbo.Tips", "FileExtension");
            DropColumn("dbo.Tips", "FileSize");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tips", "FileSize", c => c.Int());
            AddColumn("dbo.Tips", "FileExtension", c => c.String());
            AddColumn("dbo.Tips", "FileName", c => c.String());
            AddColumn("dbo.Tips", "FileId", c => c.String());
            DropColumn("dbo.Tips", "FilePopupSize");
            DropColumn("dbo.Tips", "FilePopupExtension");
            DropColumn("dbo.Tips", "FilePopupName");
            DropColumn("dbo.Tips", "FilePopupId");
        }
    }
}
