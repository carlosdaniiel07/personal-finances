namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCardAndInvoiceAdjusts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movements", "CanEdit", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categories", "CanEdit", c => c.Boolean(nullable: false));
            AddColumn("dbo.Subcategories", "CanEdit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Invoices", "Reference", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoices", "Reference", c => c.String(nullable: false, maxLength: 8));
            DropColumn("dbo.Subcategories", "CanEdit");
            DropColumn("dbo.Categories", "CanEdit");
            DropColumn("dbo.Movements", "CanEdit");
        }
    }
}
