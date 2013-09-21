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
		private readonly IAwsClient _awsClient;
		#endregion

		#region Ctor
		public AwsProvider(IAwsClient awsClient)
		{
			if (awsClient == null)
				throw new ArgumentNullException("awsClient");
			_awsClient = awsClient;
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

			return await _awsClient.GetItems(args);
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

			var items = await _awsClient.GetItems(args);
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

			var items = await _awsClient.GetItems(args);
			return items.Single();
		}
		#endregion
	}
}