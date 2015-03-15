using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Entity.Repositories
{
    public class FeedingRepository : RepositoryBase<Feedings>, IFeedingRepository
    {
        public FeedingRepository(DbContext context) : base(context)
        {
        }
    }
}