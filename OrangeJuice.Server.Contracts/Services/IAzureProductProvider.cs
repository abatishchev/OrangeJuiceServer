using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IAzureProductProvider
	{
		Task<ProductDescriptor> Get(Guid productId);

		Task Save(ProductDescriptor descriptor);

		Uri GetUrl(Guid productId);
	}
}