using System;
using System.Threading.Tasks;

using Ab.Amazon.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAzureProductProvider
	{
		Task<ProductDescriptor> Get(Guid productId);

		Task Save(ProductDescriptor descriptor);

		Task<Uri> GetUrl(Guid productId);
	}
}