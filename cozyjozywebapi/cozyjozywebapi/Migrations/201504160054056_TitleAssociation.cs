namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleAssociation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Titles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ChildPermissions", "TitleId", c => c.Int());
            CreateIndex("dbo.ChildPermissions", "TitleId");
            AddForeignKey("dbo.ChildPermissions", "TitleId", "dbo.Titles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChildPermissions", "TitleId", "dbo.Titles");
            DropIndex("dbo.ChildPermissions", new[] { "TitleId" });
            DropColumn("dbo.ChildPermissions", "TitleId");
            DropTable("dbo.Titles");
        }
    }
}
