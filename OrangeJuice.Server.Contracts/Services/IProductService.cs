using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IProductService : IDisposable
	{
		Task<ProductDescriptor> Get(Guid productId);

		Task<ProductDescriptor[]> Search(string barcode, BarcodeType barcodeType);
	}
}