namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleCP2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ChildPermissions", "IdentityRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChildPermissions", "IdentityRoleId", c => c.Int(nullable: false));
        }
    }
}
