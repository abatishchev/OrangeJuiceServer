using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using OrangeJuice.Server.Api.Models;
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
		/// <returns>Brief description of food found</returns>
		/// <param name="searchCriteria">Food search criteria</param>
		/// <url>GET /api/food/</url>
		[ResponseType(typeof(FoodDescription))]
		public async Task<IHttpActionResult> GetDescription([FromUri]FoodSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var description = await _foodRepository.SearchByTitle(searchCriteria.Title);
			return Ok(description.ToArray());
		}
	}
}