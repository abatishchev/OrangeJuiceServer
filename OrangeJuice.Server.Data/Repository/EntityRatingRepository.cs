using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityRatingRepository : IRatingRepository
	{
		#region Fields
		private readonly IRatingUnit _ratingUnit;
		#endregion

		#region Ctor
		public EntityRatingRepository(IRatingUnit ratingUnit)
		{
			_ratingUnit = ratingUnit;
		}
		#endregion

		#region IRatingRepository members
		public async Task AddOrUpdate(Guid userId, Guid productId, byte ratingValue, string comment)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				Rating rating = await _ratingUnit.Get(userId, productId) ??
								new Rating
								{
									UserId = userId,
									ProductId = productId,
								};
				rating.Value = ratingValue;
				rating.Comment = comment;

				await _ratingUnit.AddOrUpdate(rating);

				scope.Complete();
			}
		}

		public async Task Delete(Guid userId, Guid productId)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				Rating rating = await _ratingUnit.Get(userId, productId);

				if (rating == null)
					throw new ObjectNotFoundException();

				await _ratingUnit.Delete(rating);

				scope.Complete();
			}
		}

		public async Task<IRating> Search(Guid userId, Guid productId)
		{
			return await _ratingUnit.Get(userId, productId);
		}

		public async Task<IRating[]> SearchAll(Guid productId)
		{
			var ratings = await _ratingUnit.Get(productId);
			return ratings != null ?
				ratings.Cast<IRating>().ToArray() :
				null;
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_ratingUnit.Dispose();
		}
		#endregion
	}
}