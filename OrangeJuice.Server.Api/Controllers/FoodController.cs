using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class FoodController : ApiController
	{
		private readonly IFoodRepository _foodRepository;

		public FoodController(IFoodRepository foodRepository)
		{
			_foodRepository = foodRepository;
		}

		/// <summary>
		/// Searches for food by text
		/// </summary>
		/// <returns>Collection of food description</returns>
		/// <url>GET /api/food/?title={title}</url>
		public async Task<IHttpActionResult> GetByTitle([FromUri]string title)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var description = await _foodRepository.SearchByTitle(title);
			return Ok(description);
		}

		/// <summary>
		/// Searches for food by barcode
		/// </summary>
		/// <returns>Single food description</returns>
		/// <url>GET /api/food/?barcode={barcode}</url>
		public async Task<IHttpActionResult> GetByBarcode([FromUri]string barcode)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var description = await _foodRepository.SearchByBarcode(barcode);
			return Ok(description);
		}
	}
}