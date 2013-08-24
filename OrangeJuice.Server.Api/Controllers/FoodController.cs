using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public class FoodController : ApiController
	{
		private readonly IFoodRepository _foodRepository;

		public FoodController(IFoodRepository foodRepository)
		{
			if (foodRepository == null)
				throw new ArgumentNullException("foodRepository");
			_foodRepository = foodRepository;
		}

		/// <url>GET api/food/</url>
		public async Task<HttpResponseMessage> GetDescription([FromUri]FoodSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("searchCriteria"));

			if (!ModelValidator.Current.IsValid(this.ModelState))
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is not valid");

			var description = await _foodRepository.Find(searchCriteria.Title);
			return Request.CreateResponse(HttpStatusCode.OK, description);
		}
	}
}