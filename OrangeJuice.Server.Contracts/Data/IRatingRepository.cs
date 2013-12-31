using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IRatingRepository
	{
		Task AddOrUpdate(Guid userGuid, string productId, byte value);

		Task Delete(Guid userGuid, string productId);

		Task<IRating> Search(Guid userGuid, string productId);
	}
}