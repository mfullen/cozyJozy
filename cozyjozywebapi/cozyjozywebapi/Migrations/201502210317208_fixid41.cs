namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixid41 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedings", "Id", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedings", "Id", c => c.Int(nullable: false));
        }
    }
}
