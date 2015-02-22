namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixid43 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiaperChanges", "Id", "dbo.Children");
            DropIndex("dbo.DiaperChanges", new[] { "Id" });
            AlterColumn("dbo.DiaperChanges", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.DiaperChanges", "ChildId");
            AddForeignKey("dbo.DiaperChanges", "ChildId", "dbo.Children", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DiaperChanges", "ChildId", "dbo.Children");
            DropIndex("dbo.DiaperChanges", new[] { "ChildId" });
            AlterColumn("dbo.DiaperChanges", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DiaperChanges", "Id");
            AddForeignKey("dbo.DiaperChanges", "Id", "dbo.Children", "Id");
        }
    }
}
