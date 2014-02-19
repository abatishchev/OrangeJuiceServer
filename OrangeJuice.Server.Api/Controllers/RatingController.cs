using System;
using System.Threading.Tasks;
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
		public async Task<IHttpActionResult> GetRating([FromUri]RatingSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			IRating rating = await _ratingRepository.Search(searchCriteria.UserId, searchCriteria.ProductId);
			if (rating == null)
				return NotFound();

			return Ok(rating);
		}

		[Route("api/product/{productId}/rating")]
		public async Task<IHttpActionResult> GetRatings([FromUri]RatingsSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			var ratings = await _ratingRepository.SearchAll(searchCriteria.ProductId);
			if (ratings == null)
				return NotFound();

			return Ok(ratings);
		}

		public async Task<IHttpActionResult> PostRating(RatingModel ratingModel)
		{
			if (ratingModel == null)
				throw new ArgumentNullException();

			await _ratingRepository.AddOrUpdate(ratingModel.UserId, ratingModel.ProductId, ratingModel.Value, ratingModel.Comment);

			return Ok();
		}

		public async Task<IHttpActionResult> DeleteRating([FromUri]RatingSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			await _ratingRepository.Delete(searchCriteria.UserId, searchCriteria.ProductId);

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