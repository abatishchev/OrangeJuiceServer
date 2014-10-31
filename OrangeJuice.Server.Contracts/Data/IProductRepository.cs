using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public interface IProductRepository
	{
		Task<Product[]> Search(string barcode, BarcodeType barcodeType);

		Task<Guid> Save(string barcode, BarcodeType barcodeType, string sourceProductId);
	}
}