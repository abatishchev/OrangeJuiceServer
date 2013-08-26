using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IFoodRepository
	{
		Task<FoodDescription[]> SearchByTitle(string title);
	}
}