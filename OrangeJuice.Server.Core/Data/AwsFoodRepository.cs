using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IAwsClient _awsClient;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;
		private readonly IFilter<IEnumerable<FoodDescription>> _foodDescriptionFilter;

		public AwsFoodRepository(IAwsClient awsClient, IFoodDescriptionFactory foodDescriptionFactory, IFilter<IEnumerable<FoodDescription>> foodDescriptionFilter)
		{
			if (awsClient == null)
				throw new ArgumentNullException("awsClient");
			if (foodDescriptionFactory == null)
				throw new ArgumentNullException("foodDescriptionFactory");
			if (foodDescriptionFilter == null)
				throw new ArgumentNullException("foodDescriptionFilter");

			_awsClient = awsClient;
			_foodDescriptionFactory = foodDescriptionFactory;
			_foodDescriptionFilter = foodDescriptionFilter;
		}

		public async Task<FoodDescription[]> SearchByTitle(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			var ids = await _awsClient.SearchItem(title);

			var tasks = ids.Select(CreateDescription);
			return await Task.WhenAll(tasks)
			                 .ContinueWith(t => _foodDescriptionFilter.Filter(t.Result)
			                                                          .ToArray());
		}

		private Task<FoodDescription> CreateDescription(string id)
		{
			return _foodDescriptionFactory.Create(
				id,
				_awsClient.LookupAttributes(id),
				_awsClient.LookupImages(id));
		}
	}
}