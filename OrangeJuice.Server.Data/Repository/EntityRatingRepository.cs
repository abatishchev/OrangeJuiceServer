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
		public async Task AddOrUpdate(RatingId ratingId, byte ratingValue, string comment)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				Rating rating = await _ratingUnit.Get(ratingId) ??
								new Rating
								{
									User = await _userUnit.Get(ratingId.UserId),
									ProductId = ratingId.ProductId,
								};
				rating.Value = ratingValue;

				await _ratingUnit.AddOrUpdate(rating);

				scope.Complete();
			}
		}

		public async Task Delete(RatingId ratingId)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				Rating rating = await _ratingUnit.Get(ratingId);

				if (rating == null)
					throw new ObjectNotFoundException();

				await _ratingUnit.Remove(rating);

				scope.Complete();
			}
		}

		public async Task<IRating> Search(RatingId ratingId)
		{
			return await _ratingUnit.Get(ratingId);
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