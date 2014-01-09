using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityRatingRepository : IRatingRepository
	{
		#region Fields
		private readonly IFactory<IModelContainer> _containerFactory;
		private readonly IUserUnit _userUnit;
		#endregion

		#region Ctor
		public EntityRatingRepository(IFactory<IModelContainer> containerFactory, IUserUnit userUnit)
		{
			_containerFactory = containerFactory;
			_userUnit = userUnit;
		}
		#endregion

		#region IRatingRepository members
		public async Task AddOrUpdate(Guid userGuid, string productId, byte ratingValue)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				User user = await _userUnit.GetUser(userGuid);
				if (user == null)
					throw new ObjectNotFoundException();

				Rating rating = await db.Ratings.FindAsync(user.UserId, productId) ??
								new Rating
								{
									User = user,
									ProductId = productId,
								};
				rating.Value = ratingValue;

				db.Ratings.AddOrUpdate(rating);

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(Guid userGuid, string productId)
		{
			using (IModelContainer db = _containerFactory.Create())
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
			using (IModelContainer db = _containerFactory.Create())
			{
				return await db.Ratings.SingleOrDefaultAsync(r => r.User.UserGuid == userGuid &&
																  r.ProductId == productId);
			}
		}
		#endregion
	}

	public interface IUserUnit
	{
		Task<User> GetUser(Guid userGuid);
	}

	public sealed class EntityUserUnit : IUserUnit
	{
		private readonly IFactory<IModelContainer> _containerFactory;

		public EntityUserUnit(IFactory<IModelContainer> containerFactory)
		{
			_containerFactory = containerFactory;
		}

		public Task<User> GetUser(Guid userGuid)
		{
			IFactory<IModelContainer> containerFactory
		}
	}
}