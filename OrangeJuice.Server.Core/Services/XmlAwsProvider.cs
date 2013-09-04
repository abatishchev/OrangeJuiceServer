﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsProvider : IAwsProvider
	{
		#region Fields
		private readonly Func<IAwsClient> _clientFactory;
		#endregion

		#region Ctor
		public XmlAwsProvider(Func<IAwsClient> clientFactory)
		{
			if (clientFactory == null)
				throw new ArgumentNullException("clientFactory");
			_clientFactory = clientFactory;
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

			IAwsClient client = _clientFactory();
			XElement items = await client.GetItems(args);
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

			IAwsClient client = _clientFactory();
			XElement items = await client.GetItems(args);
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

			IAwsClient client = _clientFactory();
			XElement items = await client.GetItems(args);
			return items.Element(items.Name.Namespace + "Item");
		}
		#endregion
	}
}