using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Entity.Repositories
{
    public class IdentityRoleRepository : RepositoryBase<IdentityRole>, IRoleRepository
    {
        public IdentityRoleRepository(DbContext context) : base(context)
        {
        }
    }
}