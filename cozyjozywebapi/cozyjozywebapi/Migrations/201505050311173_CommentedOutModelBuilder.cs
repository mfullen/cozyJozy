namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentedOutModelBuilder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IdentityUserLogins", "User_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserClaims", "User_Id", "dbo.IdentityUsers");
            DropIndex("dbo.IdentityUserLogins", new[] { "User_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "User_Id" });
            DropPrimaryKey("dbo.IdentityUserLogins");
            DropPrimaryKey("dbo.IdentityUserRoles");

            DropColumn("dbo.IdentityUserLogins", "UserId");

            RenameTable(name: "dbo.IdentityUserClaims", newName: "AspNetUserClaims");
            RenameTable(name: "dbo.IdentityUsers", newName: "AspNetUsers");
            RenameTable(name: "dbo.IdentityUserLogins", newName: "AspNetUserLogins");
            RenameTable(name: "dbo.IdentityUserRoles", newName: "AspNetUserRoles");
            RenameTable(name: "dbo.IdentityRoles", newName: "AspNetRoles");
           
          
            RenameColumn(table: "dbo.AspNetUserLogins", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.AspNetUserClaims", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "LoginProvider", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "ProviderKey", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String(nullable: false));
           
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "UserId", "LoginProvider", "ProviderKey" });
          
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "User_Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropPrimaryKey("dbo.AspNetUserRoles");
            AddPrimaryKey("dbo.IdentityUserRoles", new[] { "RoleId", "UserId" });
            DropPrimaryKey("dbo.AspNetUserLogins");
            AddPrimaryKey("dbo.IdentityUserLogins", "UserId");
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String());
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "ProviderKey", c => c.String());
            AlterColumn("dbo.AspNetUserLogins", "LoginProvider", c => c.String());
            AlterColumn("dbo.AspNetUserClaims", "User_Id", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.AspNetUserLogins", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.IdentityUserClaims", "User_Id");
            CreateIndex("dbo.IdentityUserLogins", "User_Id");
            AddForeignKey("dbo.IdentityUserClaims", "User_Id", "dbo.IdentityUsers", "Id");
            AddForeignKey("dbo.IdentityUserLogins", "User_Id", "dbo.IdentityUsers", "Id");
            RenameTable(name: "dbo.AspNetRoles", newName: "IdentityRoles");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "IdentityUserRoles");
            RenameTable(name: "dbo.AspNetUserLogins", newName: "IdentityUserLogins");
            RenameTable(name: "dbo.AspNetUsers", newName: "IdentityUsers");
            RenameTable(name: "dbo.AspNetUserClaims", newName: "IdentityUserClaims");
        }
    }
}
