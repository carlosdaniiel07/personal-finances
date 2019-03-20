namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovementAndTransferAdjusts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movements", "AutomaticallyLaunch", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transfers", "AutomaticallyLaunch", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transfers", "AutomaticallyLaunch");
            DropColumn("dbo.Movements", "AutomaticallyLaunch");
        }
    }
}
