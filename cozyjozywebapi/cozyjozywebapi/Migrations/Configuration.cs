using cozyjozywebapi.Models;
using WebGrease.Css.Extensions;

namespace cozyjozywebapi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<cozyjozywebapi.Entity.CozyJozyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "cozyjozywebapi.Entity.CozyJozyContext";
        }

        protected override void Seed(cozyjozywebapi.Entity.CozyJozyContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Title.AddOrUpdate(t => t.Name,
                new Title { Name = "Mom" },
                new Title { Name = "Dad" },
                new Title { Name = "Brother" },
                new Title { Name = "Sister" },
                new Title { Name = "Grandma" },
                new Title { Name = "Grandpa" },
                new Title { Name = "Uncle" },
                new Title { Name = "Aunt" },
                new Title { Name = "Cousin" },
                new Title { Name = "Babysitter" },
                new Title { Name = "Parent/Guardian" }
                );
            context.SaveChanges();
            var pg = context.Title.FirstOrDefault(t => t.Name == "Parent/Guardian");

            context.ChildPermissions.ForEach(r =>
            {
                if (r.TitleId == null)
                {
                    r.TitleId = pg.Id;
                }
            });
            context.SaveChanges();
            //context.Title.FirstOrDefault(t => t.Name == "Parent/Guardian")
        }
    }
}
