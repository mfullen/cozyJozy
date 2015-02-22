namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class toazure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOfBirth = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        Male = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiaperChanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OccurredOn = c.DateTime(nullable: false),
                        Notes = c.String(),
                        Urine = c.Boolean(nullable: false),
                        Stool = c.Boolean(nullable: false),
                        ChildId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.UserId)
                .Index(t => t.ChildId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.IdentityUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Height = c.Double(),
                        Weight = c.Double(),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ChildPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChildId = c.Int(nullable: false),
                        IdentityUserId = c.String(maxLength: 128),
                        Role_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUserId)
                .ForeignKey("dbo.IdentityRoles", t => t.Role_Id)
                .Index(t => t.ChildId)
                .Index(t => t.IdentityUserId)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Feedings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        DeliveryType = c.Int(),
                        Amount = c.Double(),
                        DateReported = c.DateTime(nullable: false),
                        SpitUp = c.Boolean(nullable: false),
                        ChildId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.UserId)
                .Index(t => t.ChildId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserChildren",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ChildId })
                .ForeignKey("dbo.IdentityUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedings", "UserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.Feedings", "ChildId", "dbo.Children");
            DropForeignKey("dbo.ChildPermissions", "Role_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.ChildPermissions", "IdentityUserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.ChildPermissions", "ChildId", "dbo.Children");
            DropForeignKey("dbo.Measurements", "Id", "dbo.Children");
            DropForeignKey("dbo.DiaperChanges", "UserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserRoles", "RoleId", "dbo.IdentityRoles");
            DropForeignKey("dbo.IdentityUserLogins", "User_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserClaims", "User_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.UserChildren", "ChildId", "dbo.Children");
            DropForeignKey("dbo.UserChildren", "UserId", "dbo.IdentityUsers");
            DropForeignKey("dbo.DiaperChanges", "ChildId", "dbo.Children");
            DropIndex("dbo.Feedings", new[] { "UserId" });
            DropIndex("dbo.Feedings", new[] { "ChildId" });
            DropIndex("dbo.ChildPermissions", new[] { "Role_Id" });
            DropIndex("dbo.ChildPermissions", new[] { "IdentityUserId" });
            DropIndex("dbo.ChildPermissions", new[] { "ChildId" });
            DropIndex("dbo.Measurements", new[] { "Id" });
            DropIndex("dbo.DiaperChanges", new[] { "UserId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "UserId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "RoleId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "User_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "User_Id" });
            DropIndex("dbo.UserChildren", new[] { "ChildId" });
            DropIndex("dbo.UserChildren", new[] { "UserId" });
            DropIndex("dbo.DiaperChanges", new[] { "ChildId" });
            DropTable("dbo.UserChildren");
            DropTable("dbo.Feedings");
            DropTable("dbo.ChildPermissions");
            DropTable("dbo.Measurements");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.IdentityUsers");
            DropTable("dbo.DiaperChanges");
            DropTable("dbo.Children");
        }
    }
}
