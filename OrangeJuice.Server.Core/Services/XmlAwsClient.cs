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

		#region Ctor
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
		public async Task<XElement> GetItems(IDictionary<string, string> args)
		{
			if (args == null)
				throw new ArgumentNullException("args");

			string url = _queryBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			return _itemProvider.GetItems(doc);
		}
		#endregion
	}
}