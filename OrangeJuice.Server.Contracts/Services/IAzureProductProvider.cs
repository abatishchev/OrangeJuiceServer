using System;
using System.Threading.Tasks;

using Ab.Amazon.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAzureProductProvider
	{
		Task<Product> Get(Guid productId);

		Task Save(Product product);

		Task<Uri> GetUrl(Guid productId);
	}
}