namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeasurementDateRecorded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Measurements", "DateRecorded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Measurements", "DateRecorded");
        }
    }
}
