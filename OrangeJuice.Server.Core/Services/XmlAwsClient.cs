using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Factory;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		#region Fields
		private IPipeline _pipeline;

		#endregion

		#region Ctor
		public XmlAwsClient(IUrlBuilder urlBuilder, IHttpClient httpClient, IItemSelector itemSelector, IFactory<ProductDescriptor, XElement> factory)
		{
			_pipeline = new Pipeline()
				.Register(new PipelineFilter<ProductDescriptorSearchCriteria, Uri>(s => urlBuilder.BuildUrl(s)))
				.Register(new PipelineFilter<Uri, Task<string>>(async u => await httpClient.GetStringAsync(u)))
				.Register(new PipelineFilter<string, IEnumerable<XElement>>(r => itemSelector.SelectItems(r)))
				.Register(new PipelineFilter<IEnumerable<XElement>, IEnumerable<ProductDescriptor>>(items => items.Select(i => factory.Create(i))));
		}
		#endregion

		#region IAwsClient members
		public IEnumerable<ProductDescriptor> GetItems(ProductDescriptorSearchCriteria searchCriteria)
		{
			return (IEnumerable<ProductDescriptor>)_pipeline.Execute(searchCriteria);
		}
		#endregion
	}
}