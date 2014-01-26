using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IFoodRepository
	{
		Task<IEnumerable<FoodDescriptor>> Search(string title);

		Task<FoodDescriptor> Lookup(string barcode, BarcodeType barcodeType);
	}
}