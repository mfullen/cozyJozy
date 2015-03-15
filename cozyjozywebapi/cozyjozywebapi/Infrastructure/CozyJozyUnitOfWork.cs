﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using cozyjozywebapi.Infrastructure.Core;

namespace cozyjozywebapi.Infrastructure
{
    public class CozyJozyUnitOfWork : Disposable, IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IFeedingRepository _feedingRepository;
        private readonly IChildRepository _childRepository;
        private readonly IChildPermissionsRepository _childPermissionsRepository;
        private readonly IDiaperChangesRepository _diaperChangesRepository;
        private readonly IRoleRepository _roleRepository;

        public CozyJozyUnitOfWork(DbContext context, 
            IFeedingRepository feedingRepository, 
            IChildRepository childRepository, 
            IChildPermissionsRepository childPermissionsRepository,
            IDiaperChangesRepository diaperChangesRepository,
            IRoleRepository roleRepository)
        {
            _context = context;
            _feedingRepository = feedingRepository;
            _childRepository = childRepository;
            _childPermissionsRepository = childPermissionsRepository;
            _diaperChangesRepository = diaperChangesRepository;
            _roleRepository = roleRepository;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
           return _context.SaveChangesAsync();
        }

        public IFeedingRepository FeedingRepository
        {
            get { return _feedingRepository; }
        }

        public IChildRepository ChildRepository
        {
            get { return _childRepository; }
        }

        public IChildPermissionsRepository ChildPermissionsRepository
        {
            get { return _childPermissionsRepository; }
        }
        public IDiaperChangesRepository DiaperChangesRepository
        {
            get { return _diaperChangesRepository; }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository; }
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
