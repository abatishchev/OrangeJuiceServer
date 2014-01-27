using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IFoodRepository
	{
		Task<IEnumerable<FoodDescriptor>> Search(string title);

		Task<FoodDescriptor> Lookup(string barcode, BarcodeType barcodeType);
	}
}