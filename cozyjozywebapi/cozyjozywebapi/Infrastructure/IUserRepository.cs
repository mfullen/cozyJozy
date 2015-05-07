using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cozyjozywebapi.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
