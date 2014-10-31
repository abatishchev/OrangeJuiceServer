using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IProductService
	{
		Task<ProductDescriptor> Get(Guid productId);

		Task<ProductDescriptor[]> Search(string barcode, BarcodeType barcodeType);
	}
}