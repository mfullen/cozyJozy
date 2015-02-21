namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixid42 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedings", "Id", "dbo.Children");
            DropIndex("dbo.Feedings", new[] { "Id" });
            CreateIndex("dbo.Feedings", "ChildId");
            AddForeignKey("dbo.Feedings", "ChildId", "dbo.Children", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedings", "ChildId", "dbo.Children");
            DropIndex("dbo.Feedings", new[] { "ChildId" });
            CreateIndex("dbo.Feedings", "Id");
            AddForeignKey("dbo.Feedings", "Id", "dbo.Children", "Id");
        }
    }
}
