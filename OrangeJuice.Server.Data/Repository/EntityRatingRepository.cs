﻿using System;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using OrangeJuice.Server.Data.Context;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityRatingRepository : IRatingRepository
	{
		#region Fields
		private readonly IModelContext _db;
		#endregion

		#region Ctor
		public EntityRatingRepository(IModelContext db)
		{
			_db = db;
		}
		#endregion

		#region IRatingRepository members
		public async Task AddOrUpdate(Guid userId, Guid productId, byte ratingValue, string comment)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				Rating rating = await _db.Ratings.FindAsync(userId, productId) ??
								new Rating
								{
									UserId = userId,
									ProductId = productId,
								};
				rating.Value = ratingValue;
				rating.Comment = comment;

				_db.Ratings.AddOrUpdate(rating);
				await _db.SaveChangesAsync();

				scope.Complete();
			}
		}

		public async Task Delete(Guid userId, Guid productId)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				Rating rating = await _db.Ratings.FindAsync(userId, productId);

				if (rating == null)
					throw new ObjectNotFoundException();

				_db.Ratings.Remove(rating);

				await _db.SaveChangesAsync();

				scope.Complete();
			}
		}

		public async Task<IRating> Search(Guid userId, Guid productId)
		{
			return await _db.Ratings.FindAsync(userId, productId);
		}

		public async Task<IRating[]> SearchAll(Guid productId)
		{
			Product product = await _db.Products.FindAsync(productId);
			return product != null ? product.Ratings.ToArray() : null;
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