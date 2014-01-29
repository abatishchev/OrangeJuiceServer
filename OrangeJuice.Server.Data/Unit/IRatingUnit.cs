using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IRatingUnit : IDisposable
	{
		Task<int> AddOrUpdate(Rating rating);

		Task<Rating> Get(RatingId ratingId);

		Task<int> Remove(Rating rating);
	}
}