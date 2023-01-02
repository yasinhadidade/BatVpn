using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Batvpn.Persistence.Common
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAllByQuery(Expression<Func<T, bool>> filter = null, Expression<Func<T, bool>> order = null, int page = 1, int count = 10);
        Task<T> FirstOrDefaultByQueryAsync(Expression<Func<T, bool>> filter = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<int> SaveChangesAsync();
        Task DeleteAsync(Expression<Func<T, bool>> filter);
        Task RemoveEntityAsync(T entity);
        Task RemoveRangeAsync(List<T> entities);
    }
}
