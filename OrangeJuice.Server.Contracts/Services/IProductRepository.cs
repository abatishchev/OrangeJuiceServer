using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IProductRepository
	{
		Task<IEnumerable<ProductDescriptor>> Search(string title);

		Task<ProductDescriptor> Lookup(string barcode, BarcodeType barcodeType);
	}
}