namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleAssociation2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChildPermissions", "TitleId", "dbo.Titles");
            DropIndex("dbo.ChildPermissions", new[] { "TitleId" });
            AlterColumn("dbo.ChildPermissions", "TitleId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChildPermissions", "TitleId");
            AddForeignKey("dbo.ChildPermissions", "TitleId", "dbo.Titles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChildPermissions", "TitleId", "dbo.Titles");
            DropIndex("dbo.ChildPermissions", new[] { "TitleId" });
            AlterColumn("dbo.ChildPermissions", "TitleId", c => c.Int());
            CreateIndex("dbo.ChildPermissions", "TitleId");
            AddForeignKey("dbo.ChildPermissions", "TitleId", "dbo.Titles", "Id");
        }
    }
}
