using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<ProductDescriptor[]> GetItems(ProductDescriptorSearchCriteria searchCriteria);
	}
}