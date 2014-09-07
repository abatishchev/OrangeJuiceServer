using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<IEnumerable<ProductDescriptor>> GetItems(ProductDescriptorSearchCriteria searchCriteria);
	}
}