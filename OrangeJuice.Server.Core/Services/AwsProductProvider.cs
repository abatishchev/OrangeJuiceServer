using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProductProvider : IProductProvider
	{
		#region Fields
		private readonly IAwsClient _client;
		private readonly IProductDescriptorFactory<XElement> _factory;
		#endregion

		#region Ctor
		public AwsProductProvider(IAwsClient client, IProductDescriptorFactory<XElement> factory)
		{
			_client = client;
			_factory = factory;
		}
		#endregion

		#region IProductProvider members
		public Task<ProductDescriptor> SearchId(Guid productId)
		{
			throw new NotSupportedException();
		}

		public async Task<IEnumerable<ProductDescriptor>> SearchTitle(string title)
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

		public async Task<ProductDescriptor> SearchBarcode(string barcode, BarcodeType barcodeType)
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