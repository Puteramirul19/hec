namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombineChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Bills", new[] { "ContractAccount_Id" });
            RenameColumn(table: "dbo.Bills", name: "ContractAccount_Id", newName: "ContractAccountId");
            CreateTable(
                "dbo.HouseTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HouseTypeCode = c.String(),
                        HouseTypeName = c.String(),
                        PremiseType = c.String(),
                        PremiseCode = c.String(),
                        Average = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        FileId = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                        FileSize = c.Int(),
                        HouseCategoryId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HouseCategories", t => t.HouseCategoryId)
                .Index(t => t.HouseCategoryId);
            
            CreateTable(
                "dbo.HouseCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HouseCategoryName = c.String(),
                        HouseCategoryDesc = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InviterId = c.String(maxLength: 128),
                        InviteeId = c.String(maxLength: 128),
                        IsAccepted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InviteeId)
                .ForeignKey("dbo.AspNetUsers", t => t.InviterId)
                .Index(t => t.InviterId)
                .Index(t => t.InviteeId);
            
            CreateTable(
                "dbo.Tariffs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        TariffPerKWh = c.Double(nullable: false),
                        BoundryTier = c.Int(nullable: false),
                        CummulativeKWh = c.Int(nullable: false),
                        Sequence = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ContractAccounts", "IsDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContractAccounts", "LatestBillMonthYear", c => c.Int(nullable: false));
            AddColumn("dbo.ContractAccounts", "HouseType_Id", c => c.Guid());
            AddColumn("dbo.Bills", "PrintDocument", c => c.String());
            AddColumn("dbo.Bills", "PostDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Bills", "MonthYear", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "BaseConsumption", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Bills", "PrintDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Bills", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Bills", "Consumption", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Bills", "ContractAccountId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Bills", "ContractAccountId");
            CreateIndex("dbo.ContractAccounts", "HouseType_Id");
            AddForeignKey("dbo.ContractAccounts", "HouseType_Id", "dbo.HouseTypes", "Id");
            DropColumn("dbo.Bills", "AccountNo");
            DropColumn("dbo.Bills", "PrintDocumentNo");
            DropColumn("dbo.Bills", "PostingDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "PostingDate", c => c.String());
            AddColumn("dbo.Bills", "PrintDocumentNo", c => c.String());
            AddColumn("dbo.Bills", "AccountNo", c => c.String());
            DropForeignKey("dbo.Friendships", "InviterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "InviteeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContractAccounts", "HouseType_Id", "dbo.HouseTypes");
            DropForeignKey("dbo.HouseTypes", "HouseCategoryId", "dbo.HouseCategories");
            DropIndex("dbo.Friendships", new[] { "InviteeId" });
            DropIndex("dbo.Friendships", new[] { "InviterId" });
            DropIndex("dbo.HouseTypes", new[] { "HouseCategoryId" });
            DropIndex("dbo.ContractAccounts", new[] { "HouseType_Id" });
            DropIndex("dbo.Bills", new[] { "ContractAccountId" });
            AlterColumn("dbo.Bills", "ContractAccountId", c => c.Guid());
            AlterColumn("dbo.Bills", "Consumption", c => c.String());
            AlterColumn("dbo.Bills", "Amount", c => c.String());
            AlterColumn("dbo.Bills", "PrintDate", c => c.String());
            DropColumn("dbo.AspNetUsers", "BaseConsumption");
            DropColumn("dbo.Bills", "MonthYear");
            DropColumn("dbo.Bills", "PostDate");
            DropColumn("dbo.Bills", "PrintDocument");
            DropColumn("dbo.ContractAccounts", "HouseType_Id");
            DropColumn("dbo.ContractAccounts", "LatestBillMonthYear");
            DropColumn("dbo.ContractAccounts", "IsDefault");
            DropTable("dbo.Tariffs");
            DropTable("dbo.Friendships");
            DropTable("dbo.HouseCategories");
            DropTable("dbo.HouseTypes");
            RenameColumn(table: "dbo.Bills", name: "ContractAccountId", newName: "ContractAccount_Id");
            CreateIndex("dbo.Bills", "ContractAccount_Id");
        }
    }
}
