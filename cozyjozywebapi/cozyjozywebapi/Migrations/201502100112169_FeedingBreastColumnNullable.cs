namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedingBreastColumnNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedings", "Breast", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedings", "Breast", c => c.Int(nullable: false));
        }
    }
}
