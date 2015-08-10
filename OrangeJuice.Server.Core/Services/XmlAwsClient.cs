using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		private readonly IUrlBuilder _urlBuilder;
		private readonly IHttpClient _httpClient;
		private readonly IItemSelector _itemSelector;

		public XmlAwsClient(IUrlBuilder urlBuilder, IHttpClient httpClient, IItemSelector itemSelector)
		{
			_urlBuilder = urlBuilder;
			_httpClient = httpClient;
			_itemSelector = itemSelector;
		}

		public async Task<XElement[]> GetItems(AwsProductSearchCriteria searchCriteria)
		{
			Uri url = _urlBuilder.BuildUrl(searchCriteria);
			string response = await _httpClient.GetStringAsync(url);
			return _itemSelector.SelectItems(response).ToArray();
		}
	}
}