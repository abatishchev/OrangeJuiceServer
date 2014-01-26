using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsFoodProvider : IFoodProvider
	{
		#region Fields
		private readonly IAwsClient _client;
		private readonly IFoodDescriptionFactory _factory;
		#endregion

		#region Ctor
		public AwsFoodProvider(IAwsClient client, IFoodDescriptionFactory factory)
		{
			_client = client;
			_factory = factory;
		}
		#endregion

		#region IFoodProvider members
		public async Task<IEnumerable<FoodDescription>> Search(string title)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemSearch" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Images,ItemAttributes" },
				{ "Keywords", title }
			};

			var items = await _client.GetItems(args);
			return items.Select(i => _factory.Create(i));
		}

		public async Task<FoodDescription> Lookup(string barcode, string barcodeType)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Images,ItemAttributes" },
				{ "IdType", barcodeType },
				{ "ItemId", barcode }
			};

			var items = await _client.GetItems(args);
			return items.FirstOrDefaultNotNull(_factory.Create);
		}
		#endregion
	}
}