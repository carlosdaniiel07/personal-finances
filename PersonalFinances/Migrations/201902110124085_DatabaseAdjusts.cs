namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseAdjusts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "FinishDate", c => c.DateTime());
            AlterColumn("dbo.Projects", "Budget", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Budget", c => c.Double(nullable: false));
            AlterColumn("dbo.Projects", "FinishDate", c => c.DateTime(nullable: false));
        }
    }
}
