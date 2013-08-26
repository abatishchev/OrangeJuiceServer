using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Builders;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	// ReSharper disable PossibleNullReferenceException
	public sealed class AwsClient : IAwsClient
	{
		#region Fields
		private readonly ArgumentBuilder _argumentBuilder;
		private readonly QueryBuilder _queryBuilder;
		private readonly SignatureBuilder _signatureBuilder;
		private readonly DocumentLoader _documentLoader;
		#endregion

		#region Constructors
		public AwsClient(ArgumentBuilder argumentBuilder, QueryBuilder queryBuilder, SignatureBuilder signatureBuilder, DocumentLoader documentLoader)
		{
			if (argumentBuilder == null)
				throw new ArgumentNullException("argumentBuilder");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (signatureBuilder == null)
				throw new ArgumentNullException("signatureBuilder");
			if (documentLoader == null)
				throw new ArgumentNullException("documentLoader");

			_argumentBuilder = argumentBuilder;
			_queryBuilder = queryBuilder;
			_signatureBuilder = signatureBuilder;
			_documentLoader = documentLoader;
		}
		#endregion

		#region IAwsClient Members
		public async Task<IEnumerable<string>> ItemSearch(string title)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemSearch" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Small" },
				{ "Title", title }
			};

			string url = BuildUrl(args);
			XDocument doc = await _documentLoader.LoadXml(url);
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = GetItems(doc, ns);
			return items.Elements(ns + "Item")
						.Elements(ns + "ASIN")
						.Select(e => e.Value);
		}

		public async Task<XElement> ItemDescription(string id)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "ResponseGroup", "ItemAttributes" },
				{ "ItemId", id }
			};

			string url = BuildUrl(args);
			XDocument doc = await _documentLoader.LoadXml(url);
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = GetItems(doc, ns);
			return items.Element(ns + "Item")
						.Element(ns + "ItemAttributes");
		}

		public async Task<XElement> ItemImages(string id)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "ResponseGroup", "Images" },
				{ "ItemId", id }
			};

			string url = BuildUrl(args);
			XDocument doc = await _documentLoader.LoadXml(url);
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = GetItems(doc, ns);
			return items.Element(ns + "Item")
						.Element(ns + "ItemAttributes");
		}
		#endregion

		#region Methods
		private string BuildUrl(IDictionary<string, string> args)
		{
			args = _argumentBuilder.BuildArgs(args);
			string query = _queryBuilder.BuildQuery(args);
			return _signatureBuilder.SignQuery(query);
		}

		private static XElement GetItems(XDocument doc, XNamespace ns)
		{
			var items = doc.Root.Element(ns + "Items");
			if (items == null)
				throw new InvalidOperationException("Response recieved has no items");
			if (!IsValid(items, ns))
				throw new InvalidOperationException("Response recieved was not valid");
			return items;
		}

		private static bool IsValid(XContainer items, XNamespace ns)
		{
			return (bool)items.Element(ns + "Request")
							  .Element(ns + "IsValid");
		}
		#endregion
	}
}