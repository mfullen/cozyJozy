namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SleepTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SleepSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Notes = c.String(),
                        UserId = c.String(maxLength: 128),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ChildId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SleepSessions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SleepSessions", "ChildId", "dbo.Children");
            DropIndex("dbo.SleepSessions", new[] { "UserId" });
            DropIndex("dbo.SleepSessions", new[] { "ChildId" });
            DropTable("dbo.SleepSessions");
        }
    }
}
