using System.Collections.Generic;
using System.Linq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Validation
{
	public sealed class NullFoodDescriptorValidator : IValidator<FoodDescriptor>
	{
		#region IValidator methods
		public bool IsValid(FoodDescriptor item)
		{
			return GetProperties(item).All(p => p != null);
		}
		#endregion


		#region Methods
		private static IEnumerable<object> GetProperties(FoodDescriptor item)
		{
			yield return item.Id;
			yield return item.Title;
			yield return item.Brand;
			yield return item.SmallImageUrl;
			yield return item.MediumImageUrl;
			yield return item.LargeImageUrl;
		}
		#endregion
	}
}