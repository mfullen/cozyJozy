using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cozyjozywebapi.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        IFeedingRepository FeedingRepository { get; }
    }
}
