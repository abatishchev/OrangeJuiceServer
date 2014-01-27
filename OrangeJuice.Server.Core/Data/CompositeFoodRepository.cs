using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class CompositeFoodRepository : IFoodRepository
	{
		#region Fields
		private readonly IFoodProvider[] _foodProviders;
		#endregion

		#region Ctor
		public CompositeFoodRepository(IEnumerable<IFoodProvider> foodProviders)
		{
			_foodProviders = foodProviders.ToArray();
		}
		#endregion

		#region IFoodRepository members
		public async Task<IEnumerable<FoodDescriptor>> Search(string title)
		{
			var sources = _foodProviders.Select(p => p.Search(title));
			return await ReadProvider(sources);
		}

		public async Task<FoodDescriptor> Lookup(string barcode, BarcodeType barcodeType)
		{
			var sources = _foodProviders.Select(p => p.Lookup(barcode, barcodeType.ToString()));
			return await ReadProvider(sources);
		}
		#endregion

		#region Methods
		private static async Task<T> ReadProvider<T>(IEnumerable<Task<T>> sources) where T : class
		{
			foreach (Task<T> s in sources)
			{
				T result = await s;
				if (result != null)
					return result;
			}
			return null;
		}
		#endregion
	}
}