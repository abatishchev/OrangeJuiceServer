using System;
using System.Collections.Generic;
using System.Linq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Filters
{
	public sealed class ValidImageFoodDescriptionFilter : IFilter<FoodDescription>
	{
		public bool Filter(FoodDescription foodDescription)
		{
			if (foodDescription == null)
				throw new ArgumentNullException("foodDescription");

			var urls = GetUrls(foodDescription);
			return urls.All(u => u != null);
		}

		private static IEnumerable<object> GetUrls(FoodDescription fd)
		{
			yield return fd.LargeImageUrl;
			yield return fd.MediumImageUrl;
			yield return fd.SmallImageUrl;
		}
	}
}