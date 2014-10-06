using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface IProductRepository : IDisposable
	{
		IEnumerable<IProduct> Search(string barcode, BarcodeType barcodeType);

		Task<Guid> Save(string barcode, BarcodeType barcodeType, string sourceProductId);
	}
}