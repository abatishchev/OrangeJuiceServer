using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IAwsClientFactory _awsClientFactory;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;

		public AwsFoodRepository(IAwsClientFactory awsClientFactory, IFoodDescriptionFactory foodDescriptionFactory)
		{
			if (awsClientFactory == null)
				throw new ArgumentNullException("awsClientFactory");
			if (foodDescriptionFactory == null)
				throw new ArgumentNullException("foodDescriptionFactory");

			_awsClientFactory = awsClientFactory;
			_foodDescriptionFactory = foodDescriptionFactory;
		}

		public async Task<IEnumerable<FoodDescription>> Find(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			IAwsClient apiClient = _awsClientFactory.Create();

			IEnumerable<string> asins = await apiClient.ItemSearch(title);

			XElement[] items = await Task.WhenAll(asins.Select(apiClient.ItemLookup));

			return items.Select(item => _foodDescriptionFactory.Create(item));
		}
	}
}