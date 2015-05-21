using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

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
        IUserRepository UserRepository { get;  }
        ITitleRepository TitleRepository { get; }
        ISleepRepository SleepRepository { get; }
    }
}
