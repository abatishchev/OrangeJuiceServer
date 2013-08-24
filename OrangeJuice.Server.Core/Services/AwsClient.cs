﻿using System;
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
		private readonly ArgumentBuilder _argumentBuilder;
		private readonly QueryBuilder _queryBuilder;
		private readonly SignatureBuilder _signatureBuilder;

		private readonly HttpClient _httpClient;

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

		public void Dispose()
		{
			_httpClient.Dispose();
		}

		private string BuildUrl(IDictionary<string, string> args, [CallerMemberName]string operationName = null)
		{
			args = _argumentBuilder.BuildArgs(args, operationName);
			string query = _queryBuilder.BuildQuery(args);
			return _signatureBuilder.SignQuery(query);
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

		private async Task<XDocument> LoadDocument(string url)
		{
			using (Stream stream = await _httpClient.GetStreamAsync(url))
			{
				return XDocument.Load(stream);
			}
		}
	}
}