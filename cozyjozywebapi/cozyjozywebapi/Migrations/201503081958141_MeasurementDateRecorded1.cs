namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeasurementDateRecorded1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Measurements", "Id", "dbo.Children");
            DropIndex("dbo.Measurements", new[] { "Id" });
            AlterColumn("dbo.Measurements", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Measurements", "ChildId");
            AddForeignKey("dbo.Measurements", "ChildId", "dbo.Children", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Measurements", "ChildId", "dbo.Children");
            DropIndex("dbo.Measurements", new[] { "ChildId" });
            AlterColumn("dbo.Measurements", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Measurements", "Id");
            AddForeignKey("dbo.Measurements", "Id", "dbo.Children", "Id");
        }
    }
}
