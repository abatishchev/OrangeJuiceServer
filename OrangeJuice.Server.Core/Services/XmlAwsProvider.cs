using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsProvider : IAwsProvider
	{
		#region Fields
		private readonly XmlAwsProvider _awsProvider;
		#endregion

		#region Ctor
		public XmlAwsProvider(XmlAwsProvider awsProvider)
		{
			if (awsProvider == null)
				throw new ArgumentNullException("awsProvider");
			_awsProvider = awsProvider;
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

			var items = await _awsProvider.GetItems(args);

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

			string url = _awsProvider._queryBuilder.BuildUrl(args);
			XDocument doc = await _awsProvider._documentLoader.Load(url);
			XElement items = _awsProvider._itemProvider.GetItems(doc);

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

			string url = _awsProvider._queryBuilder.BuildUrl(args);
			XDocument doc = await _awsProvider._documentLoader.Load(url);
			XElement items = _awsProvider._itemProvider.GetItems(doc);

			return items.Element(items.Name.Namespace + "Item");
		}
		#endregion
	}
}