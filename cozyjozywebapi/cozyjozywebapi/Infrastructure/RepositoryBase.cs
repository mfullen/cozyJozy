using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using cozyjozywebapi.Infrastructure.Core;

namespace cozyjozywebapi.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        private readonly IDbSet<T> _dbset;
        private readonly DbContext _dataContext;

        protected RepositoryBase(DbContext context)
        {
            _dataContext = context;
            _dbset = context.Set<T>();
        }

        protected DbContext Table
        {
            get { return _dataContext; }
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Update(T entity, Func<T, int> getKey)
        {
            //T updatedEntity = _dbset.Attach(entity);
            //_dataContext.Entry(entity).State = EntityState.Modified;

            if (entity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = _dataContext.Entry<T>(entity);

            if (entry.State == EntityState.Detached)
            {
                T attachedEntity = _dbset.Find(getKey(entity));
                if (attachedEntity != null)
                {
                    var attachedEntry = _dataContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }
            return entity;
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual IQueryable<T> Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where(where).AsEnumerable();
            return objects.Select(obj => _dbset.Remove(obj)).AsQueryable();
        }

        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }

        public virtual IQueryable<T> All()
        {
            return _dbset;
        }

        public virtual IQueryable<T> Many(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public async Task<List<T>> AllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public T Where(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault();
        }

       

        public async Task<int> CountAsync()
        {
            return await _dbset.CountAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _dbset.SingleOrDefaultAsync(match);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _dbset.Where(match).ToListAsync();
        }
    }
}