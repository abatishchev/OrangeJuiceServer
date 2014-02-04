﻿using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class RatingController : ApiController
	{
		#region Fields
		private readonly IRatingRepository _ratingRepository;
		#endregion

		#region Ctor
		public RatingController(IRatingRepository ratingRepository)
		{
			_ratingRepository = ratingRepository;
		}
		#endregion

		#region HTTP methods
		/// <summary>
		/// Searches for product rating by rating id
		/// </summary>
		/// <returns>Rating entity</returns>
		/// <url>GET /api/rating/?userId={guid}&productId={guid}</url>
		public async Task<IHttpActionResult> GetRating(RatingId ratingId)
		{
			IRating rating = await _ratingRepository.Search(ratingId);
			if (rating == null)
				return NotFound();

			return Ok(rating);
		}

		/// <summary>
		/// Adds or updates product rating
		/// </summary>
		/// <returns>Rating id</returns>
		/// <url>POST /api/rating</url>
		public async Task<IHttpActionResult> PostRating(RatingModel ratingModel)
		{
			RatingId ratingId = new RatingId { UserId = ratingModel.UserId, ProductId = ratingModel.ProductId };

			await _ratingRepository.AddOrUpdate(ratingId, ratingModel.Value, ratingModel.Comment);

			return Ok(ratingId);
		}

		/// <summary>
		/// Deletes product rating by rating id
		/// </summary>
		/// <returns>200 OK</returns>
		/// <url>DELETE /api/rating</url>
		public async Task<IHttpActionResult> DeleteRating(RatingId ratingId)
		{
			await _ratingRepository.Delete(ratingId);

			return Ok();
		}
		#endregion

		#region Methods
		protected override void Dispose(bool disposing)
		{
			_ratingRepository.Dispose();

			base.Dispose(disposing);
		}
		#endregion
	}
}