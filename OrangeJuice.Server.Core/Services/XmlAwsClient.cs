using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		private readonly IUrlBuilder _urlBuilder;
		private readonly IHttpClient _httpClient;
		private readonly IItemSelector _itemSelector;
		private readonly IFilter<XElement> _itemFilter;

		public XmlAwsClient(IUrlBuilder urlBuilder, IHttpClient httpClient, IItemSelector itemSelector, IFilter<XElement> itemFilter)
		{
			_urlBuilder = urlBuilder;
			_httpClient = httpClient;
			_itemSelector = itemSelector;
			_itemFilter = itemFilter;
		}

		public async Task<XElement[]> GetItems(AwsProductSearchCriteria searchCriteria)
		{
			Uri url = _urlBuilder.BuildUrl(searchCriteria);
			string response = await _httpClient.GetStringAsync(url);
			return _itemSelector.SelectItems(response)
								.Where(_itemFilter.Filter)
								.ToArray();
		}
	}
}