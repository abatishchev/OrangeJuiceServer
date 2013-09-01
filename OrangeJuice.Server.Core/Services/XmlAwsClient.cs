using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		#region Fields
		private readonly IQueryBuilder _queryBuilder;
		private readonly IDocumentLoader _documentLoader;
		private readonly IItemProvider _itemProvider;
		#endregion

		#region Constructors
		public XmlAwsClient(IQueryBuilder queryBuilder, IDocumentLoader documentLoader, IItemProvider itemProvider)
		{
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (documentLoader == null)
				throw new ArgumentNullException("documentLoader");
			if (itemProvider == null)
				throw new ArgumentNullException("itemProvider");

			_queryBuilder = queryBuilder;
			_documentLoader = documentLoader;
			_itemProvider = itemProvider;
		}
		#endregion

		#region IAwsClient Members
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

			string url = _queryBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			XElement items = _itemProvider.GetItems(doc);

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

			string url = _queryBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			XElement items = _itemProvider.GetItems(doc);

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

			string url = _queryBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			XElement items = _itemProvider.GetItems(doc);

			return items.Element(items.Name.Namespace + "Item");
		}
		#endregion
	}
}