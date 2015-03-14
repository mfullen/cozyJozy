using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cozyjozywebapi.Infrastructure
{
    public class CozyJozyUnitOfWork : Disposable, IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IFeedingRepository _feedingRepository;

        public CozyJozyUnitOfWork(DbContext context, IFeedingRepository feedingRepository)
        {
            _context = context;
            _feedingRepository = feedingRepository;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IFeedingRepository FeedingRepository
        {
            get { return _feedingRepository; }
        }
    }

    public class Disposable : IDisposable
    {
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }

            _isDisposed = true;
        }

        protected virtual void DisposeCore()
        {
        }
    }
}
