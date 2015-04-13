namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReadonlyPermission : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChildPermissions", "Role_Id", "dbo.IdentityRoles");
            DropIndex("dbo.ChildPermissions", new[] { "Role_Id" });
            AddColumn("dbo.ChildPermissions", "ReadOnly", c => c.Boolean(nullable: false));
            DropColumn("dbo.ChildPermissions", "Role_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChildPermissions", "Role_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.ChildPermissions", "ReadOnly");
            CreateIndex("dbo.ChildPermissions", "Role_Id");
            AddForeignKey("dbo.ChildPermissions", "Role_Id", "dbo.IdentityRoles", "Id");
        }
    }
}
