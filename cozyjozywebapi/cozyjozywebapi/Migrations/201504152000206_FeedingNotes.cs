namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedingNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedings", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedings", "Notes");
        }
    }
}
