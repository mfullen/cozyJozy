namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trackHeadCircumference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Measurements", "HeadCircumference", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Measurements", "HeadCircumference");
        }
    }
}
