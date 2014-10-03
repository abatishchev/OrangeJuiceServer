using System.Collections.Generic;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		IEnumerable<ProductDescriptor> GetItems(ProductDescriptorSearchCriteria searchCriteria);
	}
}