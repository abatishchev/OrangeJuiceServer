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
		private readonly IUrlBuilder _urlBuilder;
		private readonly IDocumentLoader _documentLoader;
		private readonly IItemSelector _itemSelector;
		#endregion

		#region Ctor
		public AwsClient(IUrlBuilder urlBuilder, IDocumentLoader documentLoader, IItemSelector itemSelector)
		{
			_urlBuilder = urlBuilder;
			_documentLoader = documentLoader;
			_itemSelector = itemSelector;
		}
		#endregion

		#region IAwsClient members
		public async Task<IEnumerable<XElement>> GetItems(IDictionary<string, string> args)
		{
			Uri url = _urlBuilder.BuildUrl(args);
			XDocument doc = await _documentLoader.Load(url);
			return _itemSelector.SelectItems(doc);
		}
		#endregion
	}
}