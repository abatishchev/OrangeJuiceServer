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
		/// <summary>
		/// Searches for food rating by product id and user guid
		/// </summary>
		/// <returns>Rating entity</returns>
		/// <url>GET /api/rating</url>
		public async Task<IHttpActionResult> GetRating([FromUri] RatingId ratingId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			IRating rating = await _ratingRepository.Search(ratingId);
			if (rating == null)
				return NotFound();

			return Ok(rating);
		}

		/// <summary>
		/// Adds or updates rating basing on its existence by product id and user guid
		/// </summary>
		/// <returns>200 OK</returns>
		/// <url>POST /api/rating</url>
		public async Task<IHttpActionResult> PostRating([FromBody] Rating rating)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _ratingRepository.AddOrUpdate(rating.RatingId, (byte)rating.Value, rating.Comment);

			return Ok();
		}

		/// <summary>
		/// Deletes rating by product id and user guid
		/// </summary>
		/// <returns>200 OK</returns>
		/// <url>DELETE /api/rating</url>
		public async Task<IHttpActionResult> DeleteRating(RatingId ratingId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

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