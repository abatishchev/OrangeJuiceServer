using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IFoodRepository
	{
		Task<ICollection<FoodDescription>> SearchTitle(string title);

		Task<ICollection<FoodDescription>> SearchBarcode(string barcode, BarcodeType barcodeType);
	}
}