namespace Hec.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appliances",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        DefaultNumbersOfApp = c.Int(nullable: false),
                        DefaultHoursPerDay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DefaultDaysPerMonth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DefaultWatts = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForLivingRoom = c.Boolean(nullable: false),
                        ForMasterBedroom = c.Boolean(nullable: false),
                        ForBedroom = c.Boolean(nullable: false),
                        ForBathroom = c.Boolean(nullable: false),
                        ForDiningRoom = c.Boolean(nullable: false),
                        ForKitchen = c.Boolean(nullable: false),
                        TipCategoryId = c.Guid(nullable: false),
                        FileId = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                        FileSize = c.Int(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipCategories", t => t.TipCategoryId)
                .Index(t => t.TipCategoryId);
            
            CreateTable(
                "dbo.TipCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        FileId = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                        FileSize = c.Int(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContractAccounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountNo = c.String(maxLength: 15),
                        UserId = c.String(maxLength: 128),
                        IsFromSsp = c.Boolean(nullable: false),
                        AccountStatus = c.String(),
                        Name = c.String(),
                        RateCategory = c.String(),
                        RateCategoryDesc = c.String(),
                        DisconnectionStatus = c.String(),
                        PremiseType = c.String(),
                        Email = c.String(),
                        MobileNo = c.String(),
                        TelephoneNo = c.String(),
                        LatestBillAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HouseData = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountNo = c.String(),
                        PrintDocumentNo = c.String(),
                        PrintDate = c.String(),
                        PostingDate = c.String(),
                        Amount = c.String(),
                        Consumption = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ContractAccount_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContractAccounts", t => t.ContractAccount_Id)
                .Index(t => t.ContractAccount_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LoginType = c.Int(nullable: false),
                        LastLogin = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FullName = c.String(),
                        Nric = c.String(),
                        Photo = c.String(),
                        Designation = c.String(),
                        Department = c.String(),
                        Language = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RoleId = c.Guid(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        SerializedAccessRights = c.String(),
                        NeedZone = c.Boolean(nullable: false),
                        NeedSubZone = c.Boolean(nullable: false),
                        NeedUnit = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.EmailQueues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(),
                        ToAddress = c.String(),
                        ToName = c.String(),
                        Content = c.String(),
                        Attachments = c.String(),
                        ScheduledOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ProcessedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        SentOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ObjectId = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        SubjectTemplate = c.String(),
                        ContentTemplate = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HelpFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        FileId = c.String(),
                        Sequence = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KeyValues",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.OffDays",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Year = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Name = c.String(),
                        StateId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 10),
                        EhrmsCode = c.String(maxLength: 10),
                        Name = c.String(),
                        Weekend1 = c.Int(nullable: false),
                        Weekend2 = c.Int(nullable: false),
                        IntegrationId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code)
                .Index(t => t.EhrmsCode);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Tips",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        TipCategoryId = c.Guid(nullable: false),
                        DoneCount = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipCategories", t => t.TipCategoryId)
                .Index(t => t.TipCategoryId);
            
            CreateTable(
                "dbo.UserTips",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TipId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tips", t => t.TipId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TipId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTips", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTips", "TipId", "dbo.Tips");
            DropForeignKey("dbo.Tips", "TipCategoryId", "dbo.TipCategories");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OffDays", "StateId", "dbo.States");
            DropForeignKey("dbo.ContractAccounts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bills", "ContractAccount_Id", "dbo.ContractAccounts");
            DropForeignKey("dbo.Appliances", "TipCategoryId", "dbo.TipCategories");
            DropIndex("dbo.UserTips", new[] { "TipId" });
            DropIndex("dbo.UserTips", new[] { "UserId" });
            DropIndex("dbo.Tips", new[] { "TipCategoryId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.States", new[] { "EhrmsCode" });
            DropIndex("dbo.States", new[] { "Code" });
            DropIndex("dbo.OffDays", new[] { "StateId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "RoleId" });
            DropIndex("dbo.Bills", new[] { "ContractAccount_Id" });
            DropIndex("dbo.ContractAccounts", new[] { "UserId" });
            DropIndex("dbo.Appliances", new[] { "TipCategoryId" });
            DropTable("dbo.UserTips");
            DropTable("dbo.Tips");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.States");
            DropTable("dbo.OffDays");
            DropTable("dbo.KeyValues");
            DropTable("dbo.HelpFiles");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailQueues");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bills");
            DropTable("dbo.ContractAccounts");
            DropTable("dbo.TipCategories");
            DropTable("dbo.Appliances");
        }
    }
}
