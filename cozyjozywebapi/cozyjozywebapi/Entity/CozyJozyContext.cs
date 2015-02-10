using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Entity
{
    public class CozyJozyContext : DbContext
    {
        public CozyJozyContext()
            : base("CozyJozy")
        {

        }
        public DbSet<Feedings> Feedings { get; set; }
    }
}