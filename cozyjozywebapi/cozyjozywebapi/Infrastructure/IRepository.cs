using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cozyjozywebapi.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity);
        T Delete(T entity);
        IQueryable<T> Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll();
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
