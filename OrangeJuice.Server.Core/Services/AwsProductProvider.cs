using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Ab.Amazon;
using Ab.Amazon.Data;
using Ab.Factory;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProductProvider : IAwsProductProvider
	{
		private readonly IAwsClient _client;
		private readonly IFactory<ProductDescriptor, XElement, AwsProductSearchCriteria> _factory;

		public AwsProductProvider(IAwsClient client, IFactory<ProductDescriptor, XElement, AwsProductSearchCriteria> factory)
		{
			_client = client;
			_factory = factory;
		}

		public async Task<ProductDescriptor[]> Search(string barcode, BarcodeType barcodeType)
		{
			var searchCriteria = new AwsProductSearchCriteria
			{
				Operation = "ItemLookup",
				SearchIndex = "Grocery",
				ResponseGroups = new[] { "Images", "ItemAttributes" },
				IdType = barcodeType.ToString(),
				ItemId = barcode
			};

			var items = await _client.GetItems(searchCriteria);
			return items.Select(i => _factory.Create(i, searchCriteria)).ToArray();
		}
	}
}