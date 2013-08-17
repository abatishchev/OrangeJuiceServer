using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;

namespace OrangeJuice.Server.Api.Controllers
{
	public class FoodController : ApiController
	{
		private readonly GroceryDescriptionFactory _groceryDescriptionFactory;
		private readonly AwsClientFactory _awsClientFactory;

		public FoodController(AwsClientFactory awsClientFactory, GroceryDescriptionFactory groceryDescriptionFactory)
		{
			if (awsClientFactory == null)
				throw new ArgumentNullException("awsClientFactory");
			if (groceryDescriptionFactory == null)
				throw new ArgumentNullException("groceryDescriptionFactory");

			_awsClientFactory = awsClientFactory;
			_groceryDescriptionFactory = groceryDescriptionFactory;
		}

		/// <url>GET api/food/</url>
		public async Task<IEnumerable<GroceryDescription>> GetDescription(string text)
		{
			AwsClient apiClient = _awsClientFactory.Create();

			IEnumerable<string> asins = await apiClient.ItemSearch(text);

			XElement[] items = await Task.WhenAll(asins.Select(apiClient.ItemLookup));

			return items.Select(item => _groceryDescriptionFactory.Create(item));
		}
	}
}