using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface IProductRepository : IDisposable
	{
		Task<IProduct> Search(string barcode, BarcodeType barcodeType);

		Task<IProduct> Save(string barcode, BarcodeType barcodeType);
	}
}