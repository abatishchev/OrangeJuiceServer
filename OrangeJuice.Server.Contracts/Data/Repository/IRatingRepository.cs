using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface IRatingRepository : IDisposable
	{
		Task AddOrUpdate(RatingId ratingId, byte ratingValue, string comment);

		Task Delete(RatingId ratingId);

		Task<IRating> Search(RatingId ratingId);
	}
}