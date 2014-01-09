using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IRatingUnit
	{
		Task<int> AddOrUpdate(Rating rating);

		Task<Rating> GetRating(int userId, string productId);

		Task<Rating> GetRating(Guid userGuid, string productId);

		Task<int> Remove(Rating rating);
	}
}