using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
		{
			return query.Skip((page - 1) * pageSize).Take(pageSize);
		}
	}
}
