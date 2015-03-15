using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace cozyjozywebapi.Infrastructure.Core
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity, Func<T, int> getKey);
        T Delete(T entity);
        IQueryable<T> Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Where(Expression<Func<T, bool>> where);
        IQueryable<T> All();
        IQueryable<T> Many(Expression<Func<T, bool>> where);

        /**ASYNC SUPPORT **/
        //Task<T> AddAsync(T t);
        //Task<T> DeleteAsync(T t);
        Task<List<T>> AllAsync();
        //Task<T> UpdateAsync(T t);
        Task<int> CountAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match);
    }
}
