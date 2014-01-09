using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface IRatingRepository : IDisposable
	{
		Task AddOrUpdate(Guid userGuid, string productId, byte ratingValue);

		Task Delete(Guid userGuid, string productId);

		Task<IRating> Search(Guid userGuid, string productId);
	}
}