using System.Data.Entity;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Entity.Repositories
{

    public class SleepRepository : RepositoryBase<SleepSession>, ISleepRepository
    {
        public SleepRepository(DbContext context)
            : base(context)
        {
        }
    }
}