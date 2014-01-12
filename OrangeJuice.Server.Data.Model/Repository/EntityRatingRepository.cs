using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using System.Transactions;

using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityRatingRepository : IRatingRepository
	{
		#region Fields
		private readonly IRatingUnit _ratingUnit;
		private readonly IUserUnit _userUnit;
		#endregion

		#region Ctor
		public EntityRatingRepository(IRatingUnit ratingUnit, IUserUnit userUnit)
		{
			_ratingUnit = ratingUnit;
			_userUnit = userUnit;
		}
		#endregion

		#region IRatingRepository members
		public async Task AddOrUpdate(Guid userGuid, string productId, byte ratingValue)
		{
			using (var scope = new TransactionScope())
			{
				User user = await _userUnit.Get(userGuid);
				if (user == null)
					throw new ObjectNotFoundException();

				Rating rating = await _ratingUnit.Get(user.UserId, productId) ??
								new Rating
								{
									User = user,
									ProductId = productId,
								};
				rating.Value = ratingValue;

				await _ratingUnit.AddOrUpdate(rating);

				scope.Complete();
			}
		}

		public async Task Delete(Guid userGuid, string productId)
		{
			using (var scope = new TransactionScope())
			{
				Rating rating = await _ratingUnit.Get(userGuid, productId);

				if (rating == null)
					throw new ObjectNotFoundException();

				await _ratingUnit.Remove(rating);

				scope.Complete();
			}
		}

		public async Task<IRating> Search(Guid userGuid, string productId)
		{
			return await _ratingUnit.Get(userGuid, productId);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_ratingUnit.Dispose();
			_userUnit.Dispose();
		}
		#endregion
	}
}