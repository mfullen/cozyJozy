using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using cozyjozywebapi.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Entity.Repositories
{
    public class UserRepository : RepositoryBase<IdentityUser>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}