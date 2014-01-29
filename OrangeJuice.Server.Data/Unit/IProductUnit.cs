using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IProductUnit : IDisposable
	{
		Task<Product> Get(string barcode, BarcodeType barcodeType);
	}
}