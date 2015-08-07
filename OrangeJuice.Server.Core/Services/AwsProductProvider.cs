using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Factory;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProductProvider : IAwsProductProvider
	{
		private readonly IAwsClient _client;
		private readonly IFactory<ProductDescriptor, XElement> _factory;

		public AwsProductProvider(IAwsClient client, IFactory<ProductDescriptor, XElement> factory)
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
			return items.Select(_factory.Create).ToArray();
		}
	}
}