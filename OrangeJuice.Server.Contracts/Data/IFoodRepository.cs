using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IFoodRepository
	{
		Task<IEnumerable<FoodDescription>> Search(string title);

		Task<FoodDescription> Lookup(string barcode, BarcodeType barcodeType);
	}
}