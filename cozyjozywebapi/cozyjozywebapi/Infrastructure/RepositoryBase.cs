using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

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

        public virtual T Update(T entity)
        {
            T updatedEntity = _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            return updatedEntity;
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

        public virtual IQueryable<T> GetAll()
        {
            return _dbset;
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault();
        }
    }
}