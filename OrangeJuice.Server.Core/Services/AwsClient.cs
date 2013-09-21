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
		private readonly IDocumentLoader _documentLoader;
		private readonly IItemSelector _itemSelector;
		#endregion

		#region Ctor
		public AwsClient(IQueryBuilder queryBuilder, IDocumentLoader documentLoader, IItemSelector itemSelector)
		{
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (documentLoader == null)
				throw new ArgumentNullException("documentLoader");
			if (itemSelector == null)
				throw new ArgumentNullException("itemSelector");

			_queryBuilder = queryBuilder;
			_documentLoader = documentLoader;
			_itemSelector = itemSelector;
		}
		#endregion

		#region IAwsClient Members
		public async Task<IEnumerable<XElement>> GetItems(IDictionary<string, string> args)
		{
			if (args == null)
				throw new ArgumentNullException("args");

			string url = _queryBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			return _itemSelector.GetItems(doc);
		}
		#endregion
	}
}