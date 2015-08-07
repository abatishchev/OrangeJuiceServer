using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IAwsProductProvider
	{
		Task<ProductDescriptor[]> Search(string barcode, BarcodeType barcodeType);
	}
}