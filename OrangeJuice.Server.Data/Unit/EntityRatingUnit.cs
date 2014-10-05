using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Container;

namespace OrangeJuice.Server.Data.Unit
{
	public sealed class EntityRatingUnit : IRatingUnit
	{
		#region Fields
		private readonly IModelContainer _db;
		#endregion

		#region Ctor
		public EntityRatingUnit(IModelContainer db)
		{
			_db = db;
		}
		#endregion

		#region IRatingUnit members
		public async Task AddOrUpdate(Rating rating)
		{
			_db.Ratings.AddOrUpdate(rating);

			await _db.SaveChangesAsync();
		}

		public Task<Rating> Get(Guid userId, Guid productId)
		{
			return _db.Ratings.FindAsync(userId, productId);
		}

		public async Task<Rating[]> Get(Guid productId)
		{
			Product product = await _db.Products.FindAsync(productId);
			return product != null ? product.Ratings.ToArray() : null;
		}

		public async Task Delete(Rating rating)
		{
			_db.Ratings.Remove(rating);

			await _db.SaveChangesAsync();
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_db.Dispose();
		}
		#endregion
	}
}