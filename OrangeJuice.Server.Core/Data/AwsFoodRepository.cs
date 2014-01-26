using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		#region Fields
		private readonly IAwsProvider _provider;
		private readonly IFoodDescriptionFactory _factory;
		#endregion

		#region Ctor
		public AwsFoodRepository(IAwsProvider provider, IFoodDescriptionFactory factory)
		{
			_provider = provider;
			_factory = factory;
		}
		#endregion

		#region IFoodRepository members
		public async Task<IEnumerable<FoodDescription>> SearchTitle(string title)
		{
			var items = await _provider.SearchItems(title);
			return items.Select(i => _factory.Create(i));
		}

		public async Task<IEnumerable<FoodDescription>> SearchBarcode(string barcode, BarcodeType barcodeType)
		{
			var items = await _provider.ItemLookup(barcode, barcodeType.ToString());
			return items.Select(i => _factory.Create(i));
		}
		#endregion

	}
}