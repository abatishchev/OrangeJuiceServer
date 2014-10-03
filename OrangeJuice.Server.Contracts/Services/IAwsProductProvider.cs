using System.Collections.Generic;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IAwsProductProvider
	{
		IEnumerable<ProductDescriptor> Search(string barcode, BarcodeType barcodeType);
	}
}