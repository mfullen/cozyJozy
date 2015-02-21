namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deliverytypespitup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedings", "DeliveryType", c => c.Int());
            AddColumn("dbo.Feedings", "SpitUp", c => c.Boolean(nullable: false));
            DropColumn("dbo.Feedings", "Breast");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedings", "Breast", c => c.Int());
            DropColumn("dbo.Feedings", "SpitUp");
            DropColumn("dbo.Feedings", "DeliveryType");
        }
    }
}
