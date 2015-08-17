using System;
using System.Threading.Tasks;
using System.Web.Http;

using Ab.Web;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Api.Controllers
{
	[Authorize]
	public sealed class RatingController : ApiController
	{
		#region Fields
		private readonly IRatingRepository _ratingRepository;
		private readonly IUrlProvider _urlProvider;
		#endregion

		#region Ctor
		public RatingController(IRatingRepository ratingRepository, IUrlProvider urlProvider)
		{
			_ratingRepository = ratingRepository;
			_urlProvider = urlProvider;
		}
		#endregion

		#region HTTP methods
		[Route("api/rating")]
		public async Task<IHttpActionResult> GetRating([FromUri]RatingSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			Rating rating = await _ratingRepository.Search(searchCriteria.UserId, searchCriteria.ProductId);
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

		[Route("api/rating")]
		public async Task<IHttpActionResult> PostRating(RatingModel ratingModel)
		{
			if (ratingModel == null)
				throw new ArgumentNullException();

			await _ratingRepository.AddOrUpdate(ratingModel.UserId, ratingModel.ProductId, ratingModel.Value, ratingModel.Comment);

			var url = _urlProvider.UriFor<RatingController>(c => c.GetRating(new RatingSearchCriteria { ProductId = ratingModel.ProductId, UserId = ratingModel.UserId }));
			return Created(url, ratingModel);
		}

		[Route("api/rating")]
		public async Task<IHttpActionResult> DeleteRating([FromUri]RatingSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			await _ratingRepository.Delete(searchCriteria.UserId, searchCriteria.ProductId);

			return Ok();
		}
		#endregion
	}
}