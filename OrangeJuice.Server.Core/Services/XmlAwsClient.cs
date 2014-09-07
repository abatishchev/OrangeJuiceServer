using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsClient : IAwsClient
	{
		#region Fields
		private readonly IUrlBuilder _urlBuilder;
		private readonly IHttpCllient _httpClient;
		private readonly IDocumentLoader _documentLoader;
		private readonly IItemSelector _itemSelector;
		private readonly IFactory<XElement, ProductDescriptor> _factory;
		#endregion

		#region Ctor
		public AwsClient(IUrlBuilder urlBuilder, IHttpClient httpClient, IDocumentLoader documentLoader, IItemSelector itemSelector, IFactory<XElement, ProductDescriptor> factory)
		{
			_urlBuilder = urlBuilder;
			_httpClient = httpClient;		
			_documentLoader = documentLoader;
			_itemSelector = itemSelector;
			_factory = factory;
		}
		#endregion

		#region IAwsClient members
		public async Task<IEnumerable<ProductDescriptor>> GetItems(ProductDescriptorSearchCriteria searchCriteria)
		{
			Uri url = _urlBuilder.BuildUrl(searchCriteria);
			Stream stream = await _httpClient.GetStreamAsync(url);
			XDocument responce = await _documentLoader.Load(stream);
			var items = _itemSelector.SelectItems(doc);
			return items.Select(_factory.Create);
		}
		#endregion
	}
}