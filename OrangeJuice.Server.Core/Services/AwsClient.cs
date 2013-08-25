using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Builders;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsClient : IAwsClient
	{
		#region Fields
		private readonly ArgumentBuilder _argumentBuilder;
		private readonly QueryBuilder _queryBuilder;
		private readonly SignatureBuilder _signatureBuilder;

		private readonly HttpClient _httpClient;
		#endregion

		#region Constructors
		public AwsClient(ArgumentBuilder argumentBuilder, QueryBuilder queryBuilder, SignatureBuilder signatureBuilder)
		{
			if (argumentBuilder == null)
				throw new ArgumentNullException("argumentBuilder");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (signatureBuilder == null)
				throw new ArgumentNullException("signatureBuilder");

			_argumentBuilder = argumentBuilder;
			_queryBuilder = queryBuilder;
			_signatureBuilder = signatureBuilder;

			_httpClient = new HttpClient();
		}
		#endregion

		#region IAwsClient Members
		public async Task<IEnumerable<string>> ItemSearch(string title)
		{
			var args = new Dictionary<string, string>
			{
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Small" },
				{ "Condition", "All" },
				{ "Title", title }
			};

			string url = BuildUrl(args);

			XDocument doc = await LoadDocument(url);
			XNamespace ns = doc.Root.Name.Namespace;

			var items = GetItems(doc, ns);
			return items.Elements(ns + "Item")
						.Elements(ns + "ASIN")
						.Select(e => e.Value);
		}

		public async Task<XElement> ItemDescription(string id)
		{
			throw new NotImplementedException();
		}

		public async Task<XElement> ItemImages(string id)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			_httpClient.Dispose();
		}
		#endregion

		#region Methods
		private string BuildUrl(IDictionary<string, string> args, [CallerMemberName]string operationName = null)
		{
			args = _argumentBuilder.BuildArgs(args, operationName);
			string query = _queryBuilder.BuildQuery(args);
			return _signatureBuilder.SignQuery(query);
		}

		private async Task<XDocument> LoadDocument(string url)
		{
			using (Stream stream = await _httpClient.GetStreamAsync(url))
			{
				return XDocument.Load(stream);
			}
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