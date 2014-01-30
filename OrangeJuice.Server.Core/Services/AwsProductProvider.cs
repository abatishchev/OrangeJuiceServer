using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProductProvider : IAwsProductProvider
	{
		#region Fields
		private readonly IAwsClient _client;
		private readonly IFactory<XElement, ProductDescriptor> _factory;
		#endregion

		#region Ctor
		public AwsProductProvider(IAwsClient client, IFactory<XElement, ProductDescriptor> factory)
		{
			_client = client;
			_factory = factory;
		}
		#endregion

		#region IAwsProductProvider members
		public async Task<ProductDescriptor> Search(string barcode, BarcodeType barcodeType)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Images,ItemAttributes" },
				{ "IdType", barcodeType.ToString() },
				{ "ItemId", barcode }
			};

			var items = await _client.GetItems(args);
			return items.FirstOrDefaultNotNull(_factory.Create);
		}
		#endregion
	}
}