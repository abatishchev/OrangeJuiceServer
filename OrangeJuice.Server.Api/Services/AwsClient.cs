using OrangeJuice.Server.Api.Builders;
using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Web;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Api.Services
{
	public sealed class AwsClient
	{
		private readonly QueryBuilder _queryBuilder;
		private readonly SignatureBuilder _signatureBuilder;
		private readonly AwsOptions _options;

		public AwsClient(AwsOptions options)
			: this(new QueryBuilder(options.AccessKey, new PercentUrlEncoder(), new UtcDateTimeProvider()),
				   new SignatureBuilder(options.SecretKey, new PercentUrlEncoder()))
		{
			_options = options;
		}

		// TODO: refactor dependency injection
		private AwsClient(QueryBuilder queryBuilder, SignatureBuilder signatureBuilder)
		{
			_queryBuilder = queryBuilder;
			_signatureBuilder = signatureBuilder;
		}

		public async Task<XElement> ItemLookup(string asin)
		{
			var args = new Dictionary<string, string>
			{
				{ "IdType", "ASIN" },
				{ "ItemId", asin }
			};

			string url = BuildUrl(args);

			XDocument doc = await LoadDocument(url);
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = GetItems(doc, ns);

			return items.Element(ns + "Item");
		}

		public async Task<IEnumerable<string>> ItemSearch(string title)
		{
			var args = new Dictionary<string, string>
			{
				{ "Condition", "All" },
				{ "SearchIndex", "Grocery" },
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

		private string BuildUrl(IDictionary<string, string> args, [CallerMemberName]string operation = null)
		{
			args = AppenArgs(args, operation);

			string query = _queryBuilder.BuildQuery(args);
			return _signatureBuilder.SignQuery(query);
		}

		private IDictionary<string, string> AppenArgs(IDictionary<string, string> args, string operation)
		{
			return new Dictionary<string, string>(args)
			{
				{ "AssociateTag", _options.AssociateTag },
				{ "Operation", operation }
			};
		}

		private static XElement GetItems(XDocument doc, XNamespace ns)
		{
			var items = doc.Root.Element(ns + "Items");

			if (!IsValid(items, ns))
				throw new Exception();

			return items;
		}

		private static bool IsValid(XContainer items, XNamespace ns)
		{
			return (bool)items.Element(ns + "Request")
							  .Element(ns + "IsValid");
		}

		private static async Task<XDocument> LoadDocument(string url)
		{
			HttpClient client = new HttpClient();
			using (Stream stream = await client.GetStreamAsync(url))
			{
				return XDocument.Load(stream);
			}
		}
	}
}