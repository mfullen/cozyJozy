namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChildForeginKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiaperChanges", "Child_Id", "dbo.Children");
            DropForeignKey("dbo.Measurements", "Child_Id", "dbo.Children");
            DropIndex("dbo.DiaperChanges", new[] { "Child_Id" });
            DropIndex("dbo.Measurements", new[] { "Child_Id" });
            AddColumn("dbo.DiaperChanges", "ChildId", c => c.Int(nullable: false));
            AddColumn("dbo.Measurements", "ChildId", c => c.Int(nullable: false));
            AlterColumn("dbo.DiaperChanges", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Measurements", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DiaperChanges", "Id");
            CreateIndex("dbo.Measurements", "Id");
            AddForeignKey("dbo.DiaperChanges", "Id", "dbo.Children", "Id");
            AddForeignKey("dbo.Measurements", "Id", "dbo.Children", "Id");
            DropColumn("dbo.DiaperChanges", "Child_Id");
            DropColumn("dbo.Measurements", "Child_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Measurements", "Child_Id", c => c.Int());
            AddColumn("dbo.DiaperChanges", "Child_Id", c => c.Int());
            DropForeignKey("dbo.Measurements", "Id", "dbo.Children");
            DropForeignKey("dbo.DiaperChanges", "Id", "dbo.Children");
            DropIndex("dbo.Measurements", new[] { "Id" });
            DropIndex("dbo.DiaperChanges", new[] { "Id" });
            AlterColumn("dbo.Measurements", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DiaperChanges", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Measurements", "ChildId");
            DropColumn("dbo.DiaperChanges", "ChildId");
            CreateIndex("dbo.Measurements", "Child_Id");
            CreateIndex("dbo.DiaperChanges", "Child_Id");
            AddForeignKey("dbo.Measurements", "Child_Id", "dbo.Children", "Id");
            AddForeignKey("dbo.DiaperChanges", "Child_Id", "dbo.Children", "Id");
        }
    }
}
