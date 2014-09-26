using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeJuice.Server
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Except<T>(this IEnumerable<T> sequence, T item)
		{
			return sequence.Except(new[] { item });
		}

		public static async Task<IEnumerable<TResult>> SelectAsync<T, TResult>(this ICollection<T> sequence, Func<T, Task<TResult>> selector)
		{
			var list = new List<TResult>(sequence.Count);
			foreach (T item in sequence)
			{
				TResult result = await selector(item);
				list.Add(result);
			}
			return list;
		}
	}
}