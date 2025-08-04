using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shared.Repositories.Persistence
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

        public async Task<IEnumerable<T>> GetAllAsync() => (await QueryAsync()).Data;

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

		public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

		public void Update(T entity) => _dbSet.Update(entity);

		public void Delete(T entity) => _dbSet.Remove(entity);

		public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public IQueryable<T> Query => _context.Set<T>().AsQueryable();

        public Task<PagedResult<T>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>[]? filter = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true)
            => QueryAsync(page, pageSize, orderBy, ascending,filter);

        public async Task<PagedResult<T>> QueryAsync(
        int? page = null,
        int? pageSize = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        params Expression<Func<T, bool>>[]? filters)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            // ↳ AND-combine every filter simply by chaining .Where
            foreach (var filter in filters ?? Array.Empty<Expression<Func<T, bool>>>())
                query = query.Where(filter);

            var total = await query.CountAsync();

            if (orderBy is not null)
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            /* paging (assume 1-based; change if you want 0-based) */
            if (page is not null && pageSize is not null)
            {
                var skip = Math.Max(0, page.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            var data = await query.ToListAsync();
            return new PagedResult<T>(data, total);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => _dbSet.AnyAsync(predicate, ct);
    }
}
