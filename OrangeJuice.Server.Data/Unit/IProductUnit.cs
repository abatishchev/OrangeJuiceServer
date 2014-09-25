using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IProductUnit : IDisposable
	{
		Task<Product> Add(string barcode, BarcodeType barcodeType);

		IQueryable<Product> Search(string barcode, BarcodeType barcodeType);
	}
}