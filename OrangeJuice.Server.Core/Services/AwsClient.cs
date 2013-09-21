using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsClient : IAwsClient
	{
		#region Fields
		private readonly IQueryBuilder _queryBuilder;
		private readonly IDocumentLoaderFactory _documentLoaderFactory;
		private readonly IItemSelector _itemSelector;
		#endregion

		#region Ctor
		public AwsClient(IQueryBuilder queryBuilder, IDocumentLoaderFactory documentLoaderFactory, IItemSelector itemSelector)
		{
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (documentLoaderFactory == null)
				throw new ArgumentNullException("documentLoaderFactory");
			if (itemSelector == null)
				throw new ArgumentNullException("itemSelector");

			_queryBuilder = queryBuilder;
			_documentLoaderFactory = documentLoaderFactory;
			_itemSelector = itemSelector;
		}
		#endregion

		#region IAwsClient Members
		public async Task<IEnumerable<XElement>> GetItems(IDictionary<string, string> args)
		{
			if (args == null)
				throw new ArgumentNullException("args");

			string url = _queryBuilder.BuildUrl(args);
			IDocumentLoader documentLoader = _documentLoaderFactory.Create();
			XDocument doc = await documentLoader.Load(url);
			return _itemSelector.GetItems(doc);
		}
		#endregion
	}
}