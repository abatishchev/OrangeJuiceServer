using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProductProvider : IAwsProductProvider
	{
		#region Fields
		private readonly IAwsClient _client;
		#endregion

		#region Ctor
		public AwsProductProvider(IAwsClient client)
		{
			_client = client;
		}
		#endregion

		#region IAwsProductProvider members
		public async Task<ProductDescriptor> Search(string barcode, BarcodeType barcodeType)
		{
			var searchCriteria = new ProductDescriptorSearchCriteria
			{
				Operation = "ItemLookup",
				SearchIndex = "Grocery",
				ResponseGroups = new[] { "Images", "ItemAttributes" },
				IdType = barcodeType.ToString(),
				ItemId = barcode
			};

			var items = await _client.GetItems(searchCriteria);
			return items.FirstOrDefault();
		}
		#endregion
	}
}