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
		private readonly IAwsProvider _provider;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;
		private readonly IFilter<FoodDescription> _foodDescriptionFilter;

		public AwsFoodRepository(IAwsProvider provider, IFoodDescriptionFactory foodDescriptionFactory, IFilter<FoodDescription> foodDescriptionFilter)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			if (foodDescriptionFactory == null)
				throw new ArgumentNullException("foodDescriptionFactory");
			if (foodDescriptionFilter == null)
				throw new ArgumentNullException("foodDescriptionFilter");

			_provider = provider;
			_foodDescriptionFactory = foodDescriptionFactory;
			_foodDescriptionFilter = foodDescriptionFilter;
		}

		public async Task<IEnumerable<FoodDescription>> SearchByTitle(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			var items = await _provider.SearchItem(title);

			var tasks = from item in items
						from e in item.Elements(item.Name.Namespace + "ASIN")
						select CreateDescription(e.Value);

			return await Task.WhenAll(tasks)
							 .ContinueWith(t => t.Result.Where(_foodDescriptionFilter.Filter));
		}

		private Task<FoodDescription> CreateDescription(string id)
		{
			return _foodDescriptionFactory.Create(
				id,
				_provider.LookupAttributes(id),
				_provider.LookupImages(id));
		}
	}
}