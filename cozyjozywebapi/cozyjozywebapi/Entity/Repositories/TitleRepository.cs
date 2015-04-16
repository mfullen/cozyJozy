using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Entity.Repositories
{
    public class TitleRepository : RepositoryBase<Title>, ITitleRepository
    {
        public TitleRepository(DbContext context)
            : base(context)
        {
        }
    }
}