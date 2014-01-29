using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IProductProvider
	{
		Task<IEnumerable<ProductDescriptor>> SearchTitle(string title);

		Task<ProductDescriptor> SearchBarcode(string barcode, string barcodeType);
	}
}