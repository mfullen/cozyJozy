using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Entity.Repositories
{
    public class ChildRepository : RepositoryBase<Child>, IChildRepository
    {
        public ChildRepository(DbContext context) : base(context)
        {
        }
    }
}