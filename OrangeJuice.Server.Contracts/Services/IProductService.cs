using System;
using System.Threading.Tasks;

using Ab.Amazon.Data;

namespace OrangeJuice.Server.Services
{
	public interface IProductService
	{
		Task<Product> Get(Guid productId);

		Task<Product[]> Search(string barcode, BarcodeType barcodeType);
	}
}