using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Factory;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClientPipeline : ObjectPipeline
	{
		#region Fields
		private readonly IUrlBuilder _urlBuilder;
		private readonly IHttpClient _httpClient;
		private readonly IItemSelector _itemSelector;
		private readonly IFactory<ProductDescriptor, XElement> _factory;

		#endregion

		#region Ctor
		public XmlAwsClientPipeline(IUrlBuilder urlBuilder, IHttpClient httpClient, IItemSelector itemSelector, IFactory<ProductDescriptor, XElement> factory)
		{
			_urlBuilder = urlBuilder;
			_httpClient = httpClient;
			_itemSelector = itemSelector;
			_factory = factory;
		}

		#endregion

		#region ObjectPipeline members

		protected override IEnumerable<IPipelineFilter> GetFilters()
		{
			yield return new PipelineFilter<ProductDescriptorSearchCriteria, Uri>(s => _urlBuilder.BuildUrl(s));
			yield return new PipelineFilter<Uri, Task<string>>(async u => await _httpClient.GetStringAsync(u));
		    yield return new PipelineFilter<Task<string>, Task<IEnumerable<XElement>>>(async t =>
		                                                                                     {
		                                                                                         var r = await t;
		                                                                                         return _itemSelector.SelectItems(r);
		                                                                                     });
		    yield return new PipelineFilter<Task<IEnumerable<XElement>>, Task<IEnumerable<ProductDescriptor>>>(async t =>
		                                                                                                             {
		                                                                                                                 var items = await t;
		                                                                                                                 return items.Select(i => _factory.Create(i));
		                                                                                                             });
		}

		#endregion
	}
}