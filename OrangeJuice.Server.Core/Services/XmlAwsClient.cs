﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
		private readonly IHttpClient _httpClient;
		private readonly IItemSelector _itemSelector;
		private readonly IFactory<XElement, ProductDescriptor> _factory;
		#endregion

		#region Ctor
		public AwsClient(IUrlBuilder urlBuilder, IHttpClient httpClient, IItemSelector itemSelector, IFactory<XElement, ProductDescriptor> factory)
		{
			_urlBuilder = urlBuilder;
			_httpClient = httpClient;
			_itemSelector = itemSelector;
			_factory = factory;
		}
		#endregion

		#region IAwsClient members
		public async Task<IEnumerable<ProductDescriptor>> GetItems(ProductDescriptorSearchCriteria searchCriteria)
		{
			Uri url = _urlBuilder.BuildUrl(searchCriteria);
			Stream stream = await _httpClient.GetStreamAsync(url);
			var items = _itemSelector.SelectItems(stream);
			return items.Select(_factory.Create);
		}
		#endregion
	}
}