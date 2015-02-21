namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixid3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedings", "Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedings", "Id", c => c.Int(nullable: false, identity: true));
        }
    }
}
