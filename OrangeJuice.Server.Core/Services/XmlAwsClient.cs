using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		private readonly IPipeline _pipeline;

		public XmlAwsClient(IPipeline pipeline)
		{
			_pipeline = pipeline;
		}

		public async Task<IEnumerable<ProductDescriptor>> GetItems(ProductDescriptorSearchCriteria searchCriteria)
		{
			return await (Task<IEnumerable<ProductDescriptor>>)_pipeline.Execute(searchCriteria);
		}
	}
}