using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

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
		public Task<ProductDescriptor[]> Search(string barcode, BarcodeType barcodeType)
		{
			var searchCriteria = new ProductDescriptorSearchCriteria
			{
				Operation = "ItemLookup",
				SearchIndex = "Grocery",
				ResponseGroups = new[] { "Images", "ItemAttributes" },
				IdType = barcodeType.ToString(),
				ItemId = barcode
			};

			return _client.GetItems(searchCriteria);
		}
		#endregion
	}
}