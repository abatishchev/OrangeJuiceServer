using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProvider : IAwsProvider
	{
		#region Fields
		private readonly IAwsClient _client;
		#endregion

		#region Ctor
		public AwsProvider(IAwsClient client)
		{
			_client = client;
		}
		#endregion

		#region IAwsProvider members
		public Task<IEnumerable<XElement>> SearchItems(string title)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemSearch" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Images,ItemAttributes" },
				{ "Keywords", title }
			};

			return _client.GetItems(args);
		}

		public Task<IEnumerable<XElement>> ItemLookup(string barcode, string barcodeType)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Images,ItemAttributes" },
				{ "IdType", barcodeType },
				{ "ItemId", barcode }
			};

			return _client.GetItems(args);
		}
		#endregion
	}
}