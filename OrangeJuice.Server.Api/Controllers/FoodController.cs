using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;
using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Controllers
{
	public class FoodController : ApiController
	{
		private readonly IAwsClientFactory _awsClientFactory;
		private readonly IGroceryDescriptionFactory _groceryDescriptionFactory;

		public FoodController(IAwsClientFactory awsClientFactory, IGroceryDescriptionFactory groceryDescriptionFactory)
		{
			if (awsClientFactory == null)
				throw new ArgumentNullException("awsClientFactory");
			if (groceryDescriptionFactory == null)
				throw new ArgumentNullException("groceryDescriptionFactory");

			_awsClientFactory = awsClientFactory;
			_groceryDescriptionFactory = groceryDescriptionFactory;
		}

		/// <url>GET api/food/</url>
		public async Task<HttpResponseMessage> GetDescription([FromUri]GrocerSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("searchCriteria"));

			if (!ModelValidator.Current.IsValid(this.ModelState))
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is not valid");

			AwsClient apiClient = _awsClientFactory.Create();

			IEnumerable<string> asins = await apiClient.ItemSearch(searchCriteria.Title);

			XElement[] items = await Task.WhenAll(asins.Select(apiClient.ItemLookup));

			return Request.CreateResponse(HttpStatusCode.OK, items.Select(item => _groceryDescriptionFactory.Create(item)));
		}
	}
}