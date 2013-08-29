using System;
using System.Linq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Filters
{
	public class ValidImageFoodDescriptionFilter : IFilter<FoodDescription>
	{
		public bool Filter(FoodDescription fd)
		{
			if (fd == null)
				throw new ArgumentNullException("foodDescription");

			return new[] { fd.LargeImageUrl, fd.MediumImageUrl, fd.SmallImageUrl }.All(u => u != null);
		}
	}
}