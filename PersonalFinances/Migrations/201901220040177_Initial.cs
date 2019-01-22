namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        AccountType = c.Int(nullable: false),
                        InitialBalance = c.Double(nullable: false),
                        Balance = c.Double(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 1),
                        Description = c.String(nullable: false, maxLength: 50),
                        Value = c.Double(nullable: false),
                        Increase = c.Double(),
                        Decrease = c.Double(),
                        InclusionDate = c.DateTime(nullable: false),
                        AccountingDate = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        SubcategoryId = c.Int(nullable: false),
                        MovementStatus = c.Int(nullable: false),
                        Observation = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Subcategories", t => t.SubcategoryId)
                .Index(t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubcategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Type = c.String(nullable: false, maxLength: 1),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        CategoryId = c.Int(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        OriginId = c.Int(nullable: false),
                        TargetId = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        Tax = c.Double(),
                        InclusionDate = c.DateTime(nullable: false),
                        AccountingDate = c.DateTime(nullable: false),
                        TransferStatus = c.Int(nullable: false),
                        Observation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.OriginId)
                .ForeignKey("dbo.Accounts", t => t.TargetId)
                .Index(t => t.OriginId)
                .Index(t => t.TargetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "TargetId", "dbo.Accounts");
            DropForeignKey("dbo.Transfers", "OriginId", "dbo.Accounts");
            DropForeignKey("dbo.Movements", "SubcategoryId", "dbo.Subcategories");
            DropForeignKey("dbo.Subcategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Movements", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Movements", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Transfers", new[] { "TargetId" });
            DropIndex("dbo.Transfers", new[] { "OriginId" });
            DropIndex("dbo.Subcategories", new[] { "CategoryId" });
            DropIndex("dbo.Movements", new[] { "SubcategoryId" });
            DropIndex("dbo.Movements", new[] { "CategoryId" });
            DropIndex("dbo.Movements", new[] { "AccountId" });
            DropTable("dbo.Transfers");
            DropTable("dbo.Subcategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Movements");
            DropTable("dbo.Accounts");
        }
    }
}
