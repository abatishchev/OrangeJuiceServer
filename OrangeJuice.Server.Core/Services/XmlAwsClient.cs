using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlAwsClient : IAwsClient
	{
		private readonly IPipeline _pipeline;

		public XmlAwsClient(IPipeline pipeline)
		{
			_pipeline = pipeline;
		}

		public async Task<ProductDescriptor[]> GetItems(ProductDescriptorSearchCriteria searchCriteria)
		{
			return await (Task<ProductDescriptor[]>)_pipeline.Execute(searchCriteria);
		}
	}
}