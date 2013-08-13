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
		private readonly AwsOptions _awsOptions;
		private readonly GroceryDescriptionFactory _groceryDescriptionFactory;

		public FoodController(AwsOptions awsOptions, GroceryDescriptionFactory groceryDescriptionFactory)
		{
			if (awsOptions == null)
				throw new ArgumentNullException("awsOptions");
			if (groceryDescriptionFactory == null)
				throw new ArgumentNullException("groceryDescriptionFactory");

			_awsOptions = awsOptions;
			_groceryDescriptionFactory = groceryDescriptionFactory;
		}

		/// <url>GET api/food/</url>
		public async Task<IEnumerable<GroceryDescription>> GetDescription(string title)
		{
			AwsClient apiClient = new AwsClient(_awsOptions);

			IEnumerable<string> asins = await apiClient.ItemSearch(title);

			XElement[] items = await Task.WhenAll(asins.Select(apiClient.ItemLookup));

			return items.Select(item => _groceryDescriptionFactory.Create(item));
		}
	}
}