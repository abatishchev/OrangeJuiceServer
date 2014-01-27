using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class FoodController : ApiController
	{
		#region Fields
		private readonly IFoodRepository _foodRepository;
		#endregion

		#region Ctor
		public FoodController(IFoodRepository foodRepository)
		{
			_foodRepository = foodRepository;
		}
		#endregion

		#region HTTP methods
		/// <summary>
		/// Searches for food by text
		/// </summary>
		/// <returns>Collection of food descriptors</returns>
		/// <url>POST /api/food</url>
		[ActionName("title")]
		public async Task<IHttpActionResult> PostTitle(TitleSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var descriptors = await _foodRepository.Search(searchCriteria.Title);
			return Ok(descriptors.ToArray());
		}

		/// <summary>
		/// Searches for food by barcode
		/// </summary>
		/// <returns>Single food descriptor</returns>
		/// <url>POST /api/food</url>
		[ActionName("barcode")]
		public async Task<IHttpActionResult> PostBarcode(BarcodeSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var descriptor = await _foodRepository.Lookup(searchCriteria.Barcode, searchCriteria.BarcodeType);
			return Ok(descriptor);
		}
		#endregion
	}
}