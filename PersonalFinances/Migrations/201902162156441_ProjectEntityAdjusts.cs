namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectEntityAdjusts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectStatus");
        }
    }
}
