using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Container;

namespace OrangeJuice.Server.Data.Unit
{
	public sealed class EntityRatingUnit : IRatingUnit
	{
		#region Fields
		private readonly IFactory<IModelContainer> _containerFactory;
		#endregion

		#region Ctor
		public EntityRatingUnit(IFactory<IModelContainer> containerFactory)
		{
			_containerFactory = containerFactory;
		}
		#endregion

		#region IRatingUnit members
		public Task<int> AddOrUpdate(Rating rating)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				db.Ratings.AddOrUpdate(rating);
				return db.SaveChangesAsync();
			}
		}

		public Task<Rating> GetRating(int userId, string productId)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				return db.Ratings.FindAsync(userId, productId);
			}
		}

		public Task<Rating> GetRating(Guid userGuid, string productId)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				return db.Ratings.SingleOrDefaultAsync(r => r.User.UserGuid == userGuid &&
															r.ProductId == productId);
			}
		}

		public Task<int> Remove(Rating rating)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				db.Ratings.Remove(rating);

				return db.SaveChangesAsync();
			}
		}
		#endregion
	}
}