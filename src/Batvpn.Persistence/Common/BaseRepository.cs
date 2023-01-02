using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Batvpn.Persistence.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly BatVpnDbContext context;

        public BaseRepository(BatVpnDbContext context)
        {
            this.context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> GetAllByQuery(Expression<Func<T, bool>> filter = null, Expression<Func<T, bool>> order = null, int page = 1, int count = 10)
        {
            var query = context.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }
            if (order != null)
            {
                query = query.OrderBy(order).AsQueryable();
            }
            var data = query.Skip((page - 1) * count).Take(count);
            return data;
        }
        public async Task<T> FirstOrDefaultByQueryAsync(Expression<Func<T, bool>> filter = null)
        {
            var query = context.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }
            var data = await query.FirstOrDefaultAsync();
            return data;
        }
        public async Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            var query = context.Set<T>().AsQueryable().Where(filter).AsQueryable();
            context.Set<T>().RemoveRange(query);
            await context.SaveChangesAsync();
        }

        public async Task RemoveEntityAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(List<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
            await context.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
