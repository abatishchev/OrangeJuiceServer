using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAzureProductProvider
	{
		Task<ProductDescriptor> Get(Guid productId);

		Task Save(ProductDescriptor descriptor);
	}
}