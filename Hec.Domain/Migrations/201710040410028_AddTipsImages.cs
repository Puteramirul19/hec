namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTipsImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tips", "FileId", c => c.String());
            AddColumn("dbo.Tips", "FileName", c => c.String());
            AddColumn("dbo.Tips", "FileExtension", c => c.String());
            AddColumn("dbo.Tips", "FileSize", c => c.Int());
            AddColumn("dbo.Tips", "FileThumbId", c => c.String());
            AddColumn("dbo.Tips", "FileThumbName", c => c.String());
            AddColumn("dbo.Tips", "FileThumbExtension", c => c.String());
            AddColumn("dbo.Tips", "FileThumbSize", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tips", "FileThumbSize");
            DropColumn("dbo.Tips", "FileThumbExtension");
            DropColumn("dbo.Tips", "FileThumbName");
            DropColumn("dbo.Tips", "FileThumbId");
            DropColumn("dbo.Tips", "FileSize");
            DropColumn("dbo.Tips", "FileExtension");
            DropColumn("dbo.Tips", "FileName");
            DropColumn("dbo.Tips", "FileId");
        }
    }
}
