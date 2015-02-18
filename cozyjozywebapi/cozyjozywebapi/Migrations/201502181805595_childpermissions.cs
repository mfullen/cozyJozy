namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class childpermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChildPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChildId = c.Int(nullable: false),
                        IdentityUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUserId)
                .Index(t => t.ChildId)
                .Index(t => t.IdentityUserId);
            
            AddColumn("dbo.IdentityRoles", "ChildPermissions_Id", c => c.Int());
            CreateIndex("dbo.IdentityRoles", "ChildPermissions_Id");
            AddForeignKey("dbo.IdentityRoles", "ChildPermissions_Id", "dbo.ChildPermissions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityRoles", "ChildPermissions_Id", "dbo.ChildPermissions");
            DropForeignKey("dbo.ChildPermissions", "IdentityUserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.ChildPermissions", "ChildId", "dbo.Children");
            DropIndex("dbo.IdentityRoles", new[] { "ChildPermissions_Id" });
            DropIndex("dbo.ChildPermissions", new[] { "IdentityUserId" });
            DropIndex("dbo.ChildPermissions", new[] { "ChildId" });
            DropColumn("dbo.IdentityRoles", "ChildPermissions_Id");
            DropTable("dbo.ChildPermissions");
        }
    }
}
