using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Factory;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		private readonly IUrlBuilder _urlBuilder;
		private readonly IHttpClient _httpClient;
		private readonly IItemSelector _itemSelector;
		private readonly IFactory<ProductDescriptor, XElement> _factory;

		public XmlAwsClient(IUrlBuilder urlBuilder, IHttpClient httpClient, IItemSelector itemSelector, IFactory<ProductDescriptor, XElement> factory)
		{
			_urlBuilder = urlBuilder;
			_httpClient = httpClient;
			_itemSelector = itemSelector;
			_factory = factory;
		}

		public async Task<ProductDescriptor[]> GetItems(ProductDescriptorSearchCriteria searchCriteria)
		{
			Uri url = _urlBuilder.BuildUrl(searchCriteria);
			string response = await _httpClient.GetStringAsync(url);
			var items = _itemSelector.SelectItems(response);
			return items.Select(_factory.Create).ToArray();
		}
	}
}