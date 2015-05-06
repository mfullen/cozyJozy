using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Entity
{
    public class CozyJozyContext   : IdentityDbContext<User>
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
        public DbSet<Title> Title { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Child)
                .WithMany(x => x.Followers)
            .Map(x =>
            {
                x.ToTable("UserChildren");
                x.MapLeftKey("UserId");
                x.MapRightKey("ChildId");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}