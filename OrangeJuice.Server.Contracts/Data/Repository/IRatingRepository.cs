using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface IRatingRepository : IDisposable
	{
		Task AddOrUpdate(Guid userId, Guid productId, byte ratingValue, string comment);

		Task Delete(Guid ratingId, Guid productId);

		Task<IRating> Search(Guid userId, Guid productId);

		Task<IRating[]> SearchAll(Guid productId);
	}
}