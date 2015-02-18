using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Entity
{
    public class CozyJozyContext   : IdentityDbContext<CjUser>
    {
        public CozyJozyContext()
            : base("CozyJozy")
        {
        }
        public DbSet<Feedings> Feedings { get; set; }
        public DbSet<Child> Child { get; set; }
        public DbSet<DiaperChanges> DiaperChanges { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<ChildPermissions> ChildPermissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<CjUser>()
                .HasMany(x => x.Child)
                .WithMany(x => x.Followers)
            .Map(x =>
            {
                x.ToTable("UserChildren");
                x.MapLeftKey("UserId");
                x.MapRightKey("ChildId");
            });
        }
    }
}