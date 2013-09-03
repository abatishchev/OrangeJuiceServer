using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsProvider : IAwsProvider
	{
		#region Fields
		private readonly IAwsClient _awsClient;
		#endregion

		#region Ctor
		public XmlAwsProvider(IAwsClient awsClient)
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

			var items = await _awsClient.GetItems(args);
			return items.Elements(items.Name.Namespace + "Item");
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
			return items.Element(items.Name.Namespace + "Item");
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
			return items.Element(items.Name.Namespace + "Item");
		}
		#endregion
	}
}