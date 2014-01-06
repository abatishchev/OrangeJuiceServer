using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelRatingRepository : IRatingRepository
	{
		#region Fields
		// ReSharper disable once InconsistentNaming
		private readonly Func<IModelContainer> CreateContainer;
		#endregion

		#region Ctor
		public EntityModelRatingRepository()
			: this(() => new ModelContainer())
		{
		}

		internal EntityModelRatingRepository(Func<IModelContainer> createContainer)
		{
			CreateContainer = createContainer;
		}
		#endregion

		#region IRatingRepository members
		public async Task AddOrUpdate(Guid userGuid, string productId, byte value)
		{
			using (IModelContainer db = CreateContainer())
			{
				User user = await db.Users.SingleOrDefaultAsync(u => u.UserGuid == userGuid);
				if (user == null)
					throw new ObjectNotFoundException();

				Rating rating = await db.Ratings.FindAsync(user.UserId, productId) ??
								new Rating
								{
									User = user,
									ProductId = productId,
								};
				rating.Value = value;

				db.Ratings.AddOrUpdate(rating);

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(Guid userGuid, string productId)
		{
			using (IModelContainer db = CreateContainer())
			{
				Rating rating = await db.Ratings.SingleOrDefaultAsync(r => r.User.UserGuid == userGuid &&
																		   r.ProductId == productId);
				if (rating == null)
					throw new ObjectNotFoundException();

				db.Ratings.Remove(rating);

				await db.SaveChangesAsync();
			}
		}

		public async Task<IRating> Search(Guid userGuid, string productId)
		{
			using (IModelContainer db = CreateContainer())
			{
				return await db.Ratings.SingleOrDefaultAsync(r => r.User.UserGuid == userGuid &&
																  r.ProductId == productId);
			}
		}
		#endregion
	}
}