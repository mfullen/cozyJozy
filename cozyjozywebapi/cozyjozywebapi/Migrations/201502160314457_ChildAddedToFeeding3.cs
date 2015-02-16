namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChildAddedToFeeding3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedings", "ChildId", c => c.Int(nullable: false));
            DropColumn("dbo.Feedings", "ChildrenId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedings", "ChildrenId", c => c.Int(nullable: false));
            DropColumn("dbo.Feedings", "ChildId");
        }
    }
}
