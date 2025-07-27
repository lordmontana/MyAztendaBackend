using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories.Abstractions
{
	public interface IRepository<T> where T : class 
	{
        IQueryable<T> Query { get; }// read-only
        Task<IEnumerable<T>> GetAllAsync();
		Task<T?> GetByIdAsync(int id);
		Task AddAsync(T entity);
		void Update(T entity);
		void Delete(T entity);
		Task<bool> SaveChangesAsync();
        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>[]? filter = null,Expression<Func<T, object>>? orderBy = null,bool ascending = true);

        Task<PagedResult<T>> QueryAsync(
                int? page = null,
                int? pageSize = null,
                Expression<Func<T, object>>? orderBy = null,
                bool ascending = true,
                params Expression<Func<T, bool>>[] filters);   // ← array here

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    }
}
