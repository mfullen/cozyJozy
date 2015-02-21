namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleCP : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IdentityRoles", "ChildPermissions_Id", "dbo.ChildPermissions");
            DropIndex("dbo.IdentityRoles", new[] { "ChildPermissions_Id" });
            AddColumn("dbo.ChildPermissions", "IdentityRoleId", c => c.Int(nullable: false));
            AddColumn("dbo.ChildPermissions", "Role_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ChildPermissions", "Role_Id");
            AddForeignKey("dbo.ChildPermissions", "Role_Id", "dbo.IdentityRoles", "Id");
            DropColumn("dbo.IdentityRoles", "ChildPermissions_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IdentityRoles", "ChildPermissions_Id", c => c.Int());
            DropForeignKey("dbo.ChildPermissions", "Role_Id", "dbo.IdentityRoles");
            DropIndex("dbo.ChildPermissions", new[] { "Role_Id" });
            DropColumn("dbo.ChildPermissions", "Role_Id");
            DropColumn("dbo.ChildPermissions", "IdentityRoleId");
            CreateIndex("dbo.IdentityRoles", "ChildPermissions_Id");
            AddForeignKey("dbo.IdentityRoles", "ChildPermissions_Id", "dbo.ChildPermissions", "Id");
        }
    }
}
