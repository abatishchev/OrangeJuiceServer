using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IProductService : IDisposable
	{
		Task<ProductDescriptor> Get(Guid productId);

		Task<IEnumerable<ProductDescriptor>> Search(string barcode, BarcodeType barcodeType);
	}
}