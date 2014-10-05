using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<ProductDescriptor[]> GetItems(ProductDescriptorSearchCriteria searchCriteria);
	}
}