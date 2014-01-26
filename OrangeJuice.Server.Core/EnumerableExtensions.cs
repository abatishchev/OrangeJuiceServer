using System;
using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public static class EnumerableExtensions
	{
		public static TResult FirstOrDefaultNotNull<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
			where T : class
			where TResult : class
		{
			T item = source.FirstOrDefault();
			return item != null ? selector(item) : null;
		}
	}
}