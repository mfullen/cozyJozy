using System;
using System.Threading.Tasks;

namespace cozyjozywebapi.Infrastructure.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task<int> CommitAsync();
        IFeedingRepository FeedingRepository { get; }
        IChildRepository ChildRepository { get; }
        IChildPermissionsRepository ChildPermissionsRepository { get; }
        IDiaperChangesRepository DiaperChangesRepository { get; }
        IRoleRepository RoleRepository { get; }
    }
}
