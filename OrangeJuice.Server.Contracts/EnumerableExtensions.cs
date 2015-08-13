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

		public static IEnumerable<T> AsInfinite<T>(this IEnumerable<T> sequence)
		{
			var arr = sequence.ToArray();
			if (arr.Any())
			{
				while (true)
				{
					foreach (var item in arr)
					{
						yield return item;
					}
				}
			}
		}
	}
}