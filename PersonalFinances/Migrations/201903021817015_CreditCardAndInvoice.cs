namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCardAndInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(nullable: false, maxLength: 8),
                        MaturityDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(),
                        Closed = c.Boolean(nullable: false),
                        InvoiceStatus = c.Int(nullable: false),
                        CreditCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardId)
                .Index(t => t.CreditCardId);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        InvoiceClosure = c.String(nullable: false, maxLength: 2),
                        PayDay = c.String(nullable: false, maxLength: 2),
                        Limit = c.Double(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Movements", "InvoiceId", c => c.Int());
            CreateIndex("dbo.Movements", "InvoiceId");
            AddForeignKey("dbo.Movements", "InvoiceId", "dbo.Invoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movements", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "CreditCardId", "dbo.CreditCards");
            DropIndex("dbo.Invoices", new[] { "CreditCardId" });
            DropIndex("dbo.Movements", new[] { "InvoiceId" });
            DropColumn("dbo.Movements", "InvoiceId");
            DropTable("dbo.CreditCards");
            DropTable("dbo.Invoices");
        }
    }
}
