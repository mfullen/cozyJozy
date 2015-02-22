namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trackReportedByUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiaperChanges", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Feedings", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DiaperChanges", "UserId");
            CreateIndex("dbo.Feedings", "UserId");
            AddForeignKey("dbo.DiaperChanges", "UserId", "dbo.IdentityUsers", "Id");
            AddForeignKey("dbo.Feedings", "UserId", "dbo.IdentityUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedings", "UserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.DiaperChanges", "UserId", "dbo.IdentityUsers");
            DropIndex("dbo.Feedings", new[] { "UserId" });
            DropIndex("dbo.DiaperChanges", new[] { "UserId" });
            DropColumn("dbo.Feedings", "UserId");
            DropColumn("dbo.DiaperChanges", "UserId");
        }
    }
}
