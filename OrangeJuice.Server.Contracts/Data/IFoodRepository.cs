using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IFoodRepository
	{
		Task<ICollection<FoodDescription>> SearchByTitle(string title);

		Task<FoodDescription> SearchByBarcode(string barcode);
	}
}