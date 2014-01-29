using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeJuice.Server
{
	public static class EnumerableExtensions
	{
		public static async Task<T> FirstOrDefaultAsync<T>(this IEnumerable<Task<T>> sources, Func<T, bool> predicate)
			where T : class
		{
			foreach (Task<T> item in sources)
			{
				T result = await item;
				if (predicate(result))
					return result;
			}
			return null;
		}

		public static TResult FirstOrDefaultNotNull<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
			where T : class
			where TResult : class
		{
			T item = source.FirstOrDefault();
			return item != null ? selector(item) : null;
		}
	}
}