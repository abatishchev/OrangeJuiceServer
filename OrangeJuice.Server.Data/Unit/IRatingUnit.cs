using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IRatingUnit : IDisposable
	{
		Task AddOrUpdate(Rating rating);

		Task<Rating> Get(Guid userId, Guid productId);

		Task<Rating[]> Get(Guid productId);

		Task Delete(Rating rating);
	}
}