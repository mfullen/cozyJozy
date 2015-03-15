using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Infrastructure
{
    public interface IChildRepository : IRepository<Child>
    {
    }
}
