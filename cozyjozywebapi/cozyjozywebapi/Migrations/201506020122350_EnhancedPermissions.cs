namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnhancedPermissions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChildPermissions", "FeedingWriteAccess", c => c.Boolean());
            AddColumn("dbo.ChildPermissions", "DiaperChangeWriteAccess", c => c.Boolean());
            AddColumn("dbo.ChildPermissions", "SleepWriteAccess", c => c.Boolean());
            AddColumn("dbo.ChildPermissions", "MeasurementWriteAccess", c => c.Boolean());
            AddColumn("dbo.ChildPermissions", "ChildManagementWriteAccess", c => c.Boolean());
            AddColumn("dbo.ChildPermissions", "PermissionsWriteAccess", c => c.Boolean());
            AddColumn("dbo.ChildPermissions", "FeedingStatAccess", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChildPermissions", "DiaperStatAccess", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChildPermissions", "DiaperStatAccess");
            DropColumn("dbo.ChildPermissions", "FeedingStatAccess");
            DropColumn("dbo.ChildPermissions", "PermissionsWriteAccess");
            DropColumn("dbo.ChildPermissions", "ChildManagementWriteAccess");
            DropColumn("dbo.ChildPermissions", "MeasurementWriteAccess");
            DropColumn("dbo.ChildPermissions", "SleepWriteAccess");
            DropColumn("dbo.ChildPermissions", "DiaperChangeWriteAccess");
            DropColumn("dbo.ChildPermissions", "FeedingWriteAccess");
        }
    }
}
