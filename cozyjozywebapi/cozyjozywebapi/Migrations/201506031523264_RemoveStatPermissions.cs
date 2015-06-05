namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStatPermissions : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ChildPermissions", "FeedingStatAccess");
            DropColumn("dbo.ChildPermissions", "DiaperStatAccess");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChildPermissions", "DiaperStatAccess", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChildPermissions", "FeedingStatAccess", c => c.Boolean(nullable: false));
        }
    }
}
