using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

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

		#region Methods
		/// <url>GET /api/rating</url>
		[ResponseType(typeof(IRating))]
		public async Task<IHttpActionResult> GetRating([FromUri] RatingSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			IRating rating = await _ratingRepository.Search(searchCriteria.UserGuid, searchCriteria.Productid);
			if (rating == null)
				return NotFound();

			return Ok(rating);
		}

		/// <url>POST /api/rating</url>
		public async Task<IHttpActionResult> PostRating([FromUri] RatingInformation ratingInformation)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _ratingRepository.AddOrUpdate(ratingInformation.UserGuid, ratingInformation.Productid, (byte)ratingInformation.Value);

			return Ok();
		}

		/// <url>DELETE /api/rating</url>
		public async Task<IHttpActionResult> DeleteRating([FromUri] RatingSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _ratingRepository.Delete(searchCriteria.UserGuid, searchCriteria.Productid);

			return Ok();
		}
		#endregion
	}
}