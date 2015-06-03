namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveReadOnlyPermission : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ChildPermissions", "ReadOnly");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChildPermissions", "ReadOnly", c => c.Boolean(nullable: false));
        }
    }
}
