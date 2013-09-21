using System;
using System.Collections.Generic;
using System.Linq;
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
			if (client == null)
				throw new ArgumentNullException("client");
			_client = client;
		}
		#endregion

		#region IAwsProvider Members
		public async Task<IEnumerable<XElement>> SearchItem(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemSearch" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Small" },
				{ "Title", title }
			};

			return await _client.GetItems(args);
		}

		public async Task<XElement> LookupAttributes(string id)
		{
			if (String.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "ResponseGroup", "ItemAttributes" },
				{ "ItemId", id }
			};

			var items = await _client.GetItems(args);
			return items.Single();
		}

		public async Task<XElement> LookupImages(string id)
		{
			if (String.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "ResponseGroup", "Images" },
				{ "ItemId", id }
			};

			var items = await _client.GetItems(args);
			return items.Single();
		}
		#endregion
	}
}