using System.Collections.Generic;
using System.Linq;
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
			_queryBuilder = queryBuilder;
			_documentLoader = documentLoader;
			_itemSelector = itemSelector;
		}
		#endregion

		#region IAwsClient members
		public async Task<ICollection<XElement>> GetItems(IDictionary<string, string> args)
		{
			string url = _queryBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			return _itemSelector.SelectItems(doc).ToArray();
		}
		#endregion
	}
}