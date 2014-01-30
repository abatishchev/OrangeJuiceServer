using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IRatingUnit : IDisposable
	{
		Task AddOrUpdate(Rating rating);

		Task<Rating> Get(RatingId ratingId);

		Task Remove(Rating rating);
	}
}