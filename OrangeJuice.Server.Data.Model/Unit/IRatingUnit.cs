using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IRatingUnit : IDisposable
	{
		Task<int> AddOrUpdate(Rating rating);

		Task<Rating> Get(int userId, string productId);

		Task<Rating> Get(Guid userGuid, string productId);

		Task<int> Remove(Rating rating);
	}
}