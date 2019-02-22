namespace PersonalFinances.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingProjectEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        Budget = c.Double(nullable: true),
                        Description = c.String(maxLength: 100),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Movements", "ProjectId", c => c.Int());
            CreateIndex("dbo.Movements", "ProjectId");
            AddForeignKey("dbo.Movements", "ProjectId", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movements", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Movements", new[] { "ProjectId" });
            DropColumn("dbo.Movements", "ProjectId");
            DropTable("dbo.Projects");
        }
    }
}
