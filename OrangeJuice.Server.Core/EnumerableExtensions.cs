using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Except<T>(this IEnumerable<T> sequence, T item)
		{
			return sequence.Except(new[] { item });
		}
	}
}