using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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
		/// <returns>Collection of food description</returns>
		/// <url>POST /api/food</url>
		[ActionName("title")]
		public async Task<IHttpActionResult> PostTitle(TitleSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var description = await _foodRepository.Search(searchCriteria.Title);
			return Ok(description.ToArray());
		}

		/// <summary>
		/// Searches for food by barcode
		/// </summary>
		/// <returns>Single food description</returns>
		/// <url>POST /api/food</url>
		[ActionName("barcode")]
		public async Task<IHttpActionResult> PostBarcode(BarcodeSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var description = await _foodRepository.Lookup(searchCriteria.Barcode, searchCriteria.BarcodeType);
			return Ok(description);
		}
	}
}