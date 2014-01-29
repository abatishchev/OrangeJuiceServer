using System.Data.Entity.Migrations;
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
		public Task<int> AddOrUpdate(Rating rating)
		{
			_db.Ratings.AddOrUpdate(rating);

			return _db.SaveChangesAsync();
		}

		public Task<Rating> Get(RatingId ratingId)
		{
			return _db.Ratings.FindAsync(ratingId.UserId, ratingId.ProductId);
		}

		public Task<int> Remove(Rating rating)
		{
			_db.Ratings.Remove(rating);

			return _db.SaveChangesAsync();
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