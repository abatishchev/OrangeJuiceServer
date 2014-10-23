using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public interface IRatingRepository : IDisposable
	{
		Task AddOrUpdate(Guid userId, Guid productId, byte ratingValue, string comment);

		Task Delete(Guid ratingId, Guid productId);

		Task<Rating> Search(Guid userId, Guid productId);

		Task<Rating[]> SearchAll(Guid productId);
	}
}