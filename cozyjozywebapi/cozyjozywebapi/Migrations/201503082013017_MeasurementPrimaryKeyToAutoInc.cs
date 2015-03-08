namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MeasurementPrimaryKeyToAutoInc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Measurements", "Id", c => c.Int(nullable: false, identity: true));
        }

        public override void Down()
        {
        }
    }
}
